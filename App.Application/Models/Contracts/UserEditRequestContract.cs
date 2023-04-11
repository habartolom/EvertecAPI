using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Models.Contracts
{
	public class UserEditRequestContract
	{
		public Guid UserId { get; set; }
		public string? Name { get; set; }
		public string? Lastname { get; set; }
		public DateTime? Birthdate { get; set; }
		public IFormFile? Photo { get; set; }
		public int? CivilStatusId { get; set; }
		public bool? HasSilbings { get; set; }
	}
}
