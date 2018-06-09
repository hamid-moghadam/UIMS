using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace UIMS.Web.Services
{
    public class PresentationService : BaseService<Presentation, PresentationInsertViewModel, PresentationUpdateViewModel, PresentationViewModel>
    {
        public PresentationService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override void Remove(Presentation model)
        {
            model.Enable = false;
        }

        public async Task<List<PresentationProfessorViewModel>> GetAllByProfessorId(int id,string semester)
        {
            return await Entity.Where(x => x.ProfessorId == id && x.Semester.Name == semester).ProjectTo<PresentationProfessorViewModel>().ToListAsync();
        }

        public async Task<List<PresentationBuildingManagerViewModel>> GetAllByBuildingId(int id, string semester)
        {
            return await Entity.Where(x => x.BuildingClass.BuildingId == id && x.Semester.Name == semester).ProjectTo<PresentationBuildingManagerViewModel>().ToListAsync();
        }

    }
}
