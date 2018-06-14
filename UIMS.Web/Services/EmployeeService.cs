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
using Microsoft.AspNetCore.Http;
using UIMS.Web.Extentions;
using NPOI.SS.UserModel;
using AutoMapper.QueryableExtensions;

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
            //_userService.Remove(model.User);
            base.Remove(model);
        }

        public List<EmployeeInsertViewModel> GetAllByExcel(IFormFile file)
        {
            List<EmployeeInsertViewModel> employees = new List<EmployeeInsertViewModel>(5);
            var rows = new ExcelExtentions().GetRows(file);

            foreach (var row in rows)
            {
                if (row.Cells.Any(d => d.CellType == CellType.Blank) || row.Cells.Count != 4) continue;

                string name = row.GetCell(0).ToString();
                string family = row.GetCell(1).ToString();
                string melliCode = row.GetCell(2).ToString();
                string post = row.GetCell(3).ToString();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(family) || string.IsNullOrEmpty(melliCode) || string.IsNullOrEmpty(post))
                    continue;

                if (!melliCode.IsNumber())
                    continue;

                employees.Add(new EmployeeInsertViewModel()
                {
                    Name = name,
                    Family = family,
                    MelliCode = melliCode,
                    EmployeePost = post
                });
            }
            return employees;

        }

        public async Task<PaginationViewModel<EmployeeViewModel>> SearchAsync(string text, int page, int pageSize)
        {
            return await Entity.Where(x => x.User.FullName.Contains(text) || x.User.MelliCode.Contains(text) || x.Post.Contains(text)).ProjectTo<EmployeeViewModel>().ToPageAsync(pageSize, page);
        }

    }
}
