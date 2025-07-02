using CORE.DTOs.Message;
using CORE.Services.IServices;
using Microsoft.AspNetCore.SignalR;

namespace CORE.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
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

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
