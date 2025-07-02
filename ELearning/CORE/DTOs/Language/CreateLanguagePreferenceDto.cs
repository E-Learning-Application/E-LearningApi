using System.Text.Json.Serialization;
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
