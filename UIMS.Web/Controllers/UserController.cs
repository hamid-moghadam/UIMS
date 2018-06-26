using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using UIMS.Web.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;
using UIMS.Web.Models;
using UIMS.Web.Extentions.JWT;
using System.IdentityModel.Tokens.Jwt;
using UIMS.Web.Extentions;
using Microsoft.AspNetCore.Identity;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/User")]
    public class UserController : ApiController
    {
        private readonly UserService _userService;
        private readonly SemesterService _semesterService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;


        public UserController(UserService userService, IMapper mapper, SemesterService semesterService, SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _mapper = mapper;
            _signInManager = signInManager;
            _semesterService = semesterService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginationViewModel<UserViewModel>), 200)]
        public async Task<IActionResult> GetAll([FromBody] UserGetAllInputViewModel userGetAllInputVM)
        {
            var users = await _userService.GetAll(userGetAllInputVM.Role,userGetAllInputVM.Page,userGetAllInputVM.PageSize);

            return Ok(users);
        }

        //[Authorize(Policy = "admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserViewModel), 200)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var user = await _userService.GetAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserPartialViewModel), 200)]
        public async Task<IActionResult> GetInfo()
        {
            var user = await _userService.GetAsync<UserPartialViewModel>(UserId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse(200, typeof(LoginInfoViewModel), "موفق")]
        [SwaggerResponse(400, typeof(string), "نام کاربری یا پسورد وارد نشده است  - نام کاربری یا کلمه عبور اشتباه است")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Errors", "نام کاربری یا پسورد وارد نشده است");
                return BadRequest(ModelState);
            }
            var user = await _userService.IsUserValidAsync(login.Username, login.Password);
            //await _signInManager.SignInAsync(user, false);
            //userTest.
            if (user == null)
            {
                ModelState.AddModelError("Errors", "نام کاربری یا کلمه عبور اشتباه است");
                return BadRequest(ModelState);
            }
            if (!user.Enable)
            {
                ModelState.AddModelError("Errors", "کاربر غیر فعال شده است.");
                return BadRequest(ModelState);
            }
            if (user.LastLogin == null)
                user.LastLogin = DateTime.Now;

            var token = GetJWTToken(user, await _userService.GetRolesAsync(user));
            var semester = await _semesterService.GetCurrentAsycn();
            var userVM = _mapper.Map<UserLoginViewModel>(user);
            user.LastLogin = DateTime.Now;
            await _userService.SaveChangesAsync();
            return Ok(new LoginInfoViewModel{ Token = token,Semester = semester.Name, UserLoginViewModel = userVM });

        }


        [HttpPost]
        [SwaggerResponse(200, typeof(UserViewModel), "موفق")]
        [SwaggerResponse(400, typeof(UserViewModel), "شماره همراه یا تلفن یا ای دی صحیح نیست")]
        public async Task<IActionResult> Update([FromBody]UserUpdateViewModel userVM)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if (userVM.PhoneNumber != null && !userVM.PhoneNumber.IsNumber())
            //{
            //    ModelState.AddModelError("PhoneNumber", "شماره همراه وارد شده صحیح نمی باشد.");
            //    return BadRequest(ModelState);
            //}
            //if (!userVM.MelliCode.IsNumber())
            //{
            //    ModelState.AddModelError("MelliCodeError", "کد ملی باید عدد باشد");
            //    return BadRequest(ModelState);
            //}
            var user = await _userService.GetAsync(x=>x.Id == userVM.Id);

            if (user == null)
                return NotFound();

            user = _mapper.Map(userVM, user);

            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("Errors", "مشخصات کاربری قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            await _userService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        [SwaggerResponse(200, typeof(string), "موفق")]
        [SwaggerResponse(400, typeof(string), "ناموفق")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel passwordVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetAsync(x=>x.Id == UserId);
            var result = await _userService.ChangePasswordAsync(user, passwordVM.OldPassword, passwordVM.NewPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Errors", "پسورد وارد شده صحیح نیست");
                ModelState.AddModelError("Errors", "پسورد جدید باید بیش از 5 رقم باشد");
                return BadRequest(ModelState);
            }
            await _userService.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        //[Authorize(Policy = "admin")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel adminChangePasswordVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetUserByUsername(adminChangePasswordVM.Username);
            if (user == null)
            {
                ModelState.AddModelError("Errors", "کاربر پیدا نشد");
                return BadRequest(ModelState);
            }
            await _userService.ChangePasswordAsync(user, adminChangePasswordVM.Password);
            await _userService.SaveChangesAsync();

            return Ok();
        }


        //[Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]UserInsertViewModel addUserVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userType = addUserVM.Type.ToLower();
            if (userType != "admin" && userType != "supervisor")
            {
                ModelState.AddModelError("Errors", "کاربر باید یا ادمین باشد و یا ناظر سامانه");
                return BadRequest(ModelState);
            }
            //if (!addUserVM.MelliCode.IsNumber())
            //{
            //    ModelState.AddModelError("MelliCodeError", "کد ملی باید عدد باشد");
            //    return BadRequest(ModelState);
            //}

            var user = _mapper.Map<AppUser>(addUserVM);

            if (await _userService.IsExistsAsync(user))
            {
                await _userService.AddRoleToUserAsync(user, userType);
                await _userService.SaveChangesAsync();
                return Ok();
                //ModelState.AddModelError("Errors", "مشخصات کاربری قبلا در سیستم ثبت شده است.");
                //return BadRequest(ModelState);
            }
            await _userService.CreateUserAsync(user, addUserVM.Password, addUserVM.Type);
            await _userService.SaveChangesAsync();

            return Ok(new { token = GetJWTToken(user, new List<string>() { addUserVM.Type }) });
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.IsUserValidAsync(loginVM.Username, loginVM.Password);
            if (user == null)
            {
                ModelState.AddModelError("Errors", "پسورد یا نام کاربری اشتباه است.");
                return BadRequest(ModelState);
            }

            return Ok(new { token = GetJWTToken(user, await _userService.GetRolesAsync(user)) });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var user = await _userService.GetAsync(x => x.Id == id);

            if (user == null)
                return NotFound();

            _userService.Remove(user);
            await _userService.SaveChangesAsync();

            return Ok();
        }


        [NonAction]
        public string GetJWTToken(AppUser user, List<string> role)
        {
            var token = JwtToken.GetSecurityToken(user, role);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}