using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.DTO;

namespace UIMS.Web.Controllers
{
    public interface IController
    {
        [HttpGet]
        Task<IActionResult> GetAll(int pageSize = 5, int page = 1);

        [HttpGet("{id}")]
        Task<IActionResult> Get(int id);

        [HttpPost]
        Task<IActionResult> Add([FromBody] BaseInsertViewModel baseInsertVM);




        [HttpPost("{id}")]
        Task<IActionResult> Remove(int id);


        [HttpPost]
        Task<IActionResult> Update([FromBody] BaseUpdateViewModel employeeUpdateVM);

    }
}
