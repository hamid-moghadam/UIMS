using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;
using UIMS.Web.Services;
using UIMS.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class NotificationController : ApiController
    {
        private readonly NotificationService _messageService;

        public NotificationController(NotificationService messageService)
        {
            _messageService = messageService;
        }

        // GET: api/values
        [SwaggerResponse(200, typeof(PaginationViewModel<NotificationViewModel>))]
        [HttpPost]
        public async Task<IActionResult> GetAll(NotificationGetAllViewModel messageGetAllVM)
        {
            return Ok(await _messageService.GetAll(messageGetAllVM.Semester,messageGetAllVM.Page,messageGetAllVM.PageSize,UserId));
        }

        // GET api/values/5
        [HttpGet("{semester}")]
        public async Task<IActionResult> GetBadge(string semester)
        {
            return Ok(await _messageService.GetMessagesCount(semester, UserId));
        }


    }
}
