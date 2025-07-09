using CORE.DTOs.Interest;
using CORE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Handles operations related to interests, such as adding interests and managing user interests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InterestsController : ControllerBase
    {
        private readonly IInterestService _interestService;
        public InterestsController(IInterestService interestService)
        {
            _interestService = interestService;
        }

        /// <summary>
        /// Gets the current authenticated user's ID from the JWT claims.
        /// </summary>
        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        /// <summary>
        /// Retrieves all available interests in the system.
        /// </summary>
        /// <remarks>
        /// Returns a list of all interests, allowing clients to view available options for selection.
        /// </remarks>
        /// <returns>A list of all interests.</returns>
        /// <response code="200">Returns the list of all interests</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpGet("get-all-interests")]
        public async Task<IActionResult> GetAllInterests()
        {
            var interests = await _interestService.GetInterestsAsync();
            return Ok(interests);
        }

        /// <summary>
        /// Adds a new interest to the system.
        /// </summary>
        /// <remarks>
        /// Creates a new interest if it doesn't already exist (based on name).
        /// </remarks>
        /// <param name="request">The interest details to add.</param>
        /// <returns>The created or existing interest.</returns>
        [HttpPost]
        public async Task<IActionResult> AddInterest([FromBody] InterestAddRequest request)
        {
            var interest = await _interestService.AddInterestAsync(request);
            return Ok(interest);
        }

        /// <summary>
        /// Assigns an interest to the current user.
        /// </summary>
        /// <remarks>
        /// Links the specified interest to the authenticated user if not already linked.
        /// </remarks>
        /// <param name="request">The user interest details, including user ID and interest ID.</param>
        /// <returns>The created user interest.</returns>
        /// <response code="200">Returns the user interest details</response>
        /// <response code="401">If the user is not authorized to add interests for themselves</response>
        /// <response code="404">If the user or interest is not found</response>
        [HttpPost("user-interests")]
        public async Task<IActionResult> AddUserInterest([FromBody] UserInterestAddRequest request)
        {
            int userId = GetCurrentUserId();
            if (request.UserId != userId)
                return Unauthorized("You can only add interests for yourself");

            var userInterest = await _interestService.AddUserInterestAsync(request);
            return Ok(userInterest);
        }

        /// <summary>
        /// Retrieves all interests associated with the current user.
        /// </summary>
        /// <remarks>
        /// Returns a list of interests the authenticated user has selected.
        /// </remarks>
        /// <returns>A list of user interests.</returns>
        /// <response code="200">Returns the list of interests</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpGet("user-interests")]
        public async Task<IActionResult> GetUserInterests()
        {
            int userId = GetCurrentUserId();
            var interests = await _interestService.GetUserInterestsAsync(userId);
            return Ok(interests);
        }
    }
}
