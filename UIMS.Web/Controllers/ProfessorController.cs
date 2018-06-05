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
using UIMS.Web.Extentions;
using NPOI.SS.UserModel;

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
            var professors = await _professorService.GetAll(page, pageSize);

            return Ok(professors);
        }

        [SwaggerResponse(200, typeof(ProfessorViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var professor = await _professorService.GetAsync(id);
            if (professor == null)
                return NotFound();
            return Ok(professor);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProfessorInsertViewModel professorInsertVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var professorUser = _mapper.Map<AppUser>(professorInsertVM);
            var user = await _userService.GetAsync(x => x.MelliCode == professorInsertVM.MelliCode);
            if (user == null)
            {
                professorUser.UserName = professorUser.MelliCode;
                professorUser.Professor = new Professor(); 
                await _userService.CreateUserAsync(professorUser, professorUser.MelliCode, "professor");
            }
            else
            {
                bool isUserInManagerRole = await _userService.IsInRoleAsync(user, "professor");
                if (isUserInManagerRole)
                    return BadRequest("این کاربر قبلا با نقش  استاد در سیستم ثبت شده است.");

                //user.Professor = professorUser.Professor;
                user.Professor = new Professor();
                await _userService.AddRoleToUserAsync(user, "professor");
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

            await _userService.RemoveRoleAsync(professor.User, "professor");
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

        [HttpPost]
        public ActionResult Upload(IFormFileCollection formFile)
        {
            if (formFile == null || !formFile.Any())
            {
                ModelState.AddModelError("File Not Found", "فایلی آپلود نشده است");
                return BadRequest();
            }

            IFormFile file = formFile[0];
            var professors = _professorService.GetAllByExcel(file);

            foreach (var professor in professors)
            {
                var isUserExists = _userService.IsExistsAsync(x => x.MelliCode == professor.MelliCode).Result;

                if (isUserExists)
                    continue;

                var user = _mapper.Map<AppUser>(professor);
                user.UserName = user.MelliCode;
                user.Professor = new Professor() { };
                var result = _userService.CreateUserAsync(user, user.MelliCode, "professor").Result;

            }
            return Ok(_userService.SaveChanges());
        }

    }
}