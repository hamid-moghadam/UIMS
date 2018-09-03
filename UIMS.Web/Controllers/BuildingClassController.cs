using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.Services;
using AutoMapper;
using UIMS.Web.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/BuildingClass")]
    public class BuildingClassController : ApiController
    {
        private readonly BuildingClassService _buildingClassService;
        private readonly UserService _userService;
        private readonly BuildingService _buildingService;
        private readonly IMapper _mapper;


        public BuildingClassController(BuildingClassService buildingClassService, IMapper mapper, UserService userService, BuildingService buildingService)
        {
            _buildingClassService = buildingClassService;
            _userService = userService;
            _buildingService = buildingService;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BuildingClassInsertViewModel buildingClassVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            int buildingId = 0;
            if (buildingClassVM.BuildingId.HasValue)
            {
                buildingId = buildingClassVM.BuildingId.Value;
                if (!await _buildingService.IsExistsAsync(x=>x.Id == buildingId))
                {
                    ModelState.AddModelError("Errors", "ساختمان مورد نظر یافت نشد");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                var user = await _userService.GetAsync(x => x.Id == UserId);
                if (!user.BuildingManager.BuildingId.HasValue)
                {
                    ModelState.AddModelError("Errors", "مدیر ساختمان مورد نظر هیچ ساختمانی را مدیریت نمی کند");
                    return BadRequest(ModelState);
                }
                buildingId = user.BuildingManager.BuildingId.Value;
            }
            if (await _buildingClassService.IsExistsAsync(x => x.Name == buildingClassVM.Name && x.BuildingId == buildingId))
            {
                ModelState.AddModelError("Errors", "این کلاس قبلا در این ساختمان ثبت شده است");
                return BadRequest(ModelState);
            }

            await _buildingClassService.AddAsync(buildingClassVM);
            await _buildingClassService.SaveChangesAsync();
            return Ok();

        }

        [SwaggerResponse(200, typeof(BuildingClassViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await _buildingClassService.GetAsync(id);
            if (course == null)
                return NotFound();

            return Ok(course);

        }

        [SwaggerResponse(200, typeof(PaginationViewModel<BuildingClassViewModel>))]
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] FilterInputViewModel filterInputVM)
        {
            //return Ok(await _buildingClassService.GetAllAsync(filterInputVM.Filters,filterInputVM.Page,filterInputVM.PageSize));
            return Ok(await _buildingClassService.GetAllAsync(filterInputVM));
        }

        [HttpPost]
        [SwaggerResponse(200, typeof(PaginationViewModel<BuildingClassViewModel>))]
        public async Task<IActionResult> Search([FromBody]SearchViewModel searchVM)
        {
            var results = await _buildingClassService.SearchAsync(searchVM.Text, searchVM.Page, searchVM.PageSize);
            return Ok(results);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var course = await _buildingClassService.GetAsync(x => x.Id == id);
            if (course == null)
                return NotFound();

            _buildingClassService.Remove(course);
            await _buildingClassService.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] BuildingClassUpdateViewModel buildingClassUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var buildingClass = await _buildingClassService.GetAsync(x => x.Id == buildingClassUpdateVM.Id);
            if (buildingClass == null)
                return NotFound();
            if (await _buildingClassService.IsExistsAsync(x => (x.BuildingId == buildingClass.BuildingId && x.Name == buildingClassUpdateVM.Name && x.Id != buildingClass.Id)))
            {
                ModelState.AddModelError("Errors", "این کلاس قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            buildingClass = _mapper.Map(buildingClassUpdateVM, buildingClass);
            _buildingClassService.Update(buildingClass);
            await _buildingClassService.SaveChangesAsync();

            return Ok();
        }


    }
}