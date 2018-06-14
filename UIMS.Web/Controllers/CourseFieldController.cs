using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using AutoMapper;
using UIMS.Web.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/CourseField")]
    public class CourseFieldController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly CourseFieldService _courseFieldService;

        private readonly CourseService _courseService;

        private readonly FieldService _fieldService;

        public CourseFieldController(CourseService courseService,FieldService fieldService,CourseFieldService courseFieldService,IMapper mapper)
        {
            _mapper = mapper;
            _courseService = courseService;
            _fieldService = fieldService;
            _courseFieldService = courseFieldService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CourseFieldInsertViewModel courseFieldInsertVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var field = await _fieldService.GetAsync(x => x.Id == courseFieldInsertVM.FieldId.Value);
            if (field == null)
            {
                ModelState.AddModelError("Field", "رشته مورد نظر یافت نشد");
                return BadRequest(ModelState);
            }
            var course = await _courseService.GetAsync(x => x.Id == courseFieldInsertVM.CourseId.Value);
            if (course == null)
            {
                ModelState.AddModelError("Course", "درس مورد نظر یافت نشد");
                return BadRequest(ModelState);
            }

            var isAlreadyExists = await _courseFieldService.IsExistsAsync(x => x.FieldId == field.Id && x.CourseId == course.Id);
            if (isAlreadyExists)
            {
                ModelState.AddModelError("CourseField", "این درس با رشته مورد نظر قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            await _courseFieldService.AddAsync(courseFieldInsertVM);
            await _courseFieldService.SaveChangesAsync();
            return Ok();

        }

        [SwaggerResponse(200,typeof(CourseFieldViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var courseField = await _courseFieldService.GetAsync(id);
            if (courseField == null)
                return NotFound();

            return Ok(courseField);
            
        }
        [HttpGet]
        [SwaggerResponse(200, typeof(PaginationViewModel<CourseFieldViewModel>))]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            return Ok(await _courseFieldService.GetAllAsync(page, pageSize));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var courseField = await _courseFieldService.GetAsync(x=>x.Id == id);
            if (courseField == null)
                return NotFound();

            _courseFieldService.Remove(courseField);
            await _courseFieldService.SaveChangesAsync();
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CourseFieldUpdateViewModel courseFieldUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var courseField = await _courseFieldService.GetAsync(x => x.Id == courseFieldUpdateVM.Id);

            if (courseField == null)
                return NotFound();

            courseField = _mapper.Map(courseFieldUpdateVM, courseField);
            _courseFieldService.Update(courseField);
            await _courseService.SaveChangesAsync();
            return Ok();
        }
    }
}
