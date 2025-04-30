using CORE.Helpers;
using CORE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPut("user-language-preferences")]
        [Authorize]
        public async Task<IActionResult> UpdateUserLanguagePreferences([FromBody] HashSet<int> LanguagesIds)
        {
            var userId = UserHelpers.GetUserId(User);

            var result = await _userService.UpdateUserLanguagePreferences(LanguagesIds, userId);

            return StatusCode(result.StatusCode, result);
        }
    }
}
