using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UIMS.Web.Models;
using UIMS.Web.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UIMS.Web.Controllers
{
    public class RoleController : ApiController
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly IMapper _mapper;


        public RoleController(UserService userService, IMapper mapper, RoleService roleService)
        {
            _roleService = roleService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AppRoleViewModel), 200)]
        public IActionResult GetAll()
        {
            var roles = _roleService.GetAll();

            return Ok(roles.ToList());
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody]AppRoleInsertViewModel roleInsertVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = _mapper.Map<AppRole>(roleInsertVM);
            if (await _roleService.IsExistsAsync(x => x.Name == role.Name || x.PersianName == role.PersianName))
            {
                ModelState.AddModelError("Errors", "این نفش قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            await _roleService.AddAsync(role);

            return Ok();


        }


        [HttpPost]
        public async Task<IActionResult> Update([FromBody]AppRoleUpdateViewModel appRoleUpdateVM)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = await _roleService.GetAsync(x => x.Id == appRoleUpdateVM.Id);
            if (role == null)
                return NotFound();

            role = _mapper.Map(appRoleUpdateVM, role);

            if (await _roleService.IsExistsAsync(x=>(x.Name == role.Name || x.PersianName == role.PersianName) && x.Id != role.Id))
            {
                ModelState.AddModelError("Errors", "مشخصات نقش قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }
            _roleService.Update(role);
            await _roleService.SaveChangesAsync();
            return Ok();
        }


    }
}
