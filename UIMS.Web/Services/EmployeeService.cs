using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace UIMS.Web.Services
{
    public class EmployeeService : BaseService<Employee, EmployeeInsertViewModel, EmployeeUpdateViewModel, EmployeeViewModel>
    {
        private readonly UserService _userService;

        public EmployeeService(DataContext context, IMapper mapper, UserService userService) : base(context, mapper)
        {
            _userService = userService;
        }

        public override async Task<Employee> GetAsync(Expression<Func<Employee, bool>> expression)
        {
            return await Entity.Include(x => x.User).SingleOrDefaultAsync(expression);
            //return Entity.GetAsync(expression);
        }

        public override Task<Employee> AddAsync(Employee model)
        {
            model.User.UserName = model.User.MelliCode;
            
            return base.AddAsync(model);
        }

        public override void Remove(Employee model)
        {
            _userService.Remove(model.User);
            //base.Remove(model);
        }

    }
}
