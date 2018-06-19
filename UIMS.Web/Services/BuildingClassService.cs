using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UIMS.Web.Extentions;

namespace UIMS.Web.Services
{
    public class BuildingClassService : BaseService<BuildingClass, BuildingClassInsertViewModel, BuildingClassUpdateViewModel, BuildingClassViewModel>
    {
        private readonly DbSet<Presentation> _presentations;


        public BuildingClassService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _presentations = context.Set<Presentation>();
            Filters.Add("Enable", x => x.Enable);
        }


        public async Task<List<BuildingClassViewModel>> GetAllbyBuildingId(int id)
        {
            return await Entity.Where(x => x.BuildingId == id).ProjectTo<BuildingClassViewModel>().ToListAsync();
        }

        //public override Task<BuildingClassViewModel> GetAsync(int id)
        //{
        //    return Entity.Include(x=>x.Building).ProjectTo<BuildingClassViewModel>().SingleOrDefaultAsync(x => x.Id == id);
        //}

        public override void Remove(BuildingClass model)
        {
            model.Enable = false;
        }

        public override BuildingClass Update(BuildingClass model)
        {
            if (!model.Enable)
                _presentations.Where(x => x.BuildingClassId == model.Id).ForEachAsync(x => x.Enable = false).Wait();

            return base.Update(model);
        }

        public async Task<PaginationViewModel<BuildingClassViewModel>> SearchAsync(string text, int page, int pageSize)
        {
            return await Entity.Where(x => x.Name.Contains(text) || x.Building.Name.Contains(text)).ProjectTo<BuildingClassViewModel>().ToPageAsync(pageSize, page);
        }
    }
}
