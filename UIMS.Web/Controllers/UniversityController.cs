using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using UIMS.Web.Data.AppConfigurations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    public class UniversityController : ApiController
    {
        [HttpGet]
        public ActionResult<UniversityInfoViewModel> GetInfo()
        {
            var info = new UniversityInformationConfiguration().GetUniversityInfo();

            return new UniversityInfoViewModel() { Name = info.Name };
        }
        
    }
}
