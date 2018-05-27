using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using AutoMapper;
using UIMS.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace UIMS.Web.Services
{
    public class UserService:BaseService<AppUser, UserInsertViewModel, UserUpdateViewModel, UserViewModel>
    {
        private readonly UserManager<AppUser> _userManager;


        public UserService(DataContext context, IMapper mapper, UserManager<AppUser> userManager) : base(context, mapper)
        {
            _userManager = userManager;
        }

        //public override Task<PaginationViewModel<TModel>> GetAll<TModel>(int page, int pageSize)
        //{
        //    return base.GetAll<TModel>(page, pageSize);
        //}

        public async Task<IdentityResult> AddPasswordToUserAsync(AppUser user)
        {
            return await _userManager.AddPasswordAsync(user, user.PhoneNumber);
        }


        public async Task<IdentityResult> AddRoleToUserAsync(AppUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> UpdateUserAsync(AppUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<AppUser> GetUserByPhone(string phoneNumber)
        {
            return await Entity.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await Entity.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<string> GenerateToken(AppUser user)
        {
            return await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
        }

        public async Task<List<string>> GetRolesAsync(AppUser user)
        {
            return (await _userManager.GetRolesAsync(user)).ToList();
        }
        public async Task<bool> IsTokenVerified(AppUser user, string token)
        {
            return await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, token);
            //userManager.getuser
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password, string role, bool ignoreExistsChecking = false)
        {
            if (!ignoreExistsChecking)
            {
                var exists = await IsExistsAsync(user);
                if (exists)
                    throw new Exception("user has exists");
            }


            await _userManager.CreateAsync(user, password);
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<AppUser> IsUserValidAsync(string username, string password)
        {
            var user = await GetUserByUsername(username);
            if (user == null)
                return null;
            var isValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isValid)
                return null;
            return user;
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string oldP, string newP)
        {
            return await _userManager.ChangePasswordAsync(user, oldP, newP);
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string newP)
        {
            await _userManager.RemovePasswordAsync(user);
            return await _userManager.AddPasswordAsync(user, newP);
        }

        public async Task<IdentityResult> AddClaimAsync(AppUser user, Claim claim)
        {
            return await _userManager.AddClaimAsync(user, claim);
        }


        public async Task<List<string>> SetClaimRoles(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
            }
            return roles.ToList();
        }

        public async Task<bool> IsExistsAsync(AppUser entity)
        {
            return await Entity.AnyAsync(x => (x.UserName == entity.UserName || x.PhoneNumber == entity.PhoneNumber || x.MelliCode == entity.MelliCode) && x.Id != entity.Id);
        }
    }
}
