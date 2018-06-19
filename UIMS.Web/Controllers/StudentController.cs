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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using UIMS.Web.Extentions;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/Student")]
    public class StudentController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly StudentService _studentService;

        private readonly UserService _userService;

        private readonly IHostingEnvironment _hostingEnvironment;


        public StudentController(StudentService studentService, IMapper mapper, UserService userService, IHostingEnvironment hostingEnvironment)
        {
            _studentService = studentService;
            _userService = userService;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<StudentViewModel>), 200)]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            var students = await _studentService.GetAllAsync(page, pageSize);

            return Ok(students);
        }

        [SwaggerResponse(200, typeof(StudentViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await _studentService.GetAsync(id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpPost]
        [SwaggerResponse(200, typeof(PaginationViewModel<StudentViewModel>))]
        public async Task<IActionResult> Search([FromBody]SearchViewModel searchVM)
        {
            var results = await _studentService.SearchAsync(searchVM.Text, searchVM.Page, searchVM.PageSize);
            return Ok(results);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StudentInsertViewModel studentInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isCodeExists = await _studentService.IsExistsAsync(x => x.Code == studentInsertVM.StudentCode);
            if (isCodeExists)
            {
                ModelState.AddModelError("Errors", "شماره دانشجویی قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            var student = _mapper.Map<AppUser>(studentInsertVM);
            var user = await _userService.GetAsync(x => x.MelliCode == studentInsertVM.MelliCode);
            if (user == null)
            {
                student.UserName = student.Student.Code;
                await _userService.CreateUserAsync(student, student.MelliCode, "student");
            }
            else
            {
                bool isUserInStudentRole = await _userService.IsInRoleAsync(user, "student");
                if (isUserInStudentRole)
                    return BadRequest("این کاربر قبلا با نقش  دانشجو در سیستم ثبت شده است.");

                user.Student = student.Student;
                await _userService.AddRoleToUserAsync(user, "student");
            }

            await _userService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var student = await _studentService.GetAsync(x => x.Id == id);

            if (student == null)
                return NotFound();

            await _userService.RemoveRoleAsync(student.User, "student");
            _studentService.Remove(student);
            await _studentService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] StudentUpdateViewModel studentUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _studentService.GetAsync(x => x.Id == studentUpdateVM.Id);

            if (student == null)
                return NotFound();
            if (studentUpdateVM.StudentCode != null)
            {
                var isStudentExists = await _studentService.IsExistsAsync(x => x.Code == studentUpdateVM.StudentCode && x.Id != student.Id);
                if (isStudentExists)
                {
                    ModelState.AddModelError("Errors", "این دانش آموز قبلا در سیستم ثبت شده است.");
                    return BadRequest(ModelState);
                }
            }

            var user = await _userService.GetAsync(x => x.Id == student.UserId);
            user = _mapper.Map(studentUpdateVM, user);

            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("Errors", "این کاربر قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            await _studentService.SaveChangesAsync();

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
            var students = _studentService.GetAllByExcel(file);

            foreach (var studentInsertVM in students)
            {
                var isStudentExists = _studentService.IsExistsAsync(x => x.Code == studentInsertVM.StudentCode).Result;
                var isUserExists = _userService.IsExistsAsync(x => x.MelliCode == studentInsertVM.MelliCode).Result;

                if (isUserExists || isStudentExists)
                    continue;

                var user = _mapper.Map<AppUser>(studentInsertVM);
                user.UserName = user.Student.Code;
                var result = _userService.CreateUserAsync(user, user.MelliCode, "student").Result;
                _userService.SaveChanges();
            }
            return Ok();
        }

        
    }
}