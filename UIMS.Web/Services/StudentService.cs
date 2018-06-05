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
using Microsoft.AspNetCore.Http;
using UIMS.Web.Extentions;
using NPOI.SS.UserModel;

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
            //_userService.Remove(model.User);
            base.Remove(model);
        }

        public List<StudentInsertViewModel> GetAllByExcel(IFormFile file)
        {
            List<StudentInsertViewModel> students = new List<StudentInsertViewModel>(5);
            var rows = new ExcelExtentions().GetRows(file);

            foreach (var row in rows)
            {
                if (row.Cells.Any(d => d.CellType == CellType.Blank) || row.Cells.Count != 4) continue;

                string name = row.GetCell(0).ToString();
                string family = row.GetCell(1).ToString();
                string melliCode = row.GetCell(2).ToString();
                string studentCode = row.GetCell(3).ToString();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(family) || string.IsNullOrEmpty(melliCode) || string.IsNullOrEmpty(studentCode))
                    continue;

                if (!melliCode.IsNumber() || !studentCode.IsNumber())
                    continue;

                students.Add(new StudentInsertViewModel()
                {
                    Name = name,
                    Family = family,
                    MelliCode = melliCode,
                    StudentCode = studentCode
                });
            }
            return students;
        }
    }
}
