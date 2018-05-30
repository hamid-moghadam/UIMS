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

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/User")]
    public class UserController : ApiController
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;


        public UserController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<UserViewModel>), 200)]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            var users = await _userService.GetAll<UserViewModel>(page, pageSize);

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

        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse(200, typeof(LoginInfoViewModel), "موفق")]
        [SwaggerResponse(400, typeof(string), "نام کاربری یا پسورد وارد نشده است  - نام کاربری یا کلمه عبور اشتباه است")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("User-Pass", "نام کاربری یا پسورد وارد نشده است");
                return BadRequest(ModelState);
            }
            var user = await _userService.IsUserValidAsync(login.Username, login.Password);
            if (user == null)
            {
                ModelState.AddModelError("Wrong User or Password", "نام کاربری یا کلمه عبور اشتباه است");
                return BadRequest(ModelState);
            }
            if (!user.Enable)
            {
                ModelState.AddModelError("User Is Disabled", "کاربر غیر فعال شده است.");
                return BadRequest(ModelState);
            }

            var token = GetJWTToken(user, await _userService.GetRolesAsync(user));
            var userVM = _mapper.Map<UserViewModel>(user);
            return Ok(new LoginInfoViewModel{ Token = token, UserViewModel = userVM });

        }


        [HttpPost]
        [Authorize]
        [SwaggerResponse(200, typeof(UserViewModel), "موفق")]
        [SwaggerResponse(400, typeof(UserViewModel), "شماره همراه یا تلفن یا ای دی صحیح نیست")]
        public async Task<IActionResult> Update([FromBody]UserUpdateViewModel userVM)
        {
            int id = UserId;
            if (userVM.AdminEditPermitted && Roles.Contains("admin"))
                id = userVM.Id;

            if (!ModelState.IsValid || id == 0)
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
            var user = await _userService.GetAsync(x=>x.Id == id);

            user = _mapper.Map(userVM, user);

            if (await _userService.IsExistsAsync(user))
            {
                ModelState.AddModelError("Error", "مشخصات کاربری قبلا در سیستم ثبت شده است.");
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
                ModelState.AddModelError("OldPasswordError", "پسورد وارد شده صحیح نیست");
                ModelState.AddModelError("NewPasswordError", "پسورد جدید باید بیش از 5 رقم باشد");
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
                ModelState.AddModelError("User", "کاربر پیدا نشد");
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
                ModelState.AddModelError("Type Error", "The Type Must Be Either admin or supervisor");
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
                ModelState.AddModelError("Error", "مشخصات کاربری قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
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
                ModelState.AddModelError("Bad Username Or Password", "پسورد یا نام کاربری اشتباه است.");
                return BadRequest(ModelState);
            }

            return Ok(new { token = GetJWTToken(user, await _userService.GetRolesAsync(user)) });
        }


        [NonAction]
        public string GetJWTToken(AppUser user, List<string> role)
        {
            var token = JwtToken.GetSecurityToken(user, role);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}