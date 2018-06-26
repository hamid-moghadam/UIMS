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
    [Authorize]
    public class NotificationHub:BaseHub
    {
        private readonly NotificationService _messageService;
        private readonly NotificationTypeService _notificationTypeService;
        private readonly PresentationService _presentationService;
        private static int _usersCount = 0;

        public NotificationHub(SemesterService semesterService, NotificationService messageService,UserService userService, PresentationService presentationService, NotificationTypeService notificationTypeService) :base(semesterService,userService)
        {
            _notificationTypeService = notificationTypeService;
            _messageService = messageService;
            _presentationService = presentationService;
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


        public async Task SendSpecific(string[] ids,string title,string content)
        {
            var type = await _notificationTypeService.CreateIfNotExists("اختصاصی");
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(content, title, type.Id, receivers);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();

            await Clients.Users(ids).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
        }

        public async Task SendAll(string title, string content)
        {
            var type = await _notificationTypeService.CreateIfNotExists("عمومی");
            var ids = _userService.GetAll().Select(x => x.Id);
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(content, title, type.Id, receivers);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();

            await Clients.Users(ids.Select(x=>x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
        }

        public async Task SendRole(string role,string title, string content)
        {
            var type = await _notificationTypeService.CreateIfNotExists("گروهی");
            var ids = (await _userService.GetAll(role)).Select(x => x.Id);
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(content, title, type.Id, receivers);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();

            await Clients.Users(ids.Select(x => x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
        }



        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _usersCount++;
            foreach (var role in Roles)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, role);
            }
            await Clients.Groups("admin", "supervisor").SendAsync("ReceiveOnlineUsers", _usersCount);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            _usersCount--;
            foreach (var role in Roles)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, role);
            }
            await Clients.Groups("admin", "supervisor").SendAsync("ReceiveOnlineUsers", _usersCount);
        }




    }
}
