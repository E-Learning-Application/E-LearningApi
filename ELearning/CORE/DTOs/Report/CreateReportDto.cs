using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATA.Models;

namespace CORE.DTOs.Report
{
    public class CreateReportDto
    {
        public int ReportedId { get; set; }
        public string Reason { get; set; }
    }
}
