using CORE.DTOs.Message;
using CORE.DTOs.UserMatch;
using CORE.Services.IServices;
using DATA.DataAccess.Repositories.UnitOfWork;
using Microsoft.AspNetCore.SignalR;

namespace CORE.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IMatchingService _matchingService;
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IMessageService messageService, IMatchingService matchingService,IUnitOfWork unitOfWork)
        {
            _messageService = messageService;
            _matchingService = matchingService;
            _unitOfWork = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            var userId =  int.Parse(Context.UserIdentifier!);
            var user = await _unitOfWork.AppUsers.GetAsync(userId);
            if(user != null)
            {
                user.IsOnline = true;
                await _unitOfWork.AppUsers.AddOrUpdateAsync(user);
                await _unitOfWork.CommitAsync();
            }
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = int.Parse(Context.UserIdentifier!);
            var user = await _unitOfWork.AppUsers.GetAsync(userId);
            if (user != null)
            {
                user.IsOnline = false;
                await _unitOfWork.AppUsers.AddOrUpdateAsync(user);
                await _unitOfWork.CommitAsync();
            }
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessageAsync(int receiverId, string content)
        {
            int senderId = int.Parse(Context.UserIdentifier!);

            var request = new MessageAddRequest
            {
                SenderID = senderId,
                ReceiverID = receiverId,
                Message = content
            };

            var savedMessage = await _messageService.CreateAsync(request);
            await Clients.Users(senderId.ToString(),receiverId.ToString()).SendAsync("ReceiveMessage", savedMessage);

            //await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", savedMessage);

            //await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", savedMessage);
        }
        public async Task RequestMatchAsync(string matchType)
        {
            int userId = int.Parse(Context.UserIdentifier!);
            var request = new UserMatchRequest { UserId = userId, MatchType = matchType };
            var match = await _matchingService.FindMatchAsync(request);
            await Clients.User(userId.ToString()).SendAsync("MatchFound", match);

            // Notify the matched user
            var otherUserId = match.UserId1 == userId ? match.UserId2 : match.UserId1;
            await Clients.User(otherUserId.ToString()).SendAsync("MatchFound", match);
        }

        public async Task SendWebRtcSignal(string targetUserId, string signalData)
        {
            await Clients.User(targetUserId).SendAsync("ReceiveWebRtcSignal", Context.UserIdentifier, signalData);
        }

    }
}
