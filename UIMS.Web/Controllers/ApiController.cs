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

        //protected ObjectResult EnsureModelStateValidation()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
    }
}