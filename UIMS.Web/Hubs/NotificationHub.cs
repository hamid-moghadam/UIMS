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
    public class NotificationHub:BaseHub
    {
        private readonly SemesterService _SemesterService;
        private readonly NotificationService _messageService;
        private readonly PresentationService _presentationService;
        private readonly UserService _userService;


        public NotificationHub(SemesterService semesterService, NotificationService messageService,UserService userService, PresentationService presentationService)
        {
            _SemesterService = semesterService;
            _messageService = messageService;
            _presentationService = presentationService;
            _userService = userService;
        }

        public async Task SendMessage(string id,string title,string content)
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

            //user = await _userService.GetAsync(x => x.Id == int.Parse(Context.UserIdentifier));
            //var userReceivers = _userService.GetAll().Select(x => x.Id).Except(new List<int>() { user.Id }).Select(x => new MessageReceiver() { UserId = x });
            var userReceivers = _userService.GetAll().Select(x => x.Id).Select(x => new NotificationReceiver() { UserId = x });
            //if (user == null)
            //    return;
            await _messageService.AddAsync(new Notification()
            {
                Content = content,
                NotificationTypeId = 1,
                Title = title,
                SemesterId = currentSemester.Id,
                SenderId = user.Id,
                Receivers = userReceivers.ToList()
            });
            await _messageService.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", $"شما یک پیام از {user.FullName} دارید.");
        }

        public async Task SuspendPresentation(int presentationId)
        {
            var currentSemester = await _SemesterService.GetCurrentAsycn();
            var students = await _presentationService.GetStudentsAsync(presentationId);
            //if (int.TryParse(id, out int result))
            //{
            //    user = await _userService.GetAsync(x => x.Id == result);
            //}
            //else
            //    return;
            var user = await _userService.GetAsync(x => x.Id == UserId);
            await _messageService.AddAsync(new Notification()
            {
                Content = "",
                NotificationTypeId= 2,
                Title = "",
                SemesterId = currentSemester.Id,
                SenderId = user.Id,
                Receivers = students.Select(x => new NotificationReceiver() { UserId = x }).ToList()
            });
            await _messageService.SaveChangesAsync();
            await Clients.Users(students.Select(x=>x.ToString()).ToList().AsReadOnly()).SendAsync("ReceiveMessage", $"شما یک پیام از {user.FullName} دارید.");
        }

        public async Task SendMessagePresentation(string id,string title,string content)
        {
            var currentSemester = await _SemesterService.GetCurrentAsycn();
            var students = await _presentationService.GetStudentsAsync(int.Parse(id));
            var user = await _userService.GetAsync(x => x.Id == UserId);

            await _messageService.AddAsync(new Notification()
            {
                Content = content,
                NotificationTypeId = 3,
                Title = title,
                SemesterId = currentSemester.Id,
                SenderId = user.Id,
                Receivers = students.Select(x => new NotificationReceiver() { UserId = x }).ToList()
            });
            await _messageService.SaveChangesAsync();
            await Clients.Users(students.Select(x => x.ToString()).ToList().AsReadOnly()).SendAsync("ReceiveMessage", $"شما یک پیام از {user.FullName} دارید.");
        }


        [Authorize]
        public async Task Send(string title, string content)
        {
            var currentSemester = await _SemesterService.GetCurrentAsycn();
            var user = await _userService.GetAsync(x => x.Id == int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value));
            //var userReceivers = _userService.GetAll().Select(x => x.Id).Except(new List<int>() { user.Id }).Select(x => new MessageReceiver() { UserId = x });
            var userReceivers = _userService.GetAll().Select(x => x.Id).Select(x => new NotificationReceiver() { UserId = x });
            if (user == null)
                return;
            await _messageService.AddAsync(new Notification()
            {
                Content = content,
                NotificationTypeId = 1,
                Title = title,
                SemesterId = currentSemester.Id,
                SenderId = user.Id,
                Receivers = userReceivers.ToList()
            });
            await _messageService.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", $"شما یک پیام از {user.FullName} دارید.");
        }

        [Authorize]
        public async Task SendOne(string id, string title, string content)
        {
            var currentSemester = await _SemesterService.GetCurrentAsycn();
            var user = await _userService.GetAsync(x => x.Id == int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value));
            //var userReceivers = _userService.GetAll().Select(x => x.Id).Except(new List<int>() { user.Id }).Select(x => new MessageReceiver() { UserId = x });
            var userReceivers = new List<NotificationReceiver>() { new NotificationReceiver() { UserId = int.Parse(id) } };
            if (user == null)
                return;
            await _messageService.AddAsync(new Notification()
            {
                Content = content,
                NotificationTypeId = 1,
                Title = title,
                SemesterId = currentSemester.Id,
                SenderId = user.Id,
                Receivers = userReceivers.ToList()
            });
            await _messageService.SaveChangesAsync();
            await Clients.Users(user.Id.ToString()).SendAsync("ReceiveMessage", $"شما یک پیام از {user.FullName} دارید.");
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
