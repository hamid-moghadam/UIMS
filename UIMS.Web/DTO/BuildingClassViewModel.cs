using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class BuildingClassViewModel:BaseModel
    {
        public string Name { get; set; }

        public BuildingViewModel Building { get; set; }
    }
}
