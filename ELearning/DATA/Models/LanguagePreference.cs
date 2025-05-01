using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATA.Models.Enums;

namespace DATA.Models
{
    public class LanguagePreference
    {
        public int UserId { get; set; }
        public AppUser AppUser { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public LanguageProficiencyLevel ProficiencyLevel { get; set; }
        public bool IsLearning { get; set; }
    }
}
