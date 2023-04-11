using App.Application.Models.Contracts;
using App.Domain.AggregatesModel.UserAggregate;

namespace App.Application.Services.Users
{
	public interface IUserService
	{
		Task<TypedResponseContract<User>> CreateUser(UserCreateRequestContract userCreateRequest);
		Task<ResponseContract> DeleteUser(Guid userId);
		Task<TypedResponseContract<User>> EditUser(UserEditRequestContract userEditRequest);
		TypedResponseContract<List<User>> GetAllUsers();
		Task<TypedResponseContract<User>> GetUserById(Guid userId);
		TypedResponseContract<FileContract> GetUserPhoto(string fileName);
	}
}
