using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.DTO;

namespace UIMS.Web.Models.AutoMapperConfigurations
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            ForAllMaps((x, y) => y.ForAllMembers(member => member.UseDestinationValue()));
            ForAllMaps((x, y) => y.ForAllMembers(member => member.Condition(src => src != null)));

            CreateMap<AppUser, UserInsertViewModel>().ReverseMap();
            CreateMap<AppUser, EmployeeInsertViewModel>().ReverseMap();
            CreateMap<AppUser, BuildingManagerInsertViewModel>().ReverseMap();
            CreateMap<AppUser, GroupManagerInsertViewModel>().ReverseMap();
            CreateMap<AppUser, ProfessorInsertViewModel>().ReverseMap();
            CreateMap<AppUser, StudentInsertViewModel>().ReverseMap();

            CreateMap<AppUser, UserViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            //CreateMap<AppUser, UserUpdateViewModel>()
            //    .ReverseMap()
            //    .ForMember(x => x.Id, source => source.Ignore());
            //CreateMap<AppUser, EmployeeUpdateViewModel>()
            //    .ReverseMap()
            //    .ForMember(x => x.Id, source => source.Ignore());

            CreateMap<UserUpdateViewModel, AppUser>()
                .ForMember(x => x.Id, source => source.Ignore())
                .ReverseMap();
            CreateMap<EmployeeUpdateViewModel, AppUser>()
                .ForMember(x => x.Id, source => source.Ignore())
                .ReverseMap();


            CreateMap<BuildingManagerUpdateViewModel,AppUser>()
                .ForMember(x => x.Id, source => source.Ignore())
                .ReverseMap();
            //CreateMap<AppUser, BuildingManagerUpdateViewModel>()
            //    .ReverseMap()
            //    .ForMember(x => x.Id, source => source.Ignore());
            //CreateMap<AppUser, GroupManagerUpdateViewModel>()
            //    .ReverseMap()
            //    .ForMember(x => x.Id, source => source.Ignore());
            //CreateMap<AppUser, ProfessorUpdateViewModel>()
            //    .ReverseMap()
            //    .ForMember(x => x.Id, source => source.Ignore());
            //CreateMap<AppUser, StudentUpdateViewModel>()
            //    .ReverseMap()
            //    .ForMember(x => x.Id, source => source.Ignore());

            CreateMap<GroupManagerUpdateViewModel, AppUser>()
                .ForMember(x => x.Id, source => source.Ignore())
                .ReverseMap();
            CreateMap<ProfessorUpdateViewModel, AppUser>()
                .ForMember(x => x.Id, source => source.Ignore())
                .ReverseMap();
            CreateMap<StudentUpdateViewModel, AppUser>()
                .ForMember(x => x.Id, source => source.Ignore())
                .ReverseMap();




            CreateMap<Employee, EmployeeViewModel>();

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
        }

    }
}
