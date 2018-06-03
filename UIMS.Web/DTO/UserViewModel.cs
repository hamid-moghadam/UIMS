using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class UserViewModel:BaseModel
    {
        public string MelliCode { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        //public bool Enable { get; set; }

        //public ICollection<Message> SentMessages { get; set; }

        //public ICollection<MessageReceiver> ReceivedMessages { get; set; }


        public BuildingManagerViewModel BuildingManager { get; set; }


        public EmployeeViewModel Employee { get; set; }

        public GroupManagerViewModel GroupManager { get; set; }


        public ProfessorViewModel Professor { get; set; }


        public StudentViewModel Student { get; set; }

    }
}
