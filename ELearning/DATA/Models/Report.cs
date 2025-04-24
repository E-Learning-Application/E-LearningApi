using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public AppUser Reporter { get; set; }
        public int ReportedId { get; set; }
        public AppUser Reported { get; set; }
        public string Reason { get; set; }
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        /*
         many to many with user (reportId, reporterId, reportedId)
         */
    }
}
