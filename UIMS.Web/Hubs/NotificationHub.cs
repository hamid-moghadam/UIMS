using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using UIMS.Web.Services;

namespace UIMS.Web.Hubs
{
    [Authorize]
    public class NotificationHub:BaseHub
    {
        private readonly NotificationService _messageService;
        private readonly SettingsService _settingsService;
        private readonly NotificationTypeService _notificationTypeService;
        private readonly PresentationService _presentationService;
        private static int _usersCount = 0;

        public NotificationHub(SemesterService semesterService, NotificationService messageService,UserService userService, PresentationService presentationService, NotificationTypeService notificationTypeService, SettingsService settingsService) :base(semesterService,userService)
        {
            _notificationTypeService = notificationTypeService;
            _messageService = messageService;
            _presentationService = presentationService;
            _settingsService = settingsService;
        }

        public async Task SuspendPresentation(int presentationId)
        {
            try
            {
                var presentation = await _presentationService.GetAsync(presentationId);
                var type = _notificationTypeService.CreateIfNotExists("لغو کلاس ها");
                //var type = _notificationTypeService.CreateIfNotExists(await _settingsService.GetValueAsync("SuspendPresentationNotificationTypeName"));
                var students = await _presentationService.GetStudentsAsync(presentationId);
                var receivers = GetReceiversByIds(students);
                var t = await GetNotificationAsync($"کلاس {presentation.CourseField.Course.Name} استاد {presentation.Professor.UserFullName} این هفته برگزار نمی شود", "عدم تشکیل کلاس", type.Id, receivers);

                await _messageService.AddAsync(t.Item1);
                await _messageService.SaveChangesAsync();
                receivers.ForEach(x => x.NotificationId = t.Item1.Id);
                await Clients.Users(students.Select(x=>x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
                await Clients.Caller.SendAsync("ReceiveMessage", "لغو کلاس با موفقیت انجام شد");
            }
            catch(Exception e)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", e.Message);
            }
        }

        public async Task SendMessagePresentation(SendMessagePresentationInsertViewModel sendMessageInsertVM)
        {
            if (!sendMessageInsertVM.IsValid())
                return;

            var presentation = await _presentationService.GetAsync(sendMessageInsertVM.Id);
            //var type = _notificationTypeService.CreateIfNotExists(await _settingsService.GetValueAsync("SuspendPresentationNotificationTypeName"));
            var type = _notificationTypeService.CreateIfNotExists("پیام های اساتید");
            var students = await _presentationService.GetStudentsAsync(presentation.Id);
            var receivers = GetReceiversByIds(students);
            var t = await GetNotificationAsync(sendMessageInsertVM.Content, sendMessageInsertVM.Title, type.Id, receivers, sendMessageInsertVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            await Clients.Users(students.Select(x => x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
            await Clients.Caller.SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
        }


        public async Task SendSpecific(SendSpecificViewModel sendSpecificVM)
        {
            if (!sendSpecificVM.IsValid())
                return;

            //var type =  _notificationTypeService.CreateIfNotExists("اختصاصی");
            var type =  _notificationTypeService.Get(x=>x.Id == sendSpecificVM.NotificationTypeId);
            if (type == null)
                return;

            var receivers = GetReceiversByIds(sendSpecificVM.Ids);
            var t = await GetNotificationAsync(sendSpecificVM.Content, sendSpecificVM.Title, type.Id, receivers, sendSpecificVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            await Clients.Users(sendSpecificVM.Ids).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
            await Clients.Caller.SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
        }

        public async Task SendAll(SendMessageViewModel sendMessageVM)
        {
            if (!sendMessageVM.IsValid())
                return;

            //var type =  _notificationTypeService.CreateIfNotExists("عمومی");
            var type = _notificationTypeService.Get(x => x.Id == sendMessageVM.NotificationTypeId);
            if (type == null)
                type = _notificationTypeService.CreateIfNotExists("عمومی");

            var ids = _userService.GetAll().Where(x => x.Id != UserId).Select(x=>x.Id);
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(sendMessageVM.Content, sendMessageVM.Title, type.Id, receivers, sendMessageVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            await Clients.Users(ids.Select(x=>x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
            await Clients.Caller.SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
        }

        public async Task SendRole(SendRoleViewModel sendRoleVM)
        {
            if (!sendRoleVM.IsValid())
                return;

            
            var type = _notificationTypeService.Get(x => x.Id == sendRoleVM.NotificationTypeId);
            if (type == null)
                type = _notificationTypeService.CreateIfNotExists("گروهی (نقش ها)");

            var ids = (await _userService.GetAll(sendRoleVM.Role)).Select(x => x.Id);
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(sendRoleVM.Content, sendRoleVM.Title, type.Id, receivers, sendRoleVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            await Clients.Users(ids.Select(x => x.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
            await Clients.Caller.SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
        }

        public async Task SendFull (SendFullViewModel sendFullVM)
        {
            if (!sendFullVM.IsValid())
                return;

            var notifType = _notificationTypeService.Get(x => x.Id == sendFullVM.NotificationTypeId);
            if (notifType == null)
                return;

            var receivers = GetReceiversByIds(sendFullVM.Ids);
            var t = await GetNotificationAsync(sendFullVM.Content, sendFullVM.Title, sendFullVM.NotificationTypeId, receivers, sendFullVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();

            receivers.ForEach(x => x.NotificationId = t.Item1.Id);
            await Clients.Users(sendFullVM.Ids).SendAsync("ReceiveMessage", $"شما یک پیام از {t.Item2.FullName} دارید.");
            await Clients.Caller.SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
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
            await Clients.Groups("admin").SendAsync("ReceiveMessage", exception.Message);
            await Clients.Groups("admin").SendAsync("ReceiveMessage", exception.HelpLink);
        }
    }
}
