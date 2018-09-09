using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;
using UIMS.Web.Services;
using UIMS.Web.Models;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class NotificationController : ApiController
    {
        private readonly NotificationService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(NotificationService messageService, IMapper mapper)
        {
            _notificationService = messageService;
            _mapper = mapper;
        }

        // GET: api/values
        [SwaggerResponse(200, typeof(PaginationViewModel<NotificationViewModel>))]
        [HttpPost]
        public async Task<IActionResult> GetAll(NotificationGetAllViewModel messageGetAllVM)
        {
            return Ok(await _notificationService.GetAll(messageGetAllVM.NotificationTypeId, messageGetAllVM.Semester, messageGetAllVM.Page, messageGetAllVM.PageSize, UserId,messageGetAllVM.NotificationTypeName));
        }

        [SwaggerResponse(200, typeof(PaginationViewModel<NotificationViewModel>))]
        [HttpPost]
        public async Task<IActionResult> GetAllSent(NotificationGetAllViewModel messageGetAllVM)
        {
            return Ok(await _notificationService.GetSentNotifications(messageGetAllVM.NotificationTypeId, messageGetAllVM.Semester, messageGetAllVM.Page, messageGetAllVM.PageSize, UserId));
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] NotificationUpdateViewModel notificationUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var notification = await _notificationService.GetAsync(x => x.Id == notificationUpdateVM.Id);
            if (notification == null)
                return NotFound();

            notification = _mapper.Map(notificationUpdateVM, notification);
            
            _notificationService.Update(notification);
            await _notificationService.SaveChangesAsync();
            return Ok();
        }

        [SwaggerResponse(200, typeof(PaginationViewModel<NotificationViewModel>))]
        [HttpGet("{notifId}")]
        public async Task<IActionResult> GetReceivers(int notifId)
        {
            return Ok(await _notificationService.GetReceivers(notifId));
        }
            

        [HttpPost("{notifId}")]
        public async Task<IActionResult> MarkSeenMessage(int notifId)
        {
            await _notificationService.MarkNotifAsSeenAsync(notifId,UserId);
            await _notificationService.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{semester}")]
        public async Task<IActionResult> GetBadge(string semester)
        {
            return Ok(await _notificationService.GetNotificationsCount(semester, UserId));
        }
    }
}
