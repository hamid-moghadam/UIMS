using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class UserLoginViewModel
    {
        public string FullName { get; set; }

        public DateTime LastLogin { get; set; }

        public BuildingManagerViewModel BuildingManager { get; set; }


        public EmployeeViewModel Employee { get; set; }

        public GroupManagerViewModel GroupManager { get; set; }


        public ProfessorViewModel Professor { get; set; }


        public StudentViewModel Student { get; set; }
    }
}
