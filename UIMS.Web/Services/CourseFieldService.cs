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

namespace UIMS.Web.Services
{
    public class CourseFieldService : BaseService<CourseField, CourseFieldInsertViewModel, CourseFieldUpdateViewModel, CourseFieldViewModel>
    {

        private readonly DbSet<Presentation> _presentation;

        //public CourseFieldService(this)[int indes]

        //public IQueryable<CourseField> this[string index]
        //{
        //    get
        //    {
        //        return dictionary[index];
        //    }
        //}
        public Dictionary<string, IQueryable<CourseField>> dictionary;


        public CourseFieldService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _presentation = context.Set<Presentation>();
            
            int[] a = new int[2];

            dictionary = new Dictionary<string, IQueryable<CourseField>>
            {
                { "GetAll", Entity.Where(x => x.CourseId == 1) }
            };
            //Entity.AsQueryable
        }


        public async Task<List<CourseFieldViewModel>> GetAllByGroupManagerId(int id)
        {
            return await Entity.Where(x => x.Field.GroupManagerId == id).ProjectTo<CourseFieldViewModel>().ToListAsync();
        }

        public override void Remove(CourseField model)
        {
            model.Enable = false;
        }

        public override CourseField Update(CourseField model)
        {
            if (!model.Enable)
                _presentation.Where(x => x.CourseFieldId == model.Id).ToList().ForEach(x => x.Enable = false);
            //else
            //    _presentation.Where(x => x.CourseFieldId == model.Id).ToList().ForEach(x => x.Enable = false);

            SaveChanges();            
            return base.Update(model);
        }
    }
}
