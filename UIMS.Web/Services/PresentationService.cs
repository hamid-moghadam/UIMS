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
using UIMS.Web.Extentions;

namespace UIMS.Web.Services
{
    public class PresentationService : BaseService<Presentation, PresentationInsertViewModel, PresentationUpdateViewModel, PresentationViewModel>
    {
        private readonly DbSet<StudentPresentation> _studentPresentation;
        private readonly NotificationService _notificationService;

        public PresentationService(DataContext context, IMapper mapper, NotificationService notificationService) : base(context, mapper)
        {
            _studentPresentation = context.Set<StudentPresentation>();
            _notificationService = notificationService;
            SearchQuery = (text) => Entity.Where(x => x.Code.Contains(text) || x.BuildingClass.Name.Contains(text) || x.Professor.User.FullName.Contains(text) || x.CourseField.Course.Name.Contains(text) || x.CourseField.Field.Name.Contains(text));
        }

        public override void Remove(Presentation model)
        {
            model.Enable = false;
        }

        public async Task<List<PresentationProfessorViewModel>> GetAllByProfessorId(int id,string semester)
        {
            return await Entity.Where(x => x.ProfessorId == id && x.Semester.Name == semester).ProjectTo<PresentationProfessorViewModel>().ToListAsync();
        }

        public async Task<PresentationDashboardDataViewModel> GetDashboardInfo(string semester)
        {
            var lastWeekSuspendedPresentations = await _notificationService.GetLastWeekSuspendedPresentationCount(semester);
            var todaySuspendedPresentations = await _notificationService.GetTodaySuspendedPresentations(semester);
            var todayPresentations = await Entity.CountAsync(x => (int)x.Day == DateTime.Now.Day && x.Enable && x.Semester.Name == semester);


            return new PresentationDashboardDataViewModel()
            {
                TodayPresentations = todayPresentations,
                LastWeekSuspendPresentations = lastWeekSuspendedPresentations,
                TodaySuspendedPresentations = todaySuspendedPresentations
            };
        }

        public async Task<PaginationViewModel<PresentationViewModel>> GetFieldPresentations(string semester,List<int> fieldIds,int page,int pageSize)
        {
            return await 
                Entity
                .Where(x => fieldIds.Contains(x.CourseField.FieldId) && x.Semester.Name == semester)
                .ProjectTo<PresentationViewModel>()
                .ToPageAsync(pageSize,page);
        }

        public async Task<PaginationViewModel<PresentationViewModel>> GetAllByRoleAsync(int page, int pageSize,int professorId)
        {
            return await Entity
                .Where(x => x.ProfessorId == professorId)
                .OrderByDescending(x => x.Created)
                .ProjectTo<PresentationViewModel>().ToPageAsync(pageSize, page);
        }

        public async Task<List<PresentationBuildingManagerViewModel>> GetAllByBuildingId(int id, string semester)
        {
            return await Entity.Where(x => x.BuildingClass.BuildingId == id && x.Semester.Name == semester).ProjectTo<PresentationBuildingManagerViewModel>().ToListAsync();
        }

        public async Task<PaginationViewModel<PresentationViewModel>> SearchAsync(string text, int page, int pageSize)
        {
            return await Entity.Where(x => x.BuildingClass.Name.Contains(text) || x.Professor.User.FullName.Contains(text) || x.CourseField.Course.Name.Contains(text) || x.CourseField.Field.Name.Contains(text)).ProjectTo<PresentationViewModel>().ToPageAsync(pageSize, page);
        }

        public async Task<PaginationViewModel<StudentViewModel>> GetStudents(int id,int page,int pageSize)
        {
            return await _studentPresentation.Where(x => x.PresentationId == id).Select(x => x.Student).ProjectTo<StudentViewModel>().ToPageAsync(pageSize,page);
        }

        public async Task<List<int>> GetStudentsAsync(int id)
        {
            return await _studentPresentation.Where(x => x.PresentationId == id && x.Enable).Select(x => x.Student.UserId).ToListAsync();
        }
    }
}
