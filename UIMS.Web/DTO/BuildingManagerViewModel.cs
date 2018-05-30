using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class BuildingManagerViewModel:BaseModel
    {
        public string UserMelliCode { get; set; }

        public string UserFullName { get; set; }
    }
}
