using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using AutoMapper.QueryableExtensions;
using UIMS.Web.Extentions;
using Microsoft.EntityFrameworkCore;

namespace UIMS.Web.Services
{
    public class ChatService : BaseService<Chat, ChatInsertViewModel, ChatUpdateViewModel, ChatViewModel>
    {
        private readonly DbSet<ChatReply> _chatReplies;


        public ChatService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _chatReplies = context.Set<ChatReply>();
        }

        public async Task<PaginationViewModel<ChatViewModel>> GetAllAsync(int userId ,int page, int pageSize)
        {
            return await Entity.Where(x=>x.FirstUserId == userId || x.SecondUserId == userId).OrderByDescending(x=>x.Created).ProjectTo<ChatViewModel>().ToPageAsync(pageSize, page);
        }

        public async Task<PaginationViewModel<ChatReplyViewModel>> GetReplies(int chatId, int page, int pageSize)
        {
            return await _chatReplies.Where(x => x.ChatId == chatId).OrderByDescending(x => x.Created).ProjectTo<ChatReplyViewModel>().ToPageAsync(pageSize, page);
        }



        public async Task<Chat> AddIfNotExists(int firstId,int secondId)
        {
            var conversation = await GetAsync(x => (x.FirstUserId == firstId && x.SecondUserId == secondId) || (x.FirstUserId == secondId && x.SecondUserId == firstId));

            if (conversation == null)
            {
                var chat =  await AddAsync(new ChatInsertViewModel() { FirstId = firstId, SecondId = secondId });
                await SaveChangesAsync();
                return chat;
            }

            return conversation;
        }
    }
}
