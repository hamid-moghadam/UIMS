using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.Services;
using UIMS.Web.Data.AppConfigurations;
using Microsoft.Extensions.Options;
using UIMS.Web.Models;

namespace UIMS.Web.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private readonly UserService _userService;


        public ValuesController(UserService userService)
        {
            _userService = userService;
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            //options.
            //UserSecretConfiguration userSecretConfiguration = new UserSecretConfiguration();
            //userSecretConfiguration.AddUserSecret(new UserSecret() { University = "Rajaei" });

            return Ok(new AppUser() { PhoneNumber = "3546546546" });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var user = await _userService.GetAsync(id);
            return user.FullName;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
