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


        public StudentPresentationController(StudentPresentationService studentPresentationService, StudentService studentService, PresentationService presentationService, UserService userService, SemesterService semesterService)
        {
            _userService = userService;
            _semesterService = semesterService;
            _studentPresentationService = studentPresentationService;
            _presentationService = presentationService;
            _studentService = studentService;
        }

        [HttpGet]
        [SwaggerResponse(200,typeof(StudentPresentationViewModel))]
        [Authorize(Roles ="student")]
        public async Task<IActionResult> GetAll(string semester="")
        {
            var user = await _userService.GetAsync(x => x.Id == UserId);
            if (!user.Enable)
            {
                ModelState.AddModelError("User", "دانشجو غیر فعال است");
                return BadRequest(ModelState);
            }
            if (semester != "")
            {
                if (!semester.IsSemester())
                {
                    ModelState.AddModelError("Semester", "نیمسال وارد شده صحیح نیست");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                semester = (await _semesterService.GetCurrentAsycn()).Name;
            }
            
            var presentations = await _studentPresentationService.GetAll(user.Student.Id, semester);

            return Ok(presentations);

        }

        [HttpPost]
        public IActionResult Upload(IFormFileCollection formFile)
        {

            if (formFile == null || !formFile.Any())
            {
                ModelState.AddModelError("File Not Found", "فایلی آپلود نشده است");
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