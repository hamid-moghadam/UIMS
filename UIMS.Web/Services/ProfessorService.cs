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
    public class ProfessorService : BaseService<Professor, ProfessorInsertViewModel, ProfessorUpdateViewModel, ProfessorViewModel>
    {
        private readonly UserService _userService;

        public ProfessorService(DataContext context, IMapper mapper, UserService userService) : base(context, mapper)
        {
            _userService = userService;
        }

        public override async Task<Professor> GetAsync(Expression<Func<Professor, bool>> expression)
        {
            return await Entity.Include(x => x.User).SingleOrDefaultAsync(expression);
            //return Entity.GetAsync(expression);
        }

        public override Task<Professor> AddAsync(Professor model)
        {
            model.User.UserName = model.User.MelliCode;

            return base.AddAsync(model);
        }

        public override void Remove(Professor model)
        {
            //_userService.Remove(model.User);
            base.Remove(model);
        }

        public List<ProfessorInsertViewModel> GetAllByExcel(IFormFile file)
        {
            List<ProfessorInsertViewModel> professors = new List<ProfessorInsertViewModel>(5);
            var rows = new ExcelExtentions().GetRows(file);

            foreach (var row in rows)
            {
                if (row.Cells.Any(d => d.CellType == CellType.Blank) || row.Cells.Count != 3) continue;

                string name = row.GetCell(0).ToString();
                string family = row.GetCell(1).ToString();
                string melliCode = row.GetCell(2).ToString();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(family) || string.IsNullOrEmpty(melliCode))
                    continue;

                if (!melliCode.IsNumber())
                    continue;

                professors.Add(new ProfessorInsertViewModel()
                {
                    Name = name,
                    Family = family,
                    MelliCode = melliCode,
                });
            }

            return professors;
        }
    }
}
