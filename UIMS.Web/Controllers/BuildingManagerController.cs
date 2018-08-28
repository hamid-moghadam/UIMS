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
    //[Route("api/BuildingManager")]
    public class BuildingManagerController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly BuildingManagerService _buildingManagerService;
        private readonly BuildingService _buildingService;
        private readonly BuildingClassService _buildingClassService;
        private readonly PresentationService _presentationService;
        private readonly UserService _userService;

        public BuildingManagerController(BuildingManagerService buildingManagerService, IMapper mapper, UserService userService, BuildingService buildingService, BuildingClassService buildingClassService, PresentationService presentationService)
        {
            _buildingManagerService = buildingManagerService;
            _presentationService = presentationService;
            _buildingClassService = buildingClassService;
            _buildingService = buildingService;
            _userService = userService;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<BuildingManagerViewModel>), 200)]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            var managers = await _buildingManagerService.GetAllAsync(page, pageSize);

            return Ok(managers);
        }

        [SwaggerResponse(200, typeof(BuildingManagerViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var manager = await _buildingManagerService.GetAsync(id);
            if (manager == null)
                return NotFound();
            return Ok(manager);
        }

        [SwaggerResponse(200, typeof(PaginationViewModel<BuildingClassViewModel>))]
        [HttpGet]
        public async Task<IActionResult> GetBuildingClasses(int pageSize = 5, int page = 1)
        {
            var manager = await _buildingManagerService.GetAsync(x => x.UserId == UserId);
            if (!manager.BuildingId.HasValue)
            {
                ModelState.AddModelError("Errors", "ساختمانی برای مدیر ساختمان مورد نظر ثبت نشده است");
                return BadRequest(ModelState);
            }

            var buildingClasses = await _buildingClassService.GetAllbyBuildingId(manager.BuildingId.Value,page,pageSize);

            return Ok(buildingClasses);
        }



        [HttpPost]
        [SwaggerResponse(200, typeof(PaginationViewModel<BuildingManagerViewModel>))]
        public async Task<IActionResult> Search([FromBody]SearchViewModel searchVM)
        {
            var results = await _buildingManagerService.SearchAsync(searchVM.Text, searchVM.Page, searchVM.PageSize);
            return Ok(results);
        }

        [HttpGet]
        [SwaggerResponse(200, typeof(List<PresentationBuildingManagerViewModel>))]
        public async Task<IActionResult> GetPresentations(string semester)
        {
            string currentSemester = await _buildingClassService.ParseSemesterAsync(semester);
            

            var manager = await _buildingManagerService.GetAsync(x => x.UserId == UserId);
            if (!manager.BuildingId.HasValue)
            {
                ModelState.AddModelError("Errors", "ساختمانی برای مدیر ساختمان مورد نظر ثبت نشده است");
                return BadRequest(ModelState);
            }

            var presentations = await _presentationService.GetAllByBuildingId(manager.BuildingId.Value,currentSemester);

            return Ok(presentations);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BuildingManagerInsertViewModel buildingManagerInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isBuildingExists = await _buildingService.IsExistsAsync(x => x.Id == buildingManagerInsertVM.BuildingManagerBuildingId.Value);
            if (!isBuildingExists)
            {
                ModelState.AddModelError("Errors", "این ساختمان در سیستم ثبت نشده است.");
                return BadRequest(ModelState);
            }

            var manager = _mapper.Map<AppUser>(buildingManagerInsertVM);
            var user = await _userService.GetAsync(x => x.MelliCode == buildingManagerInsertVM.MelliCode);
            if (user == null)
            {
                manager.UserName = manager.MelliCode;
                await _userService.CreateUserAsync(manager, manager.MelliCode, "buildingManager");
            }
            else
            {
                bool isUserInManagerRole = await _userService.IsInRoleAsync(user, "buildingManager");
                if (isUserInManagerRole)
                    return BadRequest("این کاربر قبلا با نقش  مدیر ساختمان در سیستم ثبت شده است.");

                user.BuildingManager = manager.BuildingManager;
                await _userService.AddRoleToUserAsync(user, "buildingManager");
            }

            await _userService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var buildingManager = await _buildingManagerService.GetAsync(x => x.Id == id);

            if (buildingManager == null)
                return NotFound();

            await _userService.RemoveRoleAsync(buildingManager.User, "buildingManager");
            _buildingManagerService.Remove(buildingManager);
            await _buildingManagerService.SaveChangesAsync();

            return Ok();
        }

        //[Authorize(Roles="admin")]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] BuildingManagerUpdateViewModel buildingManagerUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buildingManager = await _buildingManagerService.GetAsync(x => x.Id == buildingManagerUpdateVM.Id);

            if (buildingManager == null)
                return NotFound();

            if (buildingManagerUpdateVM.BuildingId.HasValue && buildingManagerUpdateVM.BuildingId.Value != 0)
            {
                var isBuildingExists = await _buildingService.IsExistsAsync(x => x.Id == buildingManagerUpdateVM.BuildingId.Value);
                if (!isBuildingExists)
                {
                    ModelState.AddModelError("Errors", "این ساختمان در سیستم ثبت نشده است.");
                    return BadRequest(ModelState);
                }

                if (await _buildingManagerService.IsExistsAsync(x => x.BuildingId != null && x.BuildingId.Value == buildingManagerUpdateVM.BuildingId.Value && x.Id != buildingManagerUpdateVM.Id))
                {
                    ModelState.AddModelError("Errors", "این ساختمان توسط مدیر ساختمان دیگری مدیریت می شود");
                    return BadRequest(ModelState);
                }

            }
            
            var user = await _userService.GetAsync(x => x.Id == buildingManager.UserId);
            user = _mapper.Map(buildingManagerUpdateVM, user);

            if (buildingManagerUpdateVM.BuildingId.HasValue && buildingManagerUpdateVM.BuildingId.Value == 0)
                buildingManager.BuildingId = null;

            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("Errors", "این کاربر قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            await _buildingManagerService.SaveChangesAsync();

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
            
            var managers = _buildingManagerService.GetAllByExcel(file);

            foreach (var manager in managers)
            {
                var isUserExists = _userService.IsExistsAsync(x => x.MelliCode == manager.MelliCode).Result;

                if (isUserExists)
                    continue;

                var user = _mapper.Map<AppUser>(manager);
                user.UserName = user.MelliCode;
                var result = _userService.CreateUserAsync(user, user.MelliCode, "buildingManager").Result;
                _userService.SaveChanges();
            }
            return Ok();
        }


    }
}