using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CORE.DTOs.User
{
    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string? Bio { get; set; }
        public IFormFile? Image { get; set; }
    }
}
