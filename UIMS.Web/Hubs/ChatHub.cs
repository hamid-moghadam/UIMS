using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Services;

namespace UIMS.Web.Hubs
{
    [Authorize]
    public class ChatHub:BaseHub
    {
        private readonly ChatService _chatService;
        private readonly SemesterService _semesterService;
        private readonly UserService _userService;

        public ChatHub(ChatService conversationService, SemesterService semesterService, UserService userService)
        {
            _chatService = conversationService;
            _userService = userService;
            _semesterService = semesterService;
        }

        public async Task SendMessage(string id, string message)
        {
            var currentSemester = await _semesterService.GetCurrentAsycn();
            var conversation = await _chatService.AddIfNotExists(UserId,int.Parse(id));
            var user = await _userService.GetAsync(x => x.Id == UserId);
            conversation.Replies.Add(new Models.ChatReply()
            {
                ChatId = conversation.Id,
                ReplierId = UserId,
                Reply = message,
                SemesterId = currentSemester.Id
            });

            await Clients.User(id).SendAsync("ReceiveMessage", $"شما یک پیام از {user.FullName} دارید.");

            await Clients.User(id).SendAsync("ReceiveConversation", message);
        }
    }
}
