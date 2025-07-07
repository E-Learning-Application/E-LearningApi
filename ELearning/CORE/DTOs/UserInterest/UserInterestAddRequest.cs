using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.DTOs.Interest
{
    public class UserInterestAddRequest
    {
        public int UserId { get; set; }
        public int InterestId { get; set; }
    }
}
