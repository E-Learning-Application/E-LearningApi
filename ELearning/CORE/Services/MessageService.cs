using AutoMapper;
using CORE.DTOs.Message;
using CORE.Exceptions;
using CORE.Services.IServices;
using DATA.DataAccess.Repositories.UnitOfWork;
using DATA.Models;
using Microsoft.Extensions.Logging;

namespace CORE.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MessageService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MessageResponse> CreateAsync(MessageAddRequest? request)
        {
            if (request == null) throw new ArgumentNullException("Request can't be null");

            var senderUser = await _unitOfWork.AppUsers.FindAsync(x => x.Id == request.SenderID)
                             ?? throw new NotFoundException("Sender not found");

            var receiverUser = await _unitOfWork.AppUsers.FindAsync(x => x.Id == request.ReceiverID)
                               ?? throw new NotFoundException("Receiver not found");

            var message = _mapper.Map<Message>(request);
            await _unitOfWork.Messages.AddOrUpdateAsync(message);
            await _unitOfWork.CommitAsync();

            message.Sender = senderUser;
            message.Receiver = receiverUser;

            return _mapper.Map<MessageResponse>(message);
        }

        public async Task<bool> DeleteMessageAsync(int messageID, int currentUserId)
        {
            var message = await _unitOfWork.Messages.FindAsync(x => x.Id == messageID)
                           ?? throw new NotFoundException("Message not found");

            if (message.SenderID == currentUserId)
            {
                message.IsDeletedBySender = true;
            }
            else if (message.ReceiverID == currentUserId)
            {
                message.IsDeletedByReceiver = true;
            }
            else
            {
                throw new UnauthorizedAccessException("You can't delete this message.");
            }

            await _unitOfWork.Messages.AddOrUpdateAsync(message);
            var changes = await _unitOfWork.CommitAsync();
            return changes > 0;
        }

        public async Task<IEnumerable<MessageResponse>> GetAllChatAsync(int userID, int receiverID)
        {
            var messages = await _unitOfWork.Messages.GetAllAsync(
                x => (x.SenderID == userID && x.ReceiverID == receiverID && !x.IsDeletedBySender) ||
                     (x.SenderID == receiverID && x.ReceiverID == userID && !x.IsDeletedByReceiver),
                includes: new[] { "Sender", "Receiver" });

            return _mapper.Map<IEnumerable<MessageResponse>>(messages);
        }

        public async Task<IEnumerable<MessageResponse>> GetAllMessageAsync(int userID)
        {
            var messages = await _unitOfWork.Messages.GetAllAsync(
                x => (x.SenderID == userID && !x.IsDeletedBySender) ||
                     (x.ReceiverID == userID && !x.IsDeletedByReceiver),
                includes: new[] { "Sender", "Receiver" });

            return _mapper.Map<IEnumerable<MessageResponse>>(messages);
        }

        public async Task<MessageResponse> GetByAsync(int messageID)
        {
            var message = await _unitOfWork.Messages.FindAsync(
                x => x.Id == messageID,
                includes: new[] { "Sender", "Receiver" })
                ?? throw new NotFoundException("Message not found");

            return _mapper.Map<MessageResponse>(message);
        }

        public async Task<bool> MarkMessageAsReadAsync(int messageID)
        {
            var message = await _unitOfWork.Messages.FindAsync(x => x.Id == messageID)
                           ?? throw new NotFoundException("Message not found");

            message.IsRead = true;
            await _unitOfWork.Messages.AddOrUpdateAsync(message);
            var changes = await _unitOfWork.CommitAsync();
            return changes > 0;
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _unitOfWork.Messages.CountAsync(
                x => x.ReceiverID == userId && !x.IsRead && !x.IsDeletedBySender);
        }

        public async Task<MessageResponse?> GetLastMessageWithUserAsync(int userId, int otherUserId)
        {
            var messages = await _unitOfWork.Messages.GetAllAsync(
                x => (x.SenderID == userId && x.ReceiverID == otherUserId && !x.IsDeletedBySender) ||
                     (x.SenderID == otherUserId && x.ReceiverID == userId && !x.IsDeletedByReceiver),
                includes: new[] { "Sender", "Receiver" });

            var lastMessage = messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
            return lastMessage != null ? _mapper.Map<MessageResponse>(lastMessage) : null;
        }

        public async Task<IEnumerable<ChatSummaryResponse>> GetChatListAsync(int userId)
        {
            var messages = await _unitOfWork.Messages.GetAllAsync(
                   x => (x.SenderID == userId || x.ReceiverID == userId),
                   includes: new[] { "Sender", "Receiver" }
               );

            var grouped = messages
                .Where(m => !((m.SenderID == userId && m.IsDeletedBySender) || (m.ReceiverID == userId && m.IsDeletedByReceiver)))
                .Select(m => new
                {
                    OtherUser = m.SenderID == userId ? m.Receiver : m.Sender,
                    Message = m
                })
                .GroupBy(x => x.OtherUser.Id)
                .Select(g =>
                {
                    var last = g.OrderByDescending(m => m.Message.SentAt).First();
                    var unreadCount = g.Count(m =>
                        m.Message.ReceiverID == userId &&
                        !m.Message.IsRead &&
                        !m.Message.IsDeletedByReceiver);

                    return new ChatSummaryResponse
                    {
                        UserId = g.Key,
                        UserName = last.OtherUser.UserName,
                        ProfileImage = last.OtherUser.ImagePath,
                        LastMessage = last.Message.Content,
                        LastMessageTime = last.Message.SentAt,
                        UnreadCount = unreadCount
                    };
                });

            return grouped.OrderByDescending(x => x.LastMessageTime).ToList();
        }
    }
}
