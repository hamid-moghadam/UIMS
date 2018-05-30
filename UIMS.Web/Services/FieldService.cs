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
using AutoMapper.QueryableExtensions;

namespace UIMS.Web.Services
{
    public class FieldService : BaseService<Field, FieldInsertViewModel, FieldUpdateViewModel, FieldViewModel>
    {
        public FieldService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async override Task<Field> GetAsync(Expression<Func<Field, bool>> expression)
        {
            return await Entity.Include(x => x.Degree).SingleOrDefaultAsync(expression);
        }

        public async override Task<FieldViewModel> GetAsync(int id)
        {
            return await Entity.Include(x => x.Degree).ProjectTo<FieldViewModel>().SingleOrDefaultAsync(x=>x.Id == id);
        }


    }
}
