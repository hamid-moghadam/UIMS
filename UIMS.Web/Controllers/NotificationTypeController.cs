using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using UIMS.Web.DTO;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class NotificationTypeController : ApiController
    {

        private readonly NotificationTypeService _notifTypeService;
        private readonly IMapper _mapper;


        public NotificationTypeController(NotificationTypeService notifTypeService, IMapper mapper)
        {
            _mapper = mapper;
            _notifTypeService = notifTypeService;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<NotificationTypeViewModel>>> GetAll(FilterInputViewModel filterInputVM)
        {
            return Ok(await _notifTypeService.GetAllAsync(filterInputVM));
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationTypeViewModel>>> GetAllAttached()
        {
            return Ok(await _notifTypeService.GetAttachedNotificationTypesAsync(UserId));
        }

        // GET: api/values
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]NotificationTypeInsertViewModel notificationTypeInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _notifTypeService.IsExistsAsync(x=>x.Type == notificationTypeInsertVM.Type))
            {
                ModelState.AddModelError("Errors", "این نوع اطلاع رسانی قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _notifTypeService.AddAsync(notificationTypeInsertVM);
            await _notifTypeService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Update([FromBody]NotificationTypeUpdateViewModel notifTypeUpdateVM)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var notifType = await _notifTypeService.GetAsync(x => x.Id == notifTypeUpdateVM.Id);
            if (notifType == null)
                return NotFound();

            notifType = _mapper.Map(notifTypeUpdateVM, notifType);

            if (await _notifTypeService.IsExistsAsync(x => x.Type== notifType.Type && x.Id != notifType.Id))
            {
                ModelState.AddModelError("Errors", "مشخصات نوع اطلاع رسانی قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }
            _notifTypeService.Update(notifType);
            await _notifTypeService.SaveChangesAsync();
            return Ok();
        }


    }
}
