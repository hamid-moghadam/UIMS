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
    public class NotificationService : BaseService<Notification, NotificationInsertViewModel, NotificationUpdateViewModel, NotificationViewModel>
    {
        private readonly DbSet<NotificationReceiver> _messageReceiver;


        public NotificationService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _messageReceiver = context.Set<NotificationReceiver>();
        }

        public async Task<PaginationViewModel<NotificationViewModel>> GetAll(int typeId,string semester,int page, int pageSize,int userId)
        {
            return await _messageReceiver
                .Where(x => x.Notification.NotificationTypeId == typeId && x.Notification.Semester.Name == semester && x.UserId == userId)
                .OrderByDescending(x => x.Created)
                .ThenBy(x => !x.HasSeen)
                .Select(x => x.Notification).ProjectTo<NotificationViewModel>()
                .ToPageAsync(pageSize, page);
            //return await _messageReceiver
            //    .Where(x => x.Notification.Semester.Name == semester && x.UserId == userId)
            //    .OrderByDescending(x => x.Created)
            //    .ThenBy(x => !x.HasSeen)
            //    .Select(x => x.Notification).ProjectTo<NotificationViewModel>()
            //    .ToPageAsync(pageSize, page);

        }

        public async Task<int> GetMessagesCount(string semester,int userId)
        {
            return await _messageReceiver
                .Where(x => x.UserId == userId && x.Notification.Semester.Name == semester && !x.HasSeen)
                .CountAsync();
        }

        public async Task MarkMessagesAsSeenAsync(int userId)
        {
            var semester = await GetCurrentSemesterAsync();
            await _messageReceiver.Where(x => x.Notification.Semester.Name == semester &&  x.UserId== userId).ForEachAsync(x => x.HasSeen = true);
        }


    }
}
