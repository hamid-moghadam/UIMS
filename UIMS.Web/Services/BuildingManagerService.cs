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
using AutoMapper.QueryableExtensions;

namespace UIMS.Web.Services
{
    public class BuildingManagerService : BaseService<BuildingManager, BuildingManagerInsertViewModel, BuildingUpdateViewModel, BuildingManagerViewModel>
    {
        private readonly UserService _userService;

        public BuildingManagerService(DataContext context, IMapper mapper, UserService userService) : base(context, mapper)
        {
            _userService = userService;
            SearchQuery = (st) => Entity.Where(x => x.User.FullName.Contains(st) || x.User.MelliCode.Contains(st));
        }

        public override async Task<BuildingManager> GetAsync(Expression<Func<BuildingManager, bool>> expression)
        {
            return await Entity.Include(x => x.User).SingleOrDefaultAsync(expression);
            //return Entity.GetAsync(expression);
        }

        public override Task<BuildingManager> AddAsync(BuildingManager model)
        {
            model.User.UserName = model.User.MelliCode;

            return base.AddAsync(model);
        }

        public override void Remove(BuildingManager model)
        {
            //model.User
            base.Remove(model);
        }

        public List<BuildingManagerInsertViewModel> GetAllByExcel(IFormFile file)
        {
            List<BuildingManagerInsertViewModel> managers = new List<BuildingManagerInsertViewModel>(5);
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

                managers.Add(new BuildingManagerInsertViewModel()
                {
                    Name = name,
                    Family = family,
                    MelliCode = melliCode,
                });
            }

            return managers;
        }

        public async Task<PaginationViewModel<BuildingManagerViewModel>> SearchAsync(string text,int page,int pageSize)
        {
            return await Entity.Where(x => x.User.FullName.Contains(text) || x.User.MelliCode.Contains(text)).ProjectTo<BuildingManagerViewModel>().ToPageAsync(pageSize, page);
        }
    }
}
