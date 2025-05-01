using CORE.DTOs.Language;
using CORE.Helpers;
using CORE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagePreferenceController : ControllerBase
    {
        private readonly ILanguagePreferenceService _languagePreferenceService;

        public LanguagePreferenceController(ILanguagePreferenceService languagePreferenceService)
        {
            _languagePreferenceService = languagePreferenceService;
        }
        [HttpPut("update-user-language-preferences")]
        [Authorize]
        public async Task<IActionResult> UpdateUserLanguagePreferences([FromBody] List<CreateLanguagePreferenceDto> dtos)
        {
            int userid = UserHelpers.GetUserId(User);
            var result = await _languagePreferenceService.UpdateUserLanguagePreferences(dtos, userid);

            return StatusCode(result.StatusCode, result);   
        }
    }
}
