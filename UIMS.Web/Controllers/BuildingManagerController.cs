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

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/BuildingManager")]
    public class BuildingManagerController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly BuildingManagerService _buildingManagerService;
        private readonly BuildingService _buildingService;

        private readonly UserService _userService;

        public BuildingManagerController(BuildingManagerService buildingManagerService, IMapper mapper, UserService userService, BuildingService buildingService)
        {
            _buildingManagerService = buildingManagerService;
            _buildingService = buildingService;
            _userService = userService;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<BuildingManagerViewModel>), 200)]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            var managers = await _buildingManagerService.GetAll(page, pageSize);

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
                ModelState.AddModelError("Building", "این ساختمان در سیستم ثبت نشده است.");
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

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] BuildingManagerUpdateViewModel buildingManagerUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var buildingId = buildingManagerUpdateVM.BuildingManagerBuildingId.HasValue ? buildingManagerUpdateVM.BuildingManagerBuildingId.Value : user.BuildingManager.BuildingId;
            //if (buildingManagerUpdateVM.BuildingManagerBuildingId.HasValue)
            //{
            //    var isBuildingExists = await _buildingService.IsExistsAsync(x => x.Id == buildingId);
            //    if (!isBuildingExists)
            //    {
            //        ModelState.AddModelError("Building", "این ساختمان در سیستم ثبت نشده است.");
            //        return BadRequest(ModelState);
            //    }
            //}
            //converting when BuildingId is null not working and change BuildingID to 0
            //user.BuildingManager.BuildingId = buildingId;


            var user = await _userService.GetAsync(x => x.Id == UserId);
            user = _mapper.Map(buildingManagerUpdateVM, user);
            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("User", "این کاربر قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            await _buildingManagerService.SaveChangesAsync();

            return Ok();
        }



    }
}