using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace UIMS.Web.Services
{
    public class SemesterService : BaseService<Semester, SemesterInsertViewModel, SemesterUpdateViewModel, SemesterViewModel>
    {
        public SemesterService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Semester> GetCurrent()
        {
            return await Entity.SingleOrDefaultAsync(x => x.Enable);
        }
    }
}
