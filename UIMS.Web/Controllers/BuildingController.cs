using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using UIMS.Web.Services;
using AutoMapper;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/Building")]
    public class BuildingController : ApiController
    {
        private readonly BuildingService _buildingService;

        private readonly BuildingManagerService _buildingManagerService;

        private readonly IMapper _mapper;

        public BuildingController(BuildingService buildingService, IMapper mapper, BuildingManagerService buildingManagerService)
        {
            _buildingService = buildingService;
            _buildingManagerService = buildingManagerService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BuildingInsertViewModel baseInsertVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _buildingService.IsExistsAsync(x=>x.Name == baseInsertVM.Name))
            {
                ModelState.AddModelError("Building Exists", "ساختمان مورد نظر در سیستم وجود دارد");
                return BadRequest(ModelState);
            }

            //if (baseInsertVM.BuildingManagerId.HasValue)
            //{
            //    var isManagerHasBuilding = await _buildingManagerService.IsExistsAsync(x => x.Id == baseInsertVM.BuildingManagerId.Value && x.BuildingId != 0);
            //    ModelState.AddModelError("BuildingManager", "مدیر ساختمان مورد نظر برای ساختمانی دیگر ثبت شده است.");
            //    return BadRequest(ModelState);
            //}

            await _buildingService.AddAsync(baseInsertVM);
            await _buildingService.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("{id}")]
        //[ProducesResponseType(typeof(BuildingViewModel), 200)]
        public async Task<ActionResult<BuildingViewModel>> Get(int id)
        {
            var building = await _buildingService.GetAsync(id);
            if (building == null)
                return NotFound();
            return building;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<BuildingViewModel>), 200)]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            return Ok(await _buildingService.GetAllAsync(page, pageSize));
        }

        //[HttpPost("{id}")]
        //public async Task<IActionResult> Remove(int id)
        //{
        //    var building = await _buildingService.GetAsync(x => x.Id == id);
        //    if (building == null)
        //        return NotFound();

        //    _buildingService.Remove(building);
        //    await _buildingService.SaveChangesAsync();
        //    return Ok();

        //}

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] BuildingUpdateViewModel buildingUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if (buildingUpdateVM.BuildingManagerId.HasValue)
            //{
            //    var isManagerExists = await _buildingManagerService.IsExistsAsync(x => x.Id == buildingUpdateVM.BuildingManagerId.Value && x.User.Enable);
            //    if (!isManagerExists)
            //    {
            //        ModelState.AddModelError("BuildingManager", "مدیر ساختمان مورد نظر در سیستم ثبت نشده است و یا غیر فعال است.");
            //        return BadRequest(ModelState);
            //    }
            //}
            if (await _buildingService.IsExistsAsync(x=>x.Name == buildingUpdateVM.Name))
            {
                ModelState.AddModelError("Building", "این ساختمان با نام مورد نظر قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            var building = await _buildingService.GetAsync(x => x.Id == buildingUpdateVM.Id);
            if (building == null)
                return NotFound();
            building = _mapper.Map(buildingUpdateVM, building);

            _buildingService.Update(building);
            await _buildingService.SaveChangesAsync();
            return Ok();
        }
    }
}