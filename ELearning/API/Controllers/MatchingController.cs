using CORE.DTOs.UserMatch;
using CORE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Handles operations related to user matching, such as finding, retrieving, and ending matches
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;
        public MatchingController(IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }
        /// <summary>
        /// Gets the current authenticated user's ID from the JWT claims.
        /// </summary>
        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        /// <summary>
        /// Initiates a match request for the current user based on the specified match type.
        /// </summary>
        /// <remarks>
        /// Finds a compatible user based on language preferences, shared interests, and online status.
        /// Match types can be "text", "voice", or "video".
        /// </remarks>
        /// <param name="request">The match request containing the user ID and match type.</param>
        /// <returns>The created match details.</returns>
        /// <response code="200">Returns the match details</response>
        /// <response code="401">If the user is not authenticated or requests a match for another user</response>
        /// <response code="404">If no suitable match is found</response>
        [HttpPost("find-match")]
        public async Task<IActionResult> FindMatch([FromBody] UserMatchRequest request)
        {
            int userId = GetCurrentUserId();
            if (request.UserId != userId)
                return Unauthorized("You can only request matches for yourself");

            var match = await _matchingService.FindMatchAsync(request);
            return Ok(match);
        }

        /// <summary>
        /// Retrieves all active matches for the current user.
        /// </summary>
        /// <remarks>
        /// Returns a list of active matches, including details of the matched users and match type.
        /// </remarks>
        /// <returns>A list of active matches.</returns>
        /// <response code="200">Returns the list of matches</response>
        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            int userId = GetCurrentUserId();
            var matches = await _matchingService.GetMatchesAsync(userId);
            return Ok(matches);
        }

        /// <summary>
        /// Ends an active match by its ID.
        /// </summary>
        /// <remarks>
        /// Only the user involved in the match can end it. Sets the match's IsActive to false.
        /// </remarks>
        /// <param name="matchId">The ID of the match to end.</param>
        /// <returns>No content if successful, or bad request if failed.</returns>
        /// <response code="204">If the match was successfully ended</response>
        /// <response code="401">If the user is not authorized to end the match</response>
        /// <response code="404">If the match is not found</response>
        [HttpPut("end/{matchId:int}")]
        public async Task<IActionResult> EndMatch(int matchId)
        {
            int userId = GetCurrentUserId();
            var matches = await _matchingService.GetMatchesAsync(userId);
            if (!matches.Any(m => m.Id == matchId))
                return Unauthorized("You can only end your own matches");

            var success = await _matchingService.EndMatchAsync(matchId);
            return success ? NoContent() : BadRequest("Failed to end match");
        }
    }
}
