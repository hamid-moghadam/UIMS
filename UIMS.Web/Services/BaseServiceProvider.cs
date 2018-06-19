using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UIMS.Web.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UIMS.Web.Data;
using UIMS.Web.Models;

namespace UIMS.Web.Services
{
    public class BaseServiceProvider<TModel> : IBaseServiceProvider<TModel> where TModel : class, IKey<int>
    {
        protected readonly DataContext _context;
        protected readonly IMapper _mapper;
        protected DbSet<TModel> Entity { get; set; }
        protected IQueryable<TModel> BaseQuery { get; set; }
        protected Dictionary<string, Expression<Func<TModel, bool>>> Filters { get; set; }
        protected Expression<Func<TModel, bool>> this[string index]
        {
            get
            {
                return GetFilters(index);
            }
        }



        public BaseServiceProvider(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            Entity = context.Set<TModel>();
            BaseQuery = Entity.AsQueryable();
            Filters = new Dictionary<string, Expression<Func<TModel, bool>>>();
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

        //public Expression<Func<TModel, bool>> GetFilters(string[] filters)
        //{
        //    Expression<Func<TModel, bool>> finalExpression = null;


        //    foreach (var filter in filters)
        //    {
        //        var ex = Filters[filter];
        //        if (finalExpression == null)
        //            finalExpression = ex;
        //        else
        //            finalExpression.AndAlso(ex);
        //    }
        //    if (finalExpression == null)
        //        return (e) => true;
        //    return finalExpression;
        //}

        public Expression<Func<TModel, bool>> GetFilters(params string[] filters)
        {
            Expression<Func<TModel, bool>> finalExpression = null;
            if (filters == null || filters.Count() == 0)
                return (e) => true;

            foreach (var filter in filters)
            {
                var result = Filters.TryGetValue(filter,out Expression<Func<TModel, bool>> ex);
                if (!result)
                    continue;
                if (finalExpression == null)
                    finalExpression = ex;
                else
                    finalExpression.AndAlso(ex);
            }
            if (finalExpression == null)
                return (e) => true;
            return finalExpression;
        }


    }
}
