using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class UserPartialViewModel:BaseModel
    {
        public string MelliCode { get; set; }

        public DateTime? LastLogin { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
