using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using Microsoft.AspNetCore.Identity;
using UIMS.Web.Extentions;
using AutoMapper.QueryableExtensions;

namespace UIMS.Web.Services
{
    public class RoleService : BaseService<AppRole, AppRoleInsertViewModel, AppRoleUpdateViewModel, AppRoleViewModel>
    {

        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(DataContext context, IMapper mapper, RoleManager<AppRole> roleManager) : base(context, mapper)
        {
            _roleManager = roleManager;
        }

        public async override Task<PaginationViewModel<AppRoleViewModel>> GetAllAsync(int page, int pageSize)
        {
            return await _roleManager.Roles.ProjectTo<AppRoleViewModel>().ToPageAsync(pageSize, page);
            //return base.GetAllAsync(page, pageSize);
        }

        public async override Task<AppRole> AddAsync(AppRole model)
        {
            await _roleManager.CreateAsync(model);
            return model;
            //return base.AddAsync(model);
        }
    }
}
