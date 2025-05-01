using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.DTOs.Language
{
    public class GetLanguagePreferenceDto
    {
        public GetLanguageDto Language { get; set; }
        public string proficiencyLevel { get; set; }
        public bool IsLearning { get; set; }
    }
}
