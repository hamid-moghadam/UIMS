﻿using System;
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
    //[Route("api/Student")]
    public class StudentController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly StudentService _studentService;

        private readonly UserService _userService;

        public StudentController(StudentService studentService, IMapper mapper, UserService userService)
        {
            _studentService = studentService;
            _userService = userService;
            _mapper = mapper;
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
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StudentInsertViewModel studentInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isCodeExists = await _studentService.IsExistsAsync(x => x.Code == studentInsertVM.Code);
            if (!isCodeExists)
            {
                ModelState.AddModelError("Building", "شماره دانشجویی قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            var student = _mapper.Map<AppUser>(studentInsertVM);
            var user = await _userService.GetAsync(x => x.MelliCode == studentInsertVM.MelliCode);
            if (user == null)
            {
                student.UserName = student.MelliCode;
                await _userService.CreateUserAsync(student, student.MelliCode, "student");
            }
            else
            {
                bool isUserInStudentRole = await _userService.IsInRoleAsync(user, "student");
                if (isUserInStudentRole)
                    return BadRequest("این کاربر قبلا با نقش  دانشجو در سیستم ثبت شده است.");

                user.Student = student.Student;
                await _userService.AddRoleToUserAsync(student, "student");
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
    }
}