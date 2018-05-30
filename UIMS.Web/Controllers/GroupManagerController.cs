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
    //[Route("api/GroupManager")]
    public class GroupManagerController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly GroupManagerService _groupManagerService;
        private readonly FieldService _fieldService;
        private readonly UserService _userService;

        public GroupManagerController(GroupManagerService groupManagerService, IMapper mapper, UserService userService, FieldService fieldService)
        {
            _groupManagerService = groupManagerService;
            _fieldService = fieldService;
            _userService = userService;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<GroupManagerViewModel>), 200)]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            var managers = await _groupManagerService.GetAll(page, pageSize);

            return Ok(managers);
        }

        [SwaggerResponse(200, typeof(BuildingManagerViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var manager = await _groupManagerService.GetAsync(id);
            if (manager == null)
                return NotFound();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GroupManagerInsertViewModel groupManagerInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isFieldExists = await _fieldService.IsExistsAsync(x => x.Id == groupManagerInsertVM.GroupManagerFieldId.Value);
            if (!isFieldExists)
            {
                ModelState.AddModelError("Field", "این رشته در سیستم ثبت نشده است.");
                return BadRequest(ModelState);
            }


            var manager = _mapper.Map<AppUser>(groupManagerInsertVM);
            var user = await _userService.GetAsync(x => x.MelliCode == groupManagerInsertVM.MelliCode);
            if (user == null)
            {
                manager.UserName = manager.MelliCode;
                await _userService.CreateUserAsync(manager, manager.MelliCode, "groupManager");
            }
            else
            {
                bool isUserInManagerRole = await _userService.IsInRoleAsync(user, "groupManager");
                if (isUserInManagerRole)
                    return BadRequest("این کاربر قبلا با نقش  مدیر گروه در سیستم ثبت شده است.");

                user.GroupManager = manager.GroupManager;
                await _userService.AddRoleToUserAsync(manager, "groupManager");
            }

            await _userService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var manager = await _groupManagerService.GetAsync(x => x.Id == id);

            if (manager == null)
                return NotFound();

            _groupManagerService.Remove(manager);
            await _groupManagerService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] GroupManagerUpdateViewModel groupManagerUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (groupManagerUpdateVM.GroupManagerFieldId.HasValue)
            {
                var isFieldExists = await _fieldService.IsExistsAsync(x => x.Id == groupManagerUpdateVM.GroupManagerFieldId.Value);
                if (!isFieldExists)
                {
                    ModelState.AddModelError("Field", "این رشته در سیستم ثبت نشده است.");
                    return BadRequest(ModelState);
                }
            }


            var user = await _userService.GetAsync(x => x.Id == UserId);
            user = _mapper.Map(groupManagerUpdateVM, user);

            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("User", "این کاربر قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            await _groupManagerService.SaveChangesAsync();

            return Ok();
        }
    }
}