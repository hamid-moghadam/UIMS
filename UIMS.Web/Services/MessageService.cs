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
    public class MessageService : BaseService<Message, MessageInsertViewModel, MessageUpdateViewModel, MessageViewModel>
    {
        private readonly DbSet<MessageReceiver> _messageReceiver;


        public MessageService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _messageReceiver = context.Set<MessageReceiver>();
        }

        public async Task<PaginationViewModel<MessageViewModel>> GetAll(string semester,int page, int pageSize,int userId)
        {
            return await _messageReceiver.Where(x=>x.Message.Semester.Name == semester && x.UserId == userId).OrderBy(x=>!x.HasSeen).Select(x=>x.Message).ProjectTo<MessageViewModel>().ToPageAsync(pageSize, page);
        }

        public async Task<int> GetMessagesCount(string semester,int userId)
        {
            return await _messageReceiver.Where(x => x.UserId == userId && x.Message.Semester.Name == semester && !x.HasSeen).CountAsync();
        }


    }
}
