using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using UIMS.Web.Services;
using UIMS.Web.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;
using UIMS.Web.Models;
using UIMS.Web.Extentions;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Authorization;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/GroupManager")]
    public class GroupManagerController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly GroupManagerService _groupManagerService;
        private readonly CourseFieldService _courseFieldService;
        private readonly PresentationService _presentationService;
        private readonly FieldService _fieldService;
        private readonly UserService _userService;

        public GroupManagerController(GroupManagerService groupManagerService, IMapper mapper, UserService userService, FieldService fieldService, CourseFieldService courseFieldService, PresentationService presentationService)
        {
            _groupManagerService = groupManagerService;
            _fieldService = fieldService;
            _userService = userService;
            _courseFieldService = courseFieldService;
            _presentationService = presentationService;
            _mapper = mapper;
        }


        [HttpPost]
        [ProducesResponseType(typeof(PaginationViewModel<GroupManagerViewModel>), 200)]
        public async Task<IActionResult> GetAll([FromBody]FilterInputViewModel filterInputVM)
        {
            var managers = await _groupManagerService.GetAllAsync(filterInputVM);

            return Ok(managers);
        }

        [SwaggerResponse(200, typeof(GroupManagerViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var manager = await _groupManagerService.GetAsync(id);
            if (manager == null)
                return NotFound();
            return Ok(manager);
        }

        [HttpGet]
        [Authorize(Roles ="groupManager")]
        [SwaggerResponse(200, typeof(PaginationViewModel<PresentationViewModel>))]
        public async Task<IActionResult> GetCourseFields(int pageSize=5,int page =1 )
        {
            var manager = await _groupManagerService.GetAsync(x=>x.UserId == UserId);

            var courseFields = await _courseFieldService.GetAllByGroupManagerId(manager.Id, page,pageSize);

            return Ok(courseFields);
        }


        [HttpGet]
        [Authorize(Roles = "groupManager")]
        [SwaggerResponse(200, typeof(PaginationViewModel<PresentationViewModel>))]
        public async Task<IActionResult> GetFieldPresentations(int pageSize = 5, int page = 1)
        {
            var fieldIds = await _groupManagerService.GetFieldIdsAsync(UserId);
            var semester = await _groupManagerService.GetCurrentSemesterAsync();

            var presentations = await _presentationService.GetFieldPresentations(semester, fieldIds, page, pageSize);
            return Ok(presentations);
        }

        
        [HttpPost]
        [SwaggerResponse(200, typeof(PaginationViewModel<GroupManagerViewModel>))]
        public async Task<IActionResult> Search([FromBody]SearchViewModel searchVM)
        {
            var results = await _groupManagerService.SearchAsync(searchVM.Text, searchVM.Page, searchVM.PageSize);
            return Ok(results);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GroupManagerInsertViewModel groupManagerInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Field> fields = new List<Field>(5);
            foreach (var fieldId in groupManagerInsertVM.FieldsId)
            {
                var field = await _fieldService.GetAsync(x => x.Id == fieldId);
                if (field == null)
                {
                    ModelState.AddModelError("Errors", "این رشته در سیستم ثبت نشده است.");
                    return BadRequest(ModelState);
                }
                fields.Add(field);
            }
            


            var manager = _mapper.Map<AppUser>(groupManagerInsertVM);
            var user = await _userService.GetAsync(x => x.MelliCode == groupManagerInsertVM.MelliCode);
            if (user == null)
            {
                manager.UserName = manager.MelliCode;
                manager.GroupManager = new GroupManager()
                {
                    Fields = fields
                };
                await _userService.CreateUserAsync(manager, manager.MelliCode, "groupManager");
            }
            else
            {
                bool isUserInManagerRole = await _userService.IsInRoleAsync(user, "groupManager");
                if (isUserInManagerRole)
                    return BadRequest("این کاربر قبلا با نقش  مدیر گروه در سیستم ثبت شده است.");

                //user.GroupManager = manager.GroupManager;
                user.GroupManager = new GroupManager()
                {
                    Fields = fields
                };
                await _userService.AddRoleToUserAsync(user, "groupManager");
            }

            await _userService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> AddField([FromBody] GroupManagerAddFieldViewModel groupManagerAddFieldVM)
        {
            var field = await _fieldService.GetAsync(x => x.Id == groupManagerAddFieldVM.FieldId.Value);
            var manager = await _groupManagerService.GetAsync(x => x.Id == groupManagerAddFieldVM.GroupManagerId.Value);

            if (field == null || manager == null)
                return NotFound();


            if (!manager.User.Enable)
            {
                ModelState.AddModelError("Errors", "مدیر گروه غیر فعال است");
                return BadRequest(ModelState);
            }

            if (field.GroupManagerId.HasValue)
            {
                if (field.GroupManagerId.Value != manager.Id)
                {
                    ModelState.AddModelError("Errors", "این رشته توسط مدیر گروه دیگری مدیریت می شود");
                    return BadRequest(ModelState);
                }
                else
                {
                    ModelState.AddModelError("Errors", "این رشته قبلا برای مدیر گروه مورد نظر ثبت شده است");
                    return BadRequest(ModelState);
                }
            }

            field.GroupManagerId = manager.Id;
            await _groupManagerService.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveField([FromBody] GroupManagerAddFieldViewModel groupManagerAddFieldVM)
        {
            var field = await _fieldService.GetAsync(x => x.Id == groupManagerAddFieldVM.FieldId.Value);
            var manager = await _groupManagerService.GetAsync(x => x.Id == groupManagerAddFieldVM.GroupManagerId.Value);

            if (field == null || manager == null)
                return NotFound();


            if (!manager.User.Enable)
            {
                ModelState.AddModelError("Errors", "مدیر گروه غیر فعال است");
                return BadRequest(ModelState);
            }

            if (!field.GroupManagerId.HasValue)
            {
                ModelState.AddModelError("Errors", "مدیر گروهی برای این رشته ثبت نشده است");
                return BadRequest(ModelState);
            }

            if (field.GroupManagerId.HasValue)
            {
                if (field.GroupManagerId.Value != manager.Id)
                {
                    ModelState.AddModelError("Errors", "این رشته توسط مدیر گروه دیگری مدیریت می شود");
                    return BadRequest(ModelState);
                }
            }

            manager.Fields.Remove(field);
            field.GroupManagerId = null;
            await _groupManagerService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var manager = await _groupManagerService.GetAsync(x => x.Id == id);

            if (manager == null)
                return NotFound();

            await _userService.RemoveRoleAsync(manager.User, "groupManager");
            _groupManagerService.Remove(manager);
            await _groupManagerService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] GroupManagerUpdateViewModel groupManagerUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var manager = await _groupManagerService.GetAsync(x => x.Id == groupManagerUpdateVM.Id);

            if (manager == null)
                return NotFound();

            if (groupManagerUpdateVM.FieldsId != null && groupManagerUpdateVM.FieldsId.Count > 0)
            {
                List<Field> fields = new List<Field>(2);
                foreach (var fieldId in groupManagerUpdateVM.FieldsId)
                {
                    var field = await _fieldService.GetAsync(x => x.Id == fieldId);
                    if (field != null && (field.GroupManagerId == null || field.GroupManagerId == manager.Id))
                        fields.Add(field);
                }
                if (fields.Count > 0)
                {
                    await _groupManagerService.ResetFieldsAsync(manager, fields);
                }
            }


            var user = await _userService.GetAsync(x => x.Id == manager.UserId);
            user = _mapper.Map(groupManagerUpdateVM, user);

            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("Errors", "این کاربر قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            await _groupManagerService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public ActionResult Upload(IFormFileCollection formFile)
        {
            if (formFile == null || !formFile.Any())
            {
                ModelState.AddModelError("Errors", "فایلی آپلود نشده است");
                return BadRequest();
            }

            IFormFile file = formFile[0];
            
            var managers = _groupManagerService.GetAllByExcel(file);

            foreach (var manager in managers)
            {
                var isUserExists = _userService.IsExistsAsync(x => x.MelliCode == manager.MelliCode).Result;

                if (isUserExists)
                    continue;

                var user = _mapper.Map<AppUser>(manager);
                user.UserName = user.MelliCode;
                user.GroupManager = new GroupManager() { };
                var result = _userService.CreateUserAsync(user, user.MelliCode, "groupManager").Result;
                _userService.SaveChanges();
            }
            return Ok();
        }

    }
}