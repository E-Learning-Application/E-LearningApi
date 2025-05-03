using CORE.DTOs.User;
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
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int userId)
        {
            var result = await _userService.GetUserAsync(userId);
            return StatusCode(result.StatusCode, result);   
        }
        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var authUserId = UserHelpers.GetUserId(User);
            var roles = UserHelpers.GetUserRoles(User);
            var result = await _userService.DeleteUserAsync(userId, authUserId, roles);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdatePasswordDto dto)
        {
            var userId = UserHelpers.GetUserId(User);
            var result = await _userService.UpdateUserPasswordAsync(dto, userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto dto)
        {
            var userId = UserHelpers.GetUserId(User);
            var result = await _userService.UpdateUserAsync(dto, userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("all")]
        //[Authorize]
        public async Task<IActionResult> GetAllUsersAsync(int pageNo, int pageSize)
        {
            var result = await _userService.GetAllUsersAsync(pageNo, pageSize);
            return StatusCode(result.StatusCode, result);
        }
    }
}
