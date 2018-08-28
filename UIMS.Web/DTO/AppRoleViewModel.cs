using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class AppRoleViewModel:BaseModel
    {
        public string Name { get; set; }

        public string PersianName { get; set; }

        public string NormalizedName { get; set; }
    }
}
