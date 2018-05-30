using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace UIMS.Web.Services
{
    public class StudentService : BaseService<Student, StudentInsertViewModel, StudentUpdateViewModel, StudentViewModel>
    {
        private readonly UserService _userService;

        public StudentService(DataContext context, IMapper mapper, UserService userService) : base(context, mapper)
        {
            _userService = userService;
        }

        public override async Task<Student> GetAsync(Expression<Func<Student, bool>> expression)
        {
            return await Entity.Include(x => x.User).SingleOrDefaultAsync(expression);
            //return Entity.GetAsync(expression);
        }

        public override Task<Student> AddAsync(Student model)
        {
            model.User.UserName = model.User.MelliCode;

            return base.AddAsync(model);
        }

        public override void Remove(Student model)
        {
            _userService.Remove(model.User);
            //base.Remove(model);
        }
    }
}
