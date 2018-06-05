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

namespace UIMS.Web.Services
{
    public class BuildingClassService : BaseService<BuildingClass, BuildingClassInsertViewModel, BuildingClassUpdateViewModel, BuildingClassViewModel>
    {
        public BuildingClassService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        //public override Task<BuildingClassViewModel> GetAsync(int id)
        //{
        //    return Entity.Include(x=>x.Building).ProjectTo<BuildingClassViewModel>().SingleOrDefaultAsync(x => x.Id == id);
        //}
    }
}
