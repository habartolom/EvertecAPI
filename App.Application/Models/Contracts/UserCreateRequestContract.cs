using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Models.Contracts
{
    public class UserCreateRequestContract
    {
        public string Name { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public IFormFile Photo { get; set; } = null!;
        public int CivilStatusId { get; set; }
        public bool HasSilbings { get; set; }
    }
}
