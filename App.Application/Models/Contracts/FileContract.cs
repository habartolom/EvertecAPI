using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Models.Contracts
{
	public class FileContract
	{
		public byte[] FileData { get; set; } = null!;
		public string ContentType { get; set; } = null!;
	}
}
