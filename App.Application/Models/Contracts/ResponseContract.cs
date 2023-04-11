using System.Net;

namespace App.Application.Models.Contracts
{
	public class ResponseContract
	{
		private HeaderContract header;
		public HeaderContract Header
		{
			get
			{
				if (header is null)
				{
					header = new HeaderContract();
					header.Code = HttpStatusCode.OK;
				}
				return header;
			}

			set
			{
				header = value;
			}
		}
	}
}
