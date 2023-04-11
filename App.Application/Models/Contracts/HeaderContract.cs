using System.Net;

namespace App.Application.Models.Contracts
{
	public class HeaderContract
	{
		public HttpStatusCode Code { get; set; }
		public string Message { get; set; } = null!;
	}
}
