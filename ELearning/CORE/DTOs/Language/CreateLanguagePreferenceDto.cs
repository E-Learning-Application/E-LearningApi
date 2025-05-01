using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DATA.Models.Enums;

namespace CORE.DTOs.Language
{
    public class CreateLanguagePreferenceDto
    {
        public int LanguageId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LanguageProficiencyLevel proficiencyLevel { get; set; }
        public bool IsLearning { get; set; }
    }
}
