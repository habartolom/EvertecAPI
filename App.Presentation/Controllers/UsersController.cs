using App.Application.Models.Contracts;
using App.Application.Services.Users;
using App.Domain.AggregatesModel.UserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Presentation.Controllers
{
	/// <summary>
	/// Users Controller
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="userService"></param>
		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		/// <summary>
		/// Endpoint for get all users
		/// </summary>
		/// <returns>List with all users</returns>
		[HttpGet("All")]
		public TypedResponseContract<List<User>> GetAll()
		{
			return _userService.GetAllUsers();
		}

		/// <summary>
		/// Endpoint for create an user
		/// </summary>
		/// <param name="userCreateRequest"></param>
		/// <returns>Success response with created user</returns>
		[HttpPost("Create")]
		public Task<TypedResponseContract<User>> CreateUser([FromForm] UserCreateRequestContract userCreateRequest)
		{
			return _userService.CreateUser(userCreateRequest);
		}

		/// <summary>
		/// Endpoint for delete an user
		/// </summary>
		/// <param name="userCreateRequest"></param>
		/// <returns>Success response with edited user</returns>
		[HttpDelete("Delete")]
		public Task<ResponseContract> DeleteUser(Guid userId)
		{
			return _userService.DeleteUser(userId);
		}

		/// <summary>
		/// Endpoint for edit an user
		/// </summary>
		/// <param name="userEditRequest"></param>
		/// <returns>Success response with edited user</returns>
		[HttpPut("Edit")]
		public Task<TypedResponseContract<User>> EditUser([FromForm] UserEditRequestContract userEditRequest)
		{
			return _userService.EditUser(userEditRequest);
		}

		/// <summary>
		/// Endpoint for get an user by Id
		/// </summary>
		/// <returns>Success response with user</returns>
		[HttpGet("Get/{userId}")]
		public Task<TypedResponseContract<User>> GetUser(Guid userId)
		{
			return _userService.GetUserById(userId);
		}

		/// <summary>
		/// Endpoint for get user photo
		/// </summary>
		/// <returns>Success response with photos</returns>
		[HttpGet("Photo/{fileName}")]
		public IActionResult GetUserPhoto(string fileName)
		{
			TypedResponseContract<FileContract> response = _userService.GetUserPhoto(fileName);
			if (response.Header.Code != System.Net.HttpStatusCode.OK)
				return Ok(response);

			MemoryStream memoryStream = new MemoryStream(response.Data!.FileData);
			return new FileStreamResult(memoryStream, response.Data.ContentType);
		}
	}
}
