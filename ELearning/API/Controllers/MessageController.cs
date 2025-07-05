using CORE.DTOs.Message;
using CORE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Handles operations related to messages, such as sending, retrieving, deleting, and marking messages as read.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Gets the current authenticated user's ID from the JWT claims.
        /// </summary>
        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Retrieves all messages for the currently authenticated user.
        /// </summary>
        /// <returns>A list of all messages sent or received by the user.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int userId = GetCurrentUserId();
            var messages = await _messageService.GetAllMessageAsync(userId);
            return Ok(messages);
        }
        /// <summary>
        /// Retrieves a list of recent chat summaries for the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a list of users the current user has chatted with, including:
        /// - The other user's ID and name
        /// - Profile image
        /// - The last message exchanged
        /// - Timestamp of the last message
        /// - Count of unread messages from each user
        /// 
        /// The chat summaries are sorted by the most recent message timestamp.
        /// </remarks>
        /// <returns>
        /// A list of <see cref="ChatSummaryResponse"/> objects representing the recent chats.
        /// </returns>
        /// <response code="200">Returns the list of recent chats</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpGet("list")]
        public async Task<IActionResult> GetChatList()
        {
            int userId = GetCurrentUserId();
            var chats = await _messageService.GetChatListAsync(userId);
            return Ok(chats);
        }


        /// <summary>
        /// Retrieves the chat messages exchanged between the current user and another specific user.
        /// </summary>
        /// <param name="withUser">The ID of the other user to fetch chat with.</param>
        /// <returns>A list of messages exchanged with the specified user.</returns>
        [HttpGet("chat")]
        public async Task<IActionResult> GetChatWith([FromQuery] int withUser)
        {
            int userId = GetCurrentUserId();
            var chat = await _messageService.GetAllChatAsync(userId, withUser);
            return Ok(chat);
        }

        /// <summary>
        /// Retrieves a specific message by its ID.
        /// </summary>
        /// <param name="id">The ID of the message to retrieve.</param>
        /// <returns>The message if found.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var message = await _messageService.GetByAsync(id);
            return Ok(message);
        }

        /// <summary>
        /// Gets the count of unread messages for the current user.
        /// </summary>
        /// <returns>The number of unread messages.</returns>
        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            int userId = GetCurrentUserId();
            int count = await _messageService.GetUnreadCountAsync(userId);
            return Ok(count);
        }

        /// <summary>
        /// Retrieves the last message exchanged between the current user and another specific user.
        /// </summary>
        /// <param name="withUser">The ID of the other user.</param>
        /// <returns>The most recent message exchanged.</returns>
        [HttpGet("last")]
        public async Task<IActionResult> GetLastMessageWith([FromQuery] int withUser)
        {
            int userId = GetCurrentUserId();
            var message = await _messageService.GetLastMessageWithUserAsync(userId, withUser);
            return Ok(message);
        }

        /// <summary>
        /// Marks a message as read.
        /// </summary>
        /// <param name="id">The ID of the message to mark as read.</param>
        /// <returns>No content if successful, or bad request if failed.</returns>
        [HttpPatch("read/{id:int}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var updated = await _messageService.MarkMessageAsReadAsync(id);
            return updated ? NoContent() : BadRequest("Failed to update.");
        }

        /// <summary>
        /// Deletes a message for the current user (soft delete).
        /// </summary>
        /// <param name="id">The ID of the message to delete.</param>
        /// <returns>No content if successful, or bad request if failed.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = GetCurrentUserId();
            var deleted = await _messageService.DeleteMessageAsync(id, userId);
            return deleted ? NoContent() : BadRequest("Failed to delete.");
        }
    }
}
