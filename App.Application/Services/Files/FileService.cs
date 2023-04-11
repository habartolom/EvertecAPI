using App.Application.Models.Contracts;
using App.Infrastructure.Repositories.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Application.Services.Files
{
	public class FileService : IFileService
	{
		private readonly ILogger<FileService> _logger;
		public FileService(ILogger<FileService> logger)
		{
			_logger = logger;
		}
		public TypedResponseContract<FileContract> GetFile(string filePath)
		{
			TypedResponseContract<FileContract> response = new TypedResponseContract<FileContract>();
			try
			{
				if (File.Exists(filePath))
				{
					byte[] data = File.ReadAllBytes(filePath);
					string contentType = MimeMapping.MimeUtility.GetMimeMapping(filePath);
					FileContract fileContract = new FileContract() { FileData = data, ContentType = contentType };
					response.Data = fileContract;
				}
				else{
					response.Header.Code = HttpStatusCode.NotFound;
					response.Header.Message = "Not Found";
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

		public ResponseContract RemoveFile(string filePath)
		{
			ResponseContract response = new ResponseContract();
			try
			{
				if(File.Exists(filePath))
					File.Delete(filePath);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred!");
				response.Header.Code = HttpStatusCode.InternalServerError;
				response.Header.Message = ex.ToString();
			}
			return response;
		}

		public async Task<TypedResponseContract<string>> SaveFile(IFormFile file, string directoryPath)
		{
			TypedResponseContract<string> response = new TypedResponseContract<string>();
			try
			{
				string strCurrentDate = DateTime.Now.ToString("yyyyMMddHHmmss");
				string strGuid = Guid.NewGuid().ToString();

				string fileName = file.FileName;
				string extension = Path.GetExtension(fileName);

				string uniqueFileName = $"{strCurrentDate}_{strGuid}{extension}";
				string filePath = Path.Combine(directoryPath, uniqueFileName);

				if (!Directory.Exists(directoryPath))
					Directory.CreateDirectory(directoryPath);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				response.Data = uniqueFileName;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred!");
				response.Header.Code = HttpStatusCode.InternalServerError;
				response.Header.Message = ex.ToString();
			}
			return response;
		}
	}
}
