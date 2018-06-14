using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;
using UIMS.Web.Services;
using UIMS.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class MessageController : ApiController
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        // GET: api/values
        [SwaggerResponse(200, typeof(PaginationViewModel<MessageViewModel>))]
        [HttpPost]
        public async Task<IActionResult> GetAll(MessageGetAllViewModel messageGetAllVM)
        {
            return Ok(await _messageService.GetAll(messageGetAllVM.Semester,messageGetAllVM.Page,messageGetAllVM.PageSize,UserId));
        }

        // GET api/values/5
        [HttpGet("{semester}")]
        public async Task<IActionResult> GetBadge(string semester)
        {
            return Ok(await _messageService.GetMessagesCount(semester, UserId));
        }


        [HttpGet]
        public async Task<IActionResult> Test()
        {
            await _messageService.AddAsync(new Message()
            {
                Content = "sadsa",
                MessageTypeId = 1,
                Title = "sdsdf",
                SemesterId = 1,
                SenderId = 1,
                Receivers = new List<MessageReceiver>() { new MessageReceiver() {UserId =1 }, new MessageReceiver() { UserId = 3 }, new MessageReceiver() { UserId = 2 } }
            });
            await _messageService.SaveChangesAsync();
            return Ok();
        }



    }
}
