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

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/Professor")]
    public class ProfessorController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly ProfessorService _professorService;
        //private readonly BuildingService _buildingService;

        private readonly UserService _userService;

        public ProfessorController(ProfessorService professorService, IMapper mapper, UserService userService)
        {
            _professorService = professorService;
            _userService = userService;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<ProfessorViewModel>), 200)]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            var managers = await _professorService.GetAll(page, pageSize);

            return Ok(managers);
        }

        [SwaggerResponse(200, typeof(ProfessorViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var manager = await _professorService.GetAsync(id);
            if (manager == null)
                return NotFound();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProfessorInsertViewModel professorInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var professor = _mapper.Map<AppUser>(professorInsertVM);
            var user = await _userService.GetAsync(x => x.MelliCode == professorInsertVM.MelliCode);
            if (user == null)
            {
                professor.UserName = professor.MelliCode;
                await _userService.CreateUserAsync(professor, professor.MelliCode, "professor");
            }
            else
            {
                bool isUserInManagerRole = await _userService.IsInRoleAsync(user, "professor");
                if (isUserInManagerRole)
                    return BadRequest("این کاربر قبلا با نقش  استاد در سیستم ثبت شده است.");

                user.Professor = professor.Professor;
                await _userService.AddRoleToUserAsync(professor, "professor");
            }

            await _userService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var professor = await _professorService.GetAsync(x => x.Id == id);

            if (professor == null)
                return NotFound();

            _professorService.Remove(professor);
            await _professorService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] ProfessorUpdateViewModel professorUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetAsync(x => x.Id == UserId);
            user = _mapper.Map(professorUpdateVM, user);

            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("User", "این کاربر قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            await _professorService.SaveChangesAsync();

            return Ok();
        }


    }
}