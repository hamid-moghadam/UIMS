using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class MessageTypeController : ApiController
    {

        private readonly MessageTypeService _messageTypeService;

        public MessageTypeController(MessageTypeService messageTypeService)
        {
            _messageTypeService = messageTypeService;
        }
        // GET: api/values
        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            await _messageTypeService.AddAsync(new DTO.MessageTypeInsertViewModel() { Name = name });

            await _messageTypeService.SaveChangesAsync();
            return Ok();


        }
    }
}
