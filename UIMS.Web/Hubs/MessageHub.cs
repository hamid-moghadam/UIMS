using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UIMS.Web.Models;
using UIMS.Web.Services;

namespace UIMS.Web.Hubs
{
    //[Authorize]
    public class MessageHub:Hub
    {
        private readonly SemesterService _SemesterService;
        private readonly MessageService _messageService;
        private readonly UserService _userService;
        public MessageHub(SemesterService semesterService, MessageService messageService,UserService userService)
        {
            _SemesterService = semesterService;
            _messageService = messageService;
            _userService = userService;
        }
        
        public async Task SendMessage(string id ,string title,string content)
        {
            var currentSemester = await _SemesterService.GetCurrentAsycn();
            AppUser user = null;
            //if (int.TryParse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value,out int result))
            if (int.TryParse(id, out int result))
            {
                user = await _userService.GetAsync(x => x.Id == result);
            }
            else
                return;

            //user = await _userService.GetAsync(x => x.Id == );
            //var userReceivers = _userService.GetAll().Select(x => x.Id).Except(new List<int>() { user.Id }).Select(x => new MessageReceiver() { UserId = x });
            var userReceivers = _userService.GetAll().Select(x => x.Id).Select(x => new MessageReceiver() { UserId = x });
            //if (user == null)
            //    return;
            await _messageService.AddAsync(new Message()
            {
                Content = content,
                MessageTypeId = 1,
                Title = title,
                SemesterId = currentSemester.Id,
                SenderId = user.Id,
                Receivers = userReceivers.ToList()
            });
            await _messageService.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", $"شما یک پیام از {user.FullName} دارید.");
        }

        public async Task Send(string title, string content)
        {
            var currentSemester = await _SemesterService.GetCurrentAsycn();
            var user = await _userService.GetAsync(x => x.Id == int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value));
            //var userReceivers = _userService.GetAll().Select(x => x.Id).Except(new List<int>() { user.Id }).Select(x => new MessageReceiver() { UserId = x });
            var userReceivers = _userService.GetAll().Select(x => x.Id).Select(x => new MessageReceiver() { UserId = x });
            if (user == null)
                return;
            await _messageService.AddAsync(new DTO.MessageInsertViewModel()
            {
                Content = content,
                MessageTypeId = 1,
                Title = title,
                SemesterId = currentSemester.Id,
                SenderId = user.Id,
                Receivers = userReceivers.ToList()
            });
            await _messageService.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", $"شما یک پیام از {user.FullName} دارید.");
        }

        public override async Task OnConnectedAsync()
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

    }
}
