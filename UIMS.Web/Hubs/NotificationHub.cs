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
            try
            {
                var presentation = await _presentationService.GetAsync(presentationId);
                var type = _notificationTypeService.CreateIfNotExists("عدم تشکیل کلاس");
                var students = await _presentationService.GetStudentsAsync(presentationId);
                var receivers = GetReceiversByIds(students);
                var t = await GetNotificationAsync($"کلاس {presentation.CourseField.Course.Name} استاد {presentation.Professor.UserFullName} این هفته برگزار نمی شود", "عدم تشکیل کلاس", type.Id, receivers);

                await _messageService.AddAsync(t.Item1);
                await _messageService.SaveChangesAsync();
                receivers.ForEach(x => x.NotificationId = t.Item1.Id);
                await Clients.Users(students.Select(x=>x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
            }
            catch(Exception e)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", e.Message);
            }
        }

        public async Task SendMessagePresentation(string id,string title,string content,string subtitle="")
        {
            var presentation = await _presentationService.GetAsync(int.Parse(id));
            var type = _notificationTypeService.CreateIfNotExists("ارسال پیام به دانشجویان کلاس");
            var students = await _presentationService.GetStudentsAsync(presentation.Id);
            var receivers = GetReceiversByIds(students);
            var t = await GetNotificationAsync(content, title, type.Id, receivers,subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            await Clients.Users(students.Select(x => x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
        }


        public async Task SendSpecific(string[] ids,string title,string content, string subtitle = "")
        {
            var type =  _notificationTypeService.CreateIfNotExists("اختصاصی");
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(content, title, type.Id, receivers, subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            await Clients.Users(ids).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
        }

        public async Task SendAll(string title, string content, string subtitle = "")
        {
            var type =  _notificationTypeService.CreateIfNotExists("عمومی");
            var ids = _userService.GetAll().Select(x => x.Id);
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(content, title, type.Id, receivers, subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            await Clients.Users(ids.Select(x=>x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
        }

        public async Task SendRole(string role,string title, string content, string subtitle = "")
        {
            var type =  _notificationTypeService.CreateIfNotExists("گروهی (نقش ها)");
            var ids = (await _userService.GetAll(role)).Select(x => x.Id);
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(content, title, type.Id, receivers, subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            await Clients.Users(ids.Select(x => x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
        }

        public async Task Send(string[] ids, int notifTypeId, string title, string content, string subtitle = "")
        {
            var notifType = _notificationTypeService.Get(x => x.Id == notifTypeId);
            if (notifType == null)
                return;

            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(content, title, notifTypeId, receivers, subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();

            receivers.ForEach(x => x.NotificationId = t.Item1.Id);
            await Clients.Users(ids).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
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
