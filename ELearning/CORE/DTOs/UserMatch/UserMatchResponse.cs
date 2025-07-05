using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.DTOs.UserMatch
{
    public class UserMatchResponse
    {
        public int Id { get; set; }
        public int UserId1 { get; set; }
        public string UserName1 { get; set; } = string.Empty;
        public string? ImagePath1 { get; set; }
        public int UserId2 { get; set; }
        public string UserName2 { get; set; } = string.Empty;
        public string? ImagePath2 { get; set; }
        public string MatchType { get; set; } = string.Empty; // "text", "voice", "video"
        public double MatchScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
