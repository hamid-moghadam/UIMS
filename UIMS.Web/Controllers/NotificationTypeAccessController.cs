using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.Services;
using UIMS.Web.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMS.Web.Controllers
{
    public class NotificationTypeAccessController : ApiController
    {
        private readonly RoleService  _roleService;
        private readonly NotificationTypeService _notificationTypeService;
        private readonly NotificationAccessService _notificationAccessService;
        private readonly IMapper _mapper;

        public NotificationTypeAccessController(NotificationAccessService notificationAccessService,IMapper mapper, RoleService roleService, NotificationTypeService notificationTypeService)
        {
            _notificationAccessService = notificationAccessService;
            _notificationTypeService = notificationTypeService;
            _roleService = roleService;
            _mapper = mapper;
        }


        // GET: api/values
        [SwaggerResponse(200, typeof(List<NotificationAccessViewModel>))]
        [HttpPost]
        public async Task<PaginationViewModel<NotificationAccessViewModel>> GetAll([FromBody]FilterInputViewModel filterInputVM)
        {
            return await _notificationAccessService.GetAllAsync(filterInputVM.Filters,filterInputVM.Page,filterInputVM.PageSize);
        }


        [SwaggerResponse(200, typeof(List<NotificationAccessViewModel>))]
        [HttpGet("{roleId}")]
        public async Task<List<NotificationAccessViewModel>> GetAll(int roleId)
        {
            return await _notificationAccessService.GetAllByRoleAsync(roleId);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]NotificationAccessInsertModel notificationAccessInsertModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _roleService.IsExistsAsync(x=>x.Id == notificationAccessInsertModel.AppRoleId.Value))
            {
                ModelState.AddModelError("Errors", "این نقش در سیستم ثبت نشده است.");
                return BadRequest(ModelState);
            }
            if (!await _notificationTypeService.IsExistsAsync(x => x.Id == notificationAccessInsertModel.NotificationTypeId.Value))
            {
                ModelState.AddModelError("Errors", "این نوع اطلاع رسانی در سیستم ثبت نشده است.");
                return BadRequest(ModelState);
            }
            if (await _notificationAccessService.IsExistsAsync(notificationAccessInsertModel))
            {
                ModelState.AddModelError("Errors","این دسترسی اطلاع رسانی قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }
            await _notificationAccessService.AddAsync(notificationAccessInsertModel);
            await _notificationAccessService.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var notifAccess = await _notificationAccessService.GetAsync(x=>x.Id == id);

            if (notifAccess == null)
            {
                ModelState.AddModelError("Errors", "این دسترسی اطلاع رسانی در سیستم ثبت نشده است.");
                return BadRequest(ModelState);
            }
            _notificationAccessService.Remove(notifAccess);
            await _notificationAccessService.SaveChangesAsync();
            return Ok();
        }
    }
}
