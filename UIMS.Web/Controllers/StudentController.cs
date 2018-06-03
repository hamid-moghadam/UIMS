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
            var students = await _studentService.GetAll(page, pageSize);

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
        public async Task<IActionResult> Add([FromBody] StudentInsertViewModel studentInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isCodeExists = await _studentService.IsExistsAsync(x => x.Code == studentInsertVM.StudentCode);
            if (isCodeExists)
            {
                ModelState.AddModelError("Student", "شماره دانشجویی قبلا در سیستم ثبت شده است.");
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

            if (studentUpdateVM.Code != null)
            {
                var isStudentExists = await _studentService.IsExistsAsync(x => x.Code == studentUpdateVM.Code && x.Id != UserId);
                if (isStudentExists)
                {
                    ModelState.AddModelError("Student", "این دانش آموز قبلا در سیستم ثبت شده است.");
                    return BadRequest(ModelState);
                }
            }


            var user = await _userService.GetAsync(x => x.Id == UserId);
            user = _mapper.Map(studentUpdateVM, user);

            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("User", "این کاربر قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            await _studentService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public ActionResult Upload(IFormFileCollection formFile)
        {
            List<StudentInsertViewModel> students = new List<StudentInsertViewModel>(5);
            
            IFormFile file = formFile[0];
            if (file != null && file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                using (var stream = new FileStream(file.FileName, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.Any(d => d.CellType == CellType.Blank) || row.Cells.Count != 4) continue;

                        string name = row.GetCell(0).ToString();
                        string family = row.GetCell(1).ToString();
                        string melliCode = row.GetCell(2).ToString();
                        string studentCode = row.GetCell(3).ToString();

                        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(family) || string.IsNullOrEmpty(melliCode) || string.IsNullOrEmpty(studentCode))
                            continue;

                        if (!melliCode.IsNumber() || !studentCode.IsNumber())
                            continue;

                        students.Add(new StudentInsertViewModel()
                        {
                             Name = name,
                             Family = family,
                             MelliCode = melliCode,
                             StudentCode = studentCode
                        });
                    }
                }
            }
            foreach (var studentInsertVM in students)
            {
                var isStudentExists = _studentService.IsExistsAsync(x => x.Code == studentInsertVM.StudentCode).Result;
                var isUserExists = _userService.IsExistsAsync(x => x.MelliCode == studentInsertVM.MelliCode).Result;

                if (isUserExists || isStudentExists)
                    continue;

                var user = _mapper.Map<AppUser>(studentInsertVM);
                user.UserName = user.Student.Code;
                var result = _userService.CreateUserAsync(user, user.MelliCode, "student").Result;

            }
            return Ok(_userService.SaveChanges());
        }

    }
}