using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AutoMapper;
using UIMS.Web.DTO;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public abstract class ApiController : Controller
    {
        protected int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);


        protected List<string> Roles => User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value.Split(',').ToList();
        //protected List<string> Roles => User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //protected ObjectResult EnsureModelStateValidation()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
    }
}