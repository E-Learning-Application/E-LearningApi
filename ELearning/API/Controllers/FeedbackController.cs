using CORE.DTOs.Feedback;
using CORE.Helpers;
using CORE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackDto feedbackDto)
        {
            var userId = UserHelpers.GetUserId(User);

            var result = await _feedbackService.CreateFeedbackAsync(feedbackDto, userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllFeedbacksAsync(int feedbackedId)
        {
            var result = await _feedbackService.GetAllFeedbacksAsync(feedbackedId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteFeedbackAsync(int feedbackId)
        {
            var userId = UserHelpers.GetUserId(User);
            var result = await _feedbackService.DeleteFeedbadckAsync(feedbackId, userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateFeedbackAsync([FromBody] UpdateFeedbackDto dto)
        {
            var userId = UserHelpers.GetUserId(User);
            var result = await _feedbackService.UpdateFeedbackAsync(dto, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
