using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using UIMS.Web.Services;
using AutoMapper;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/Course")]
    public class CourseController : ApiController
    {
        private readonly CourseService _courseService;
        private readonly IMapper _mapper;


        public CourseController(CourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CourseInsertViewModel courseInsertVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _courseService.IsExistsAsync(x=>x.Code == courseInsertVM.Code || x.Name == courseInsertVM.Name))
            {
                ModelState.AddModelError("Errors", "این درس قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            await _courseService.AddAsync(courseInsertVM);
            await _courseService.SaveChangesAsync();
            return Ok();

        }

        [SwaggerResponse(200, typeof(CourseViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await _courseService.GetAsync(id);
            if (course == null)
                return NotFound();

            return Ok(course);

        }

        [SwaggerResponse(200, typeof(PaginationViewModel<CourseViewModel>))]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            return Ok(await _courseService.GetAllAsync(page, pageSize));
        }

        [HttpPost]
        [SwaggerResponse(200, typeof(PaginationViewModel<CourseViewModel>))]
        public async Task<IActionResult> Search([FromBody]SearchViewModel searchVM)
        {
            var results = await _courseService.SearchAsync(searchVM.Text, searchVM.Page, searchVM.PageSize);
            return Ok(results);
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var course = await _courseService.GetAsync(x => x.Id == id);
            if (course == null)
                return NotFound();

            _courseService.Remove(course);
            await _courseService.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CourseUpdateViewModel courseUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _courseService.IsExistsAsync(x=>(x.Name == courseUpdateVM.Name || x.Code == courseUpdateVM.Code) && x.Id != courseUpdateVM.Id))
            {
                ModelState.AddModelError("Errors", "این درس قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            var course = await _courseService.GetAsync(x => x.Id == courseUpdateVM.Id);
            if (course == null)
                return NotFound();

            course = _mapper.Map(courseUpdateVM, course);
            _courseService.Update(course);
            await _courseService.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        public IActionResult Upload(IFormFileCollection formFile)
        {

            if (formFile == null || !formFile.Any())
            {
                ModelState.AddModelError("Errors", "فایلی آپلود نشده است");
                return BadRequest(ModelState);
            }

            IFormFile file = formFile[0];
            var courses = _courseService.GetAllByExcel(file);

            foreach (var courseInsert in courses)
            {
                var isCourseExists = _courseService.IsExistsAsync(x => x.Name == courseInsert.Name || x.Code == courseInsert.Code).Result;
                if (isCourseExists)
                    continue;

                var courseResult = _courseService.AddAsync(courseInsert).Result;
                _courseService.SaveChanges();
            }
            return Ok();
        }


    }
}