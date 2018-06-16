using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.Services;
using UIMS.Web.DTO;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMS.Web.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class ChatController : ApiController
    {
        private readonly ChatService _chatService;


        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        // GET: api/values
        [HttpGet]
        public async Task<ActionResult<PaginationViewModel<ChatViewModel>>> GetAll(int pageSize = 5, int page = 1)
        {
            return await _chatService.GetAllAsync(UserId, page, pageSize);
        }


        [HttpPost]
        public async Task<ActionResult<PaginationViewModel<ChatReplyViewModel>>> GetReplies([FromBody] ChatGetRepliesViewModel chatGetRepliesVM)
        {
            var chat = await _chatService.GetAsync(x => x.Id == chatGetRepliesVM.ChatId.Value);
            if (chat == null)
                return NotFound();

            return await _chatService.GetReplies(chatGetRepliesVM.ChatId.Value, chatGetRepliesVM.Page, chatGetRepliesVM.PageSize);
        }
    }
}
