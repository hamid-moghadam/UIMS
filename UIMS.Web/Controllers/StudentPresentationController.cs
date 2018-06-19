using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using UIMS.Web.Services;
using UIMS.Web.Models;
using UIMS.Web.Extentions;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using AutoMapper;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/StudentPresentation")]
    public class StudentPresentationController : ApiController
    {
        private readonly StudentPresentationService _studentPresentationService;
        private readonly StudentService _studentService;
        private readonly PresentationService _presentationService;
        private readonly UserService _userService;
        private readonly SemesterService _semesterService;
        private readonly IMapper _mapper;


        public StudentPresentationController(StudentPresentationService studentPresentationService, StudentService studentService, PresentationService presentationService, UserService userService, SemesterService semesterService, IMapper mapper)
        {
            _userService = userService;
            _semesterService = semesterService;
            _studentPresentationService = studentPresentationService;
            _mapper = mapper;
            _presentationService = presentationService;
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(Roles ="student")]
        [SwaggerResponse(200,typeof(StudentPresentationViewModel))]
        public async Task<IActionResult> GetAll(string semester)
        {
            //var user = await _userService.GetAsync(x => x.Id == UserId);
            //if (!user.Enable)
            //{
            //    ModelState.AddModelError("User", "دانشجو غیر فعال است");
            //    return BadRequest(ModelState);
            //}
            var student = await _studentService.GetAsync(x => x.UserId == UserId);
            string currentSemester = await _studentPresentationService.ParseSemester(semester);
            
            var presentations = await _studentPresentationService.GetAll(student.Id, currentSemester);

            return Ok(presentations);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StudentPresentationInsertViewModel studentPresentationInsertVM)
        {
            var student = await _studentService.GetAsync(x => x.Code == studentPresentationInsertVM.StudentCode);
            if (student == null)
                return NotFound();

            var presentaion = await _presentationService.GetAsync(x => x.Code == studentPresentationInsertVM.PresentationCode);
            if (presentaion == null)
                return NotFound();
            if (!presentaion.Enable)
            {
                ModelState.AddModelError("Errors", "کلاس مورد نظر غیر فعال شده است");
                return BadRequest(ModelState);
            }

            if (await _studentPresentationService.IsExistsAsync(x=>x.StudentId == student.Id && x.PresentationId == presentaion.Id))
            {
                ModelState.AddModelError("Errors", "این کلاس قبلا توسط دانشجو گرفته شده است");
                return BadRequest(ModelState);
            }
            await _studentPresentationService.AddAsync(studentPresentationInsertVM);
            await _studentPresentationService.SaveChangesAsync();
            return Ok();

        }


        [HttpPost]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> Update([FromBody] StudentPresentationUpdateViewModel studentPresentationUpdateVM)
        {
            var studentPresentation = await _studentPresentationService.GetAsync(x => x.Id == studentPresentationUpdateVM.Id);

            if (studentPresentation == null)
                return NotFound();

            studentPresentation = _mapper.Map(studentPresentationUpdateVM, studentPresentation);

            _studentPresentationService.Update(studentPresentation);
            await _studentPresentationService.SaveChangesAsync();

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
            var studentPresentations = _studentPresentationService.GetAllByExcel(file);

            foreach (var studentInsert in studentPresentations)
            {
                var student = _studentService.GetAsync(x => x.Code == studentInsert.StudentCode && x.User.Enable).Result;
                var presentation = _presentationService.GetAsync(x => x.Code == studentInsert.PresentationCode && x.Enable).Result;

                if (presentation == null || student == null)
                    continue;

                var isStudentPresentationExists = _studentPresentationService.IsExistsAsync(x => x.PresentationId == presentation.Id && x.StudentId == student.Id).Result;
                if (isStudentPresentationExists)
                    continue;

                var studentResult = _studentPresentationService.AddAsync(new StudentPresentation()
                {
                    PresentationId = presentation.Id,
                    StudentId = student.Id
                }).Result;
                _studentPresentationService.SaveChanges();
            }
            return Ok();
        }
    }
}