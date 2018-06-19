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
    public class GroupManagerService : BaseService<GroupManager, GroupManagerInsertViewModel, GroupManagerUpdateViewModel, GroupManagerViewModel>
    {
        private readonly UserService _userService;
        private readonly DbSet<Field> _fields;
        public GroupManagerService(DataContext context, IMapper mapper, UserService userService) : base(context, mapper)
        {
            _userService = userService;
            _fields = context.Set<Field>();
        }


        public override async Task<GroupManager> GetAsync(Expression<Func<GroupManager, bool>> expression)
        {
            return await Entity.Include(x => x.User).SingleOrDefaultAsync(expression);
            //return Entity.GetAsync(expression);
        }

        public override Task<GroupManager> AddAsync(GroupManager model)
        {
            model.User.UserName = model.User.MelliCode;

            return base.AddAsync(model);
        }

        public override void Remove(GroupManager model)
        {
            //_userService.Remove(model.User);
            base.Remove(model);
        }

        public List<GroupManagerInsertViewModel> GetAllByExcel(IFormFile file)
        {
            List<GroupManagerInsertViewModel> managers = new List<GroupManagerInsertViewModel>(5);
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

                managers.Add(new GroupManagerInsertViewModel()
                {
                    Name = name,
                    Family = family,
                    MelliCode = melliCode,
                });
            }
            return managers;
        }

        public async Task<PaginationViewModel<GroupManagerViewModel>> SearchAsync(string text, int page, int pageSize)
        {
            return await Entity.Where(x => x.User.FullName.Contains(text) || x.User.MelliCode.Contains(text)).ProjectTo<GroupManagerViewModel>().ToPageAsync(pageSize, page);
        }

        public async Task ResetFieldsAsync(GroupManager manager,List<Field> fields)
        {
            await _fields.Where(x => x.GroupManagerId == manager.Id).ForEachAsync(x => x.GroupManagerId = null);
            await SaveChangesAsync();
            manager.Fields = fields;
        }

    }
}
