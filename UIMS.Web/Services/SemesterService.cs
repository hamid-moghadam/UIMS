using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using UIMS.Web.Extentions;

namespace UIMS.Web.Services
{
    public class SemesterService : BaseService<Semester, SemesterInsertViewModel, SemesterUpdateViewModel, SemesterViewModel>
    {
        public SemesterService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public override async Task<PaginationViewModel<SemesterViewModel>> GetAllAsync(int page, int pageSize)
        {
            return await Entity.OrderBy(x=>!x.Enable).ProjectTo<SemesterViewModel>().ToPageAsync(pageSize, page);
        }

        public async Task<Semester> GetCurrentAsycn()
        {
            return await Entity.SingleOrDefaultAsync(x => x.Enable);
        }

        public async Task<Semester> SetCurrentAsycn(Semester semester)
        {
            await Entity.Where(x => x.Id != semester.Id).ForEachAsync(x => x.Enable = false);
            semester.Enable = true;
            return semester;
        }
    }
}
