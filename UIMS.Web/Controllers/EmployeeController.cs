﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using UIMS.Web.DTO;
using AutoMapper;
using UIMS.Web.Models;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/Employee")]
    public class EmployeeController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly EmployeeService _employeeService;
        private readonly UserService _userService;

        public EmployeeController(EmployeeService employeeService, IMapper mapper, UserService userService)
        {
            _employeeService = employeeService;
            _userService = userService;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<EmployeeViewModel>), 200)]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            var employees = await _employeeService.GetAll(page, pageSize);

            return Ok(employees);
        }

        [SwaggerResponse(200, typeof(EmployeeViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _employeeService.GetAsync(id);
            if (employee == null)
                return NotFound();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmployeeInsertViewModel employeeInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeUser = _mapper.Map<AppUser>(employeeInsertVM);
            var user = await _userService.GetAsync(x => x.MelliCode == employeeInsertVM.MelliCode);
            if (user == null)
            {
                employeeUser.UserName = employeeUser.MelliCode;
                await _userService.CreateUserAsync(employeeUser,employeeUser.MelliCode,"employee");
            }
            else
            {
                bool isUserInEmployeeRole = await _userService.IsInRoleAsync(user, "employee");
                if (isUserInEmployeeRole)
                    return BadRequest("این کاربر قبلا با نقش  کارمند در سیستم ثبت شده است.");

                user.Employee = employeeUser.Employee;
                await _userService.AddRoleToUserAsync(employeeUser, "employee");
            }

            await _userService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var employee = await _employeeService.GetAsync(x => x.Id == id);

            if (employee == null)
                return NotFound();

            _employeeService.Remove(employee);
            await _employeeService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] EmployeeUpdateViewModel employeeUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetAsync(x => x.Id == UserId);
            user = _mapper.Map(employeeUpdateVM, user);

            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("User", "User Exists");
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            await _employeeService.SaveChangesAsync();

            return Ok();
        }
    }
}