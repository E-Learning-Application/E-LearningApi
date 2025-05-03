using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.DTOs.User
{
    public class GetUserSummeryDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? ImagePath { get; set; }
    }
}
