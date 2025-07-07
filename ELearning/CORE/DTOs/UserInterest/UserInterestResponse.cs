using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.DTOs.Interest
{
    public class UserInterestResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int InterestId { get; set; }
        public string InterestName { get; set; } = string.Empty;
    }
}
