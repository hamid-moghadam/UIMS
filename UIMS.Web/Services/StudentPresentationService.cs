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
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace UIMS.Web.Services
{
    public class StudentPresentationService : BaseService<StudentPresentation, StudentPresentationInsertViewModel, StudentPresentationUpdateViewModel, StudentPresentationViewModel>
    {
        public StudentPresentationService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }


        public async Task<List<PresentationPartialViewModel>> GetAll(int studentId,string semester)
        {
            //return await Entity.Where(x => x.StudentId == studentId && x.Presentation.Semester.Name == semester).Select(x=>x.Presentation).ProjectTo<StudentPresentationViewModel>().ToListAsync();
            return await Entity.Where(x => x.StudentId == studentId && x.Presentation.Semester.Name == semester).Select(x=>x.Presentation).ProjectTo<PresentationPartialViewModel>().ToListAsync();
        }

        public List<StudentPresentationInsertViewModel> GetAllByExcel(IFormFile file)
        {
            List<StudentPresentationInsertViewModel> studentPresentations = new List<StudentPresentationInsertViewModel>(5);
            var rows = new ExcelExtentions().GetRows(file);

            foreach (var row in rows)
            {
                if (row.Cells.Any(d => d.CellType == CellType.Blank) || row.Cells.Count != 2) continue;

                string studentCode = row.GetCell(0).ToString();
                string presentationCode = row.GetCell(1).ToString();

                if (string.IsNullOrEmpty(studentCode) || string.IsNullOrEmpty(presentationCode))
                    continue;

                if (!studentCode.IsNumber() || !presentationCode.IsNumber())
                    continue;

                studentPresentations.Add(new StudentPresentationInsertViewModel()
                {
                    StudentCode = studentCode,
                    PresentationCode = presentationCode
                });
            }
            return studentPresentations;
        }
    }
}
