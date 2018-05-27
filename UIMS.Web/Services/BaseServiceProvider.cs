using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public BaseServiceProvider(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            Entity = context.Set<TModel>();
        }

        public void Remove(TModel model) => Entity.Remove(model);


        public async Task<TModel> AddAsync(TModel model)
        {
            //TModel baseModel = _mapper.Map<TModel>(model);
            return (await Entity.AddAsync(model)).Entity;

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
