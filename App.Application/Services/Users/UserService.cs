using App.Application.Models.Contracts;
using App.Application.Services.Files;
using App.Domain.AggregatesModel.CivilStatusAggregate;
using App.Domain.AggregatesModel.UserAggregate;
using App.Infrastructure.Database.Entities;
using App.Infrastructure.Repositories.EntityFramework;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Application.Services.Users
{
	public class UserService : IUserService
	{
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<UserService> _logger;
		private readonly IMapper _mapper;
		private readonly ICivilStatusRepository<CivilStatusEntity> _civilStatusRepository;
		private readonly IUserRepository<UserEntity> _userRepository;
		private readonly IFileService _fileService;
		private readonly string _storagePath;

		public UserService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger, IMapper mapper, ICivilStatusRepository<CivilStatusEntity> civilStatusRepository, IUserRepository<UserEntity> userRepository, IFileService fileService)
		{
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
			_logger = logger;
			_mapper = mapper;
			_civilStatusRepository = civilStatusRepository;
			_userRepository = userRepository;
			_fileService = fileService;
			_storagePath = _configuration.GetSection("Storage:UserPhotos").Value!;
		}

		public async Task<TypedResponseContract<User>> CreateUser(UserCreateRequestContract userCreateRequest)
		{
			TypedResponseContract<User> response = new TypedResponseContract<User>();
			try
			{
				TypedResponseContract<string> saveFileResponse = await _fileService.SaveFile(userCreateRequest.Photo, _storagePath);
				if(saveFileResponse.Header.Code == HttpStatusCode.OK)
				{
					UserEntity userEntity = _mapper.Map<UserEntity>(userCreateRequest);
					userEntity.UserId = Guid.NewGuid();
					userEntity.FileName = userCreateRequest.Photo.FileName;
					userEntity.UniqueFileName = saveFileResponse.Data!;
					
					userEntity = await _userRepository.CreateAsync(userEntity);
					CivilStatusEntity civilStatus = await _civilStatusRepository.FindByIdAsync(userEntity.CivilStatusId);

					User user = _mapper.Map<User>(userEntity);
					user.PhotoUrl = GetUserPhotoURL(userEntity.UniqueFileName);
					user.CivilStatus = _mapper.Map<CivilStatus>(civilStatus);
					response.Data = user;
				}
				else
				{
					response.Header = saveFileResponse.Header;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred!");
				response.Header.Code = HttpStatusCode.InternalServerError;
				response.Header.Message = ex.ToString();
			}
			return response;
		}

		public async Task<ResponseContract> DeleteUser(Guid userId)
		{
			ResponseContract response = new ResponseContract();
			try
			{
				UserEntity userEntity = await _userRepository.FindByIdAsync(userId);
				await _userRepository.DeleteAsync(userEntity);

				string filePath = Path.Combine(_storagePath, userEntity.UniqueFileName);
				ResponseContract removeFileResponse = _fileService.RemoveFile(filePath);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred!");
				response.Header.Code = HttpStatusCode.InternalServerError;
				response.Header.Message = ex.ToString();
			}
			return response;
		}

		public async Task<TypedResponseContract<User>> EditUser(UserEditRequestContract userEditRequest)
		{
			TypedResponseContract<User> response = new TypedResponseContract<User>();
			try
			{
				UserEntity userEntity = await _userRepository.FindByIdAsync(userEditRequest.UserId);
				string userEditRequestJsonString = JsonConvert.SerializeObject(userEditRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
				JsonConvert.PopulateObject(userEditRequestJsonString, userEntity);

				if(userEditRequest.Photo != null)
				{
					TypedResponseContract<string> saveFileResponse = await _fileService.SaveFile(userEditRequest.Photo, _storagePath);
					if (saveFileResponse.Header.Code == HttpStatusCode.OK)
					{
						userEntity.FileName = userEditRequest.Photo.FileName;
						userEntity.UniqueFileName = saveFileResponse.Data!;
					}
					else
					{
						response.Header = saveFileResponse.Header;
					}
				}

				userEntity = await _userRepository.UpdateAsync(userEntity);
				CivilStatusEntity civilStatus = await _civilStatusRepository.FindByIdAsync(userEntity.CivilStatusId);

				User user = _mapper.Map<User>(userEntity);
				user.PhotoUrl = GetUserPhotoURL(userEntity.UniqueFileName);
				user.CivilStatus = _mapper.Map<CivilStatus>(civilStatus);
				response.Data = user;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred!");
				response.Header.Code = HttpStatusCode.InternalServerError;
				response.Header.Message = ex.ToString();
			}
			return response;
		}

		public TypedResponseContract<List<User>> GetAllUsers()
		{
			TypedResponseContract<List<User>> response = new TypedResponseContract<List<User>>();
			try
			{
				List<User> users = new List<User>();
				List<UserEntity> userEntities = _userRepository.GetAll().Include(x => x.CivilStatus).ToList();
				foreach (UserEntity userEntity in userEntities)
				{
					string? userPhotoURL = GetUserPhotoURL(userEntity.UniqueFileName);
					User user = _mapper.Map<User>(userEntity);
					user.PhotoUrl = userPhotoURL;
					users.Add(user);
				}

				response.Data = users;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred!");
				response.Header.Code = HttpStatusCode.InternalServerError;
				response.Header.Message = ex.ToString();
			}
			return response;
		}

		public async Task<TypedResponseContract<User>> GetUserById(Guid userId)
		{
			TypedResponseContract<User> response = new TypedResponseContract<User>();
			try
			{
				UserEntity userEntity = await _userRepository.FindByIdAsync(userId);
				CivilStatusEntity civilStatus = await _civilStatusRepository.FindByIdAsync(userEntity.CivilStatusId);

				User user = _mapper.Map<User>(userEntity);
				user.PhotoUrl = GetUserPhotoURL(userEntity.UniqueFileName);
				user.CivilStatus = _mapper.Map<CivilStatus>(civilStatus);
				response.Data = user;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "An error occurred!");
				response.Header.Code = HttpStatusCode.InternalServerError;
				response.Header.Message = ex.ToString();
			}
			return response;
		}

		public TypedResponseContract<FileContract> GetUserPhoto(string fileName)
		{
			string filePath = Path.Combine(_storagePath, fileName);
			return _fileService.GetFile(filePath);
		}

		private string? GetUserPhotoURL(string fileName)
		{
			string filePath = Path.Combine(_storagePath, fileName);
			if (!File.Exists(filePath))
				return null;

			string httpRequestScheme = _httpContextAccessor.HttpContext.Request.Scheme;
			string httpRequestHost = _httpContextAccessor.HttpContext.Request.Host.Value;
			string appBaseUrl = $"{httpRequestScheme}://{httpRequestHost}";
			string photoUrl = $"{appBaseUrl}/api/Users/Photo/{fileName}";
			return photoUrl;
		}
	}
}
