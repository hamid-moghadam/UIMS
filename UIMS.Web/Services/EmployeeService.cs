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
    public class EmployeeService : BaseService<Employee, EmployeeInsertViewModel, EmployeeUpdateViewModel, EmployeeViewModel>
    {

        public EmployeeService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }



    }
}
