using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.DTOs.UserMatch
{
    public class UserMatchRequest
    {
        public int UserId { get; set; } //initiating user
        public string MatchType { get; set; } = "text"; //default to text
    }
}
