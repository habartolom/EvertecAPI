using App.Application.Models.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services.Files
{
	public interface IFileService
	{
		TypedResponseContract<FileContract> GetFile(string filePath);
		ResponseContract RemoveFile(string filePath);
		Task<TypedResponseContract<string>> SaveFile(IFormFile file, string directoryPath);
	}
}
