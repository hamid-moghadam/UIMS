using System;
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
        public async Task<IActionResult> GetAll(int? pageSize = 5, int? page = 1)
        {
            var employees = await _employeeService.GetAll(page.Value, pageSize.Value);

            return Ok(employees);
        }

        [SwaggerResponse(200, typeof(EmployeeViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _employeeService.GetAsync(id));
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmployeeInsertViewModel employeeInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _mapper.Map<Employee>(employeeInsertVM);

            if (await _userService.IsExistsAsync(employee.User))
            {
                ModelState.AddModelError("User", "User Exists");
                return BadRequest(ModelState);
            }

            await _employeeService.AddAsync(employee);
            await _employeeService.SaveChangesAsync();

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

            var employeeUpdate = _mapper.Map<Employee>(employeeUpdateVM);

            if (await _userService.IsExistsAsync(employeeUpdate.User))
            {
                ModelState.AddModelError("User", "User Exists");
                return BadRequest(ModelState);
            }

            _employeeService.Update(employeeUpdate);

            await _employeeService.SaveChangesAsync();

            return Ok();
        }
    }
}