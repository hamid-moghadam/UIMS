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
            

            CreateMap<AppUser, UserViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AppUser, UserInsertViewModel>().ReverseMap();
            CreateMap<AppUser, UserUpdateViewModel>().ReverseMap();


            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<Employee, EmployeeInsertViewModel>().ReverseMap();
            CreateMap<Employee, EmployeeUpdateViewModel>().ReverseMap();


        }

    }
}
