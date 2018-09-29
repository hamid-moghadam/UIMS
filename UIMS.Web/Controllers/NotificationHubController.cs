using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.DTO;
using UIMS.Web.Hubs;
using UIMS.Web.Services;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    [Authorize]
    public class NotificationHubController:ApiController
    {
        private const string SUCCESSFUL_MESSAGE = "اطلاع رسانی با موفقیت انجام شد";
        private readonly IHubContext<NotificationHub> _hobContext;
        private readonly NotificationHubService _notificationHubService;



        public NotificationHubController(IHubContext<NotificationHub> hobContext, NotificationHubService notificationHubService)
        {
            _hobContext = hobContext;
            _notificationHubService = notificationHubService;
        }

        [HttpPost("{presentationId}")]
        public async Task<IActionResult> SuspendPresentation(int presentationId)
        {

            _notificationHubService.InitializeToken(UserId, Roles);

            var notifTuple = await _notificationHubService.SuspendPresentation(presentationId);
            await _hobContext.Clients.Users(notifTuple.Item1.Select(x=>x.UserId.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {notifTuple.Item2.FullName} دارید.");
            await _hobContext.Clients.User(UserId.ToString()).SendAsync("ReceiveMessage", "لغو کلاس با موفقیت انجام شد");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessagePresentation(SendMessagePresentationInsertViewModel sendMessageInsertVM)
        {
            if (!sendMessageInsertVM.IsValid())
                return BadRequest();

            _notificationHubService.InitializeToken(UserId, Roles);

            var notifTuple = await _notificationHubService.SendMessagePresentation(sendMessageInsertVM);

            await _hobContext.Clients.Users(notifTuple.Item1.Select(x => x.UserId.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {notifTuple.Item2.FullName} دارید.");
            await _hobContext.Clients.User(UserId.ToString()).SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> SendSpecific(SendSpecificViewModel sendSpecificVM)
        {
            if (!sendSpecificVM.IsValid())
                return BadRequest();

            _notificationHubService.InitializeToken(UserId, Roles);

            var notifTuple = await _notificationHubService.SendSpecific(sendSpecificVM);
            await _hobContext.Clients.Users(notifTuple.Item1.Select(x => x.UserId.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {notifTuple.Item2.FullName} دارید.");
            await _hobContext.Clients.User(UserId.ToString()).SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendAll(SendMessageViewModel sendMessageVM)
        {
            if (!sendMessageVM.IsValid())
                return BadRequest();

            _notificationHubService.InitializeToken(UserId, Roles);

            var notifTuple = await _notificationHubService.SendAll(sendMessageVM);

            await _hobContext.Clients.Users(notifTuple.Item1.Select(x=>x.UserId.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {notifTuple.Item2.FullName} دارید.");
            await _hobContext.Clients.User(UserId.ToString()).SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendRole(SendRoleViewModel sendRoleVM)
        {
            if (!sendRoleVM.IsValid())
                return BadRequest();

            _notificationHubService.InitializeToken(UserId, Roles);

            var notifTuple = await _notificationHubService.SendAll(sendRoleVM);
            await _hobContext.Clients.Users(notifTuple.Item1.Select(x => x.UserId.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {notifTuple.Item2.FullName} دارید.");
            await _hobContext.Clients.User(UserId.ToString()).SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendFull (SendFullViewModel sendFullVM)
        {
            if (!sendFullVM.IsValid())
                return BadRequest();

            _notificationHubService.InitializeToken(UserId, Roles);

            var notifTuple = await _notificationHubService.SendFull(sendFullVM);
            await _hobContext.Clients.Users(notifTuple.Item1.Select(x => x.UserId.ToString()).ToArray()).SendAsync("ReceiveMessage", $"شما یک پیام از {notifTuple.Item2.FullName} دارید.");
            await _hobContext.Clients.User(UserId.ToString()).SendAsync("ReceiveMessage", SUCCESSFUL_MESSAGE);
            return Ok();
        }




    }
}
