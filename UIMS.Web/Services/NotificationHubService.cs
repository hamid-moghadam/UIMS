using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.DTO;
using UIMS.Web.Models;

namespace UIMS.Web.Services
{
    public class NotificationHubService:BaseHubService
    {
        private readonly NotificationService _messageService;
        private readonly SettingsService _settingsService;
        private readonly NotificationTypeService _notificationTypeService;
        private readonly PresentationService _presentationService;

        public NotificationHubService(SemesterService semesterService, NotificationService messageService, UserService userService, PresentationService presentationService, NotificationTypeService notificationTypeService, SettingsService settingsService) :base(semesterService,userService)
        {
            _notificationTypeService = notificationTypeService;
            _messageService = messageService;
            _presentationService = presentationService;
            _settingsService = settingsService;
        }




        public async Task<Tuple<List<NotificationReceiver>,AppUser>> SuspendPresentation(int presentationId)
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
                return new Tuple<List<NotificationReceiver>, AppUser>(receivers,t.Item2);
            }
            catch (Exception e)
            {
                return null;
                //await Clients.Caller.SendAsync("ReceiveMessage", e.Message);
            }

        }

        public async Task<Tuple<List<NotificationReceiver>,AppUser>> SendMessagePresentation(SendMessagePresentationInsertViewModel sendMessageInsertVM)
        {
            var presentation = await _presentationService.GetAsync(sendMessageInsertVM.Id);
            //var type = _notificationTypeService.CreateIfNotExists(await _settingsService.GetValueAsync("SuspendPresentationNotificationTypeName"));
            var type = _notificationTypeService.CreateIfNotExists("پیام های اساتید");
            var students = await _presentationService.GetStudentsAsync(presentation.Id);
            var receivers = GetReceiversByIds(students);
            var t = await GetNotificationAsync(sendMessageInsertVM.Content, sendMessageInsertVM.Title, type.Id, receivers, sendMessageInsertVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);
            return new Tuple<List<NotificationReceiver>, AppUser>(receivers, t.Item2);
        }


        public async Task<Tuple<List<NotificationReceiver>,AppUser>> SendSpecific(SendSpecificViewModel sendSpecificVM)
        {
            //var type =  _notificationTypeService.CreateIfNotExists("اختصاصی");
            var type = _notificationTypeService.Get(x => x.Id == sendSpecificVM.NotificationTypeId);
            if (type == null)
                type = _notificationTypeService.CreateIfNotExists("اختصاصی");

            var receivers = GetReceiversByIds(sendSpecificVM.Ids);
            var t = await GetNotificationAsync(sendSpecificVM.Content, sendSpecificVM.Title, type.Id, receivers, sendSpecificVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            return new Tuple<List<NotificationReceiver>, AppUser>(receivers, t.Item2);
        }

        public async Task<Tuple<List<NotificationReceiver>,AppUser>> SendAll(SendMessageViewModel sendMessageVM)
        {
            //var type =  _notificationTypeService.CreateIfNotExists("عمومی");
            var type = _notificationTypeService.Get(x => x.Id == sendMessageVM.NotificationTypeId);
            if (type == null)
                type = _notificationTypeService.CreateIfNotExists("عمومی");

            var ids = _userService.GetAll().Where(x => x.Id != UserId).Select(x => x.Id);
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(sendMessageVM.Content, sendMessageVM.Title, type.Id, receivers, sendMessageVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            return new Tuple<List<NotificationReceiver>, AppUser>(receivers, t.Item2);

        }

        public async Task<Tuple<List<NotificationReceiver>,AppUser>> SendRole(SendRoleViewModel sendRoleVM)
        {
            var type = _notificationTypeService.Get(x => x.Id == sendRoleVM.NotificationTypeId);
            if (type == null)
                type = _notificationTypeService.CreateIfNotExists("گروهی (نقش ها)");

            var ids = (await _userService.GetAll(sendRoleVM.Role)).Select(x => x.Id);
            var receivers = GetReceiversByIds(ids);
            var t = await GetNotificationAsync(sendRoleVM.Content, sendRoleVM.Title, type.Id, receivers, sendRoleVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();
            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            return new Tuple<List<NotificationReceiver>, AppUser>(receivers,t.Item2);

        }

        public async Task<Tuple<List<NotificationReceiver>,AppUser>> SendFull(SendFullViewModel sendFullVM)
        {
            var notifType = _notificationTypeService.Get(x => x.Id == sendFullVM.NotificationTypeId);
            if (notifType == null)
                return null;

            var receivers = GetReceiversByIds(sendFullVM.Ids);
            var t = await GetNotificationAsync(sendFullVM.Content, sendFullVM.Title, sendFullVM.NotificationTypeId, receivers, sendFullVM.Subtitle);

            await _messageService.AddAsync(t.Item1);
            await _messageService.SaveChangesAsync();

            receivers.ForEach(x => x.NotificationId = t.Item1.Id);

            return new Tuple<List<NotificationReceiver>, AppUser>(receivers,t.Item2);
        }



    }
}
