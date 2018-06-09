using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Data;
using UIMS.Web.Extentions;
using UIMS.Web.Models;

namespace UIMS.Web.Services
{
    public class BaseServiceProvider<TModel> : IBaseServiceProvider<TModel> where TModel : class, IKey<int>
    {
        protected readonly DataContext _context;
        protected readonly IMapper _mapper;
        protected DbSet<TModel> Entity { get; set; }
        protected IQueryable<TModel> BaseQuery { get; set; }


        public BaseServiceProvider(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            Entity = context.Set<TModel>();
            BaseQuery = Entity.AsQueryable();
        }

        public virtual void Remove(TModel model) => Entity.Remove(model);


        public async virtual Task<TModel> AddAsync(TModel model)
        {
            //TModel baseModel = _mapper.Map<TModel>(model);
            return (await Entity.AddAsync(model)).Entity;

        }

        public async Task<string> ParseSemester(string semester)
        {
            if (semester != null && semester.IsSemester())
                return semester;
            else
                return (await _context.Set<Semester>().SingleOrDefaultAsync(x => x.Enable)).Name;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
