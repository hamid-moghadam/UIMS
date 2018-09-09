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
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using UIMS.Web.Extentions;

namespace UIMS.Web.Services
{
    public class UserService:BaseService<AppUser, UserInsertViewModel, UserUpdateViewModel, UserViewModel>
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(DataContext context, IMapper mapper, UserManager<AppUser> userManager) : base(context, mapper)
        {
            _userManager = userManager;
            SearchQuery = (search) => Entity.Where(x => x.FullName.Contains(search) || x.MelliCode.Contains(search) || x.PhoneNumber.Contains(search));
            BaseQuery = BaseQuery.Where(x => x.Enable);

        }

        public override async Task<AppUser> GetAsync(Expression<Func<AppUser, bool>> expression)
        {
            return await Entity
                .Include(x => x.Professor)
                .Include(x => x.GroupManager)
                .Include(x => x.Employee)
                .Include(x => x.Student)
                .Include(x => x.BuildingManager)
                .SingleOrDefaultAsync(expression);
        }

        //public override Task<PaginationViewModel<TModel>> GetAll<TModel>(int page, int pageSize)
        //{
        //    return base.GetAll<TModel>(page, pageSize);
        //}
        public async Task<PaginationViewModel<UserViewModel>> GetAll(string role,int page, int pageSize,string searchQuery)
        {
            if (role == "")
                return await SearchQuery(searchQuery).ProjectTo<UserViewModel>().ToPageAsync(page, pageSize);

            var users = await _userManager.GetUsersInRoleAsync(role);
            var usersVM = _mapper.Map<List<UserViewModel>>(users.OrderByDescending(x=>x.Created).ToList());
            return usersVM.ToPage(pageSize, page);
        }

        public async Task<IList<AppUser>> GetAll(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users;
        }

        public async Task<IdentityResult> AddPasswordToUserAsync(AppUser user)
        {
            return await _userManager.AddPasswordAsync(user, user.PhoneNumber);
        }


        public async Task<IdentityResult> AddRoleToUserAsync(AppUser user, string role)
        {
            if (!await _userManager.IsInRoleAsync(user,role))
                return await _userManager.AddToRoleAsync(user, role);
            return null;
        }

        public async Task<IdentityResult> RemoveRoleAsync(AppUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> UpdateUserAsync(AppUser user)
        {
            //await ChangePasswordAsync(user, user.MelliCode);
            user.UserName = user.MelliCode;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<AppUser> GetUserByPhone(string phoneNumber)
        {
            return await Entity.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await Entity
                .Include(x => x.Professor)
                .Include(x => x.GroupManager)
                .Include(x => x.Employee)
                .Include(x => x.Student)
                .Include(x => x.BuildingManager)
                .SingleOrDefaultAsync(x => x.UserName == username);
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
            if (entity.UserName == null)
                return true;
            if (entity.PhoneNumber == null)
                return await Entity.AnyAsync(x => (x.UserName == entity.UserName || x.MelliCode == entity.MelliCode) && x.Id != entity.Id);
            else
                return await Entity.AnyAsync(x => (x.UserName == entity.UserName || x.PhoneNumber == entity.PhoneNumber || x.MelliCode == entity.MelliCode) && x.Id != entity.Id);

        }
        public async Task<bool> IsEnable(int userId)
        {
            return await Entity.AnyAsync(x => x.Id == userId && x.Enable);
        }

        public async Task<bool> IsInRoleAsync(AppUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public override void Remove(AppUser model)
        {
            model.Enable = false;
            //base.Remove(model);
        }

    }
}
