using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.DTO;
using UIMS.Web.Extentions;

namespace UIMS.Web.Models.AutoMapperConfigurations
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            ForAllMaps((x, y) => y.ForAllMembers(member => member.UseDestinationValue()));
            //ForAllMaps((x, y) => y.ForAllMembers(member => member.Condition(src => src != null)));

            CreateMap<AppUser, UserInsertViewModel>().ReverseMap();
            CreateMap<AppUser, EmployeeInsertViewModel>().ReverseMap();
            CreateMap<AppUser, BuildingManagerInsertViewModel>().ReverseMap();
            CreateMap<AppUser, GroupManagerInsertViewModel>().ReverseMap();
            CreateMap<AppUser, ProfessorInsertViewModel>().ReverseMap();
            CreateMap<AppUser, StudentInsertViewModel>().ReverseMap();

            CreateMap<AppUser, UserViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AppUser, UserLoginViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AppUser, UserPartialViewModel>();

            //CreateMap<AppUser, UserUpdateViewModel>()
            //    .ReverseMap()
            //    .ForMember(x => x.Id, source => source.Ignore());

            //CreateMap<EmployeeUpdateViewModel, AppUser>()
            //    .ForMember(x => x.Id, source => source.Ignore())
            //    //.AfterMap((em, ap) => ap.Employee.Post = em.EmployeePost)
            //    .ReverseMap();

            //CreateMap<AppUser, BuildingManagerUpdateViewModel>()
            //    .ReverseMap()
            //    .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<AppUser, EmployeeUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<AppUser,UserUpdateViewModel> ()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<BuildingManagerUpdateViewModel,AppUser>()
                .ForMember(x => x.Id, source => source.Ignore())
                .AfterMap((bu,ap) => ap.BuildingManager.BuildingId = bu.BuildingId.HasValue && bu.BuildingId.Value != 0?bu.BuildingId:ap.BuildingManager.BuildingId)
                .ReverseMap();
            CreateMap<AppUser, GroupManagerUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<AppUser, ProfessorUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<AppUser, StudentUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());

            //CreateMap<GroupManagerUpdateViewModel, AppUser>()
            //    .ForMember(x => x.Id, source => source.Ignore())
            //    .ReverseMap();
            //CreateMap<ProfessorUpdateViewModel, AppUser>()
            //    .ForMember(x => x.Id, source => source.Ignore())
            //    .ReverseMap();
            //CreateMap<StudentUpdateViewModel, AppUser>()
            //    .ForMember(x => x.Id, source => source.Ignore())
            //    .ReverseMap();




            CreateMap<Employee, EmployeeViewModel>();

            CreateMap<BuildingManagerUpdateViewModel, BuildingManager>()
                .ForMember(x => x.BuildingId, src => src.MapFrom(x => x.BuildingId));
            CreateMap<BuildingManager, BuildingManagerViewModel>();

            CreateMap<GroupManager, GroupManagerViewModel>();

            CreateMap<Professor, ProfessorViewModel>();

            CreateMap<Student, StudentViewModel>();

            CreateMap<Building, BuildingInsertViewModel>().ReverseMap();
            CreateMap<Building, BuildingViewModel>();
            CreateMap<BuildingUpdateViewModel,Building>().ReverseMap();

            CreateMap<Degree, DegreeViewModel>();
            CreateMap<Degree, DegreeInsertViewModel>().ReverseMap();
            CreateMap<DegreeUpdateViewModel,Degree>().ReverseMap();

            CreateMap<Field, FieldViewModel>();
            CreateMap<Field, FieldInsertViewModel>().ReverseMap();
            CreateMap<FieldUpdateViewModel,Field>().ReverseMap();

            CreateMap<Semester, SemesterViewModel>();
            CreateMap<Semester, SemesterInsertViewModel>().ReverseMap();
            CreateMap<SemesterUpdateViewModel,Semester>().ReverseMap();

            CreateMap<Course, CourseViewModel>();
            CreateMap<Course, CourseInsertViewModel>().ReverseMap();
            CreateMap<CourseUpdateViewModel,Course>().ReverseMap();


            CreateMap<BuildingClass, BuildingClassViewModel>();
            CreateMap<BuildingClass, BuildingClassInsertViewModel>().ReverseMap();
            CreateMap<BuildingClassUpdateViewModel, BuildingClass>().ReverseMap();

            CreateMap<CourseField, CourseFieldViewModel>();
            CreateMap<CourseField, CourseFieldInsertViewModel>().ReverseMap();
            CreateMap<CourseFieldUpdateViewModel, CourseField>().ReverseMap();

            CreateMap<Presentation, PresentationViewModel>()
                .ForMember(x => x.Day, source => source.MapFrom(x => (int)x.Day));
            CreateMap<Presentation, PresentationPartialViewModel>()
                .ForMember(x => x.Day, source => source.MapFrom(x => (int)x.Day));
            CreateMap<Presentation, PresentationBuildingManagerViewModel>()
                .ForMember(x => x.Day, source => source.MapFrom(x => (int)x.Day));
            CreateMap<Presentation, PresentationProfessorViewModel>()
                .ForMember(x => x.Day, source => source.MapFrom(x => (int)x.Day));
            CreateMap<Presentation, PresentationInsertViewModel>().ReverseMap();
            CreateMap<PresentationUpdateViewModel, Presentation>().ReverseMap();

            CreateMap<StudentPresentation, StudentPresentationViewModel>();
                //.ForMember(x => x.Enable, source => source.MapFrom(x => x.Enable))
                //.ForMember(x => x.BuildingClass, source => source.MapFrom(x => x.Presentation.BuildingClass))
                //.ForMember(x => x.Code, source => source.MapFrom(x => x.Presentation.Code))
                //.ForMember(x => x.CourseField, source => source.MapFrom(x => x.Presentation.CourseField))
                //.ForMember(x => x.Day, source => source.MapFrom(x => x.Presentation.Day))
                //.ForMember(x => x.End, source => source.MapFrom(x => x.Presentation.End))
                //.ForMember(x => x.Id, source => source.MapFrom(x => x.Presentation.Id))
                //.ForMember(x => x.Professor, source => source.MapFrom(x => x.Presentation.Professor))
                //.ForMember(x => x.Semester, source => source.MapFrom(x => x.Presentation.Semester))
                //.ForMember(x => x.Start, source => source.MapFrom(x => x.Presentation.Start));
            CreateMap<StudentPresentation, StudentPresentationPartialViewModel>();
            CreateMap<StudentPresentation, StudentPresentationInsertViewModel>().ReverseMap();
            CreateMap<StudentPresentationUpdateViewModel, StudentPresentation>().ReverseMap();

            CreateMap<NotificationType, NotificationTypeViewModel>();
            CreateMap<NotificationType, NotificationTypeInsertViewModel>().ReverseMap();
            CreateMap<NotificationTypeUpdateViewModel, NotificationType>().ReverseMap();


            CreateMap<Notification, NotificationInsertViewModel>().ReverseMap();
            CreateMap<Notification, NotificationViewModel>();
            //CreateMap<Message, MessageInsertViewModel>();


            CreateMap<Settings, SettingsViewModel>();
            CreateMap<Settings, SettingsInsertViewModel>().ReverseMap();
            CreateMap<SettingsUpdateViewModel, Settings>().ReverseMap();

        }

    }
}
