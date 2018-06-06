using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;

namespace UIMS.Web.Services
{
    public class CourseFieldService : BaseService<CourseField, CourseFieldInsertViewModel, CourseFieldUpdateViewModel, CourseFieldViewModel>
    {


        //public CourseFieldService(this)[int indes]

        public IQueryable<CourseField> this[string index]
        {
            get
            {
                return dictionary[index];
            }
        }
        public Dictionary<string, IQueryable<CourseField>> dictionary;


        public CourseFieldService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            
            int[] a = new int[2];

            dictionary = new Dictionary<string, IQueryable<CourseField>>
            {
                { "GetAll", Entity.Where(x => x.CourseId == 1) }
            };
            //Entity.AsQueryable
        }
    }
}
