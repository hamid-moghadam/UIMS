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

            CreateMap<AppUser, UserInsertViewModel>().ReverseMap();
            CreateMap<AppUser, EmployeeInsertViewModel>().ReverseMap();
            CreateMap<AppUser, BuildingManagerInsertViewModel>().ReverseMap();
            CreateMap<AppUser, GroupManagerInsertViewModel>().ReverseMap();
            CreateMap<AppUser, ProfessorInsertViewModel>().ReverseMap();
            CreateMap<AppUser, StudentInsertViewModel>().ReverseMap();

            CreateMap<AppUser, UserViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AppUser, UserUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<AppUser, EmployeeUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<AppUser, BuildingManagerUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<AppUser, GroupManagerUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<AppUser, ProfessorUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());
            CreateMap<AppUser, StudentUpdateViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, source => source.Ignore());


            CreateMap<Building, BuildingInsertViewModel>().ReverseMap();
            CreateMap<Building, BuildingViewModel>();
            CreateMap<Building, BuildingUpdateViewModel>().ReverseMap();


            CreateMap<Employee, EmployeeViewModel>();

            CreateMap<BuildingManager, BuildingManagerViewModel>();

            CreateMap<GroupManager, GroupManagerViewModel>();

            CreateMap<Professor, ProfessorViewModel>();

            CreateMap<Student, StudentViewModel>();


            CreateMap<Degree, DegreeViewModel>();
            CreateMap<Degree, DegreeInsertViewModel>().ReverseMap();
            CreateMap<Degree, DegreeUpdateViewModel>().ReverseMap();

            CreateMap<Field, FieldViewModel>();
            CreateMap<Field, FieldInsertViewModel>().ReverseMap();
            CreateMap<Field, FieldUpdateViewModel>().ReverseMap();

            CreateMap<Semester, SemesterViewModel>();
            CreateMap<Semester, SemesterInsertViewModel>().ReverseMap();
            CreateMap<Semester, SemesterUpdateViewModel>().ReverseMap();

        }

    }
}
