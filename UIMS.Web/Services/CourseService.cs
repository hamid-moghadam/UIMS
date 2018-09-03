using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using Microsoft.AspNetCore.Http;
using UIMS.Web.Extentions;
using NPOI.SS.UserModel;
using AutoMapper.QueryableExtensions;

namespace UIMS.Web.Services
{
    public class CourseService : BaseService<Course, CourseInsertViewModel, CourseUpdateViewModel, CourseViewModel>
    {
        public CourseService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            SearchQuery = (st) => Entity.Where(x => x.Name.Contains(st) || x.Code.Contains(st));
        }

        public List<CourseInsertViewModel> GetAllByExcel(IFormFile file)
        {
            List<CourseInsertViewModel> courses = new List<CourseInsertViewModel>(5);
            var rows = new ExcelExtentions().GetRows(file);

            foreach (var row in rows)
            {
                if (row.Cells.Any(d => d.CellType == CellType.Blank) || row.Cells.Count != 2) continue;

                string name = row.GetCell(0).ToString();
                string courseCode = row.GetCell(1).ToString();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(courseCode))
                    continue;

                if (!courseCode.IsNumber())
                    continue;

                courses.Add(new CourseInsertViewModel()
                {
                    Code = courseCode,
                    Name = name
                });
            }
            return courses;
        }

        public async Task<PaginationViewModel<CourseViewModel>> SearchAsync(string text, int page, int pageSize)
        {
            return await Entity.Where(x => x.Name.Contains(text) || x.Code.Contains(text)).ProjectTo<CourseViewModel>().ToPageAsync(pageSize, page);
        }
    }

}
