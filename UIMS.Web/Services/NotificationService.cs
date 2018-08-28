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
            if (typeId == 0)
                return await _messageReceiver
                .Where(x => x.Notification.Semester.Name == semester && x.UserId == userId)
                .OrderByDescending(x => x.Created)
                .ThenBy(x => !x.HasSeen)
                .Select(x => x.Notification)
                .ProjectTo<NotificationViewModel>()
                .ToPageAsync(pageSize, page);

            return await _messageReceiver
                .Where(x => x.Notification.NotificationTypeId == typeId && x.Notification.Semester.Name == semester && x.UserId == userId)
                .OrderByDescending(x => x.Created)
                .ThenBy(x => !x.HasSeen)
                .Select(x => x.Notification)
                .ProjectTo<NotificationViewModel>()
                .ToPageAsync(pageSize, page);
        }

        public async Task<int> GetNotificationsCount(string semester,int userId)
        {
            return await _messageReceiver
                .Where(x => x.UserId == userId && x.Notification.Semester.Name == semester && !x.HasSeen)
                .CountAsync();
        }

        public async Task<PaginationViewModel<NotificationViewModel>> GetSentNotifications(int typeId, string semester, int page, int pageSize, int userId)
        {
            return await Entity
                .Where(x => x.Semester.Name == semester && x.SenderId == userId && x.NotificationTypeId == typeId)
                .OrderByDescending(x => x.Created)
                .ProjectTo<NotificationViewModel>()
                .ToPageAsync(pageSize, page);
        }

        public async Task<List<NotificationReceiverPartialViewModel>> GetReceivers(int notifId/*,int page,int pageSize*/)
        {
            return await _messageReceiver
                .Where(x => x.NotificationId == notifId)
                .OrderByDescending(x => x.Created)
                .ProjectTo<NotificationReceiverPartialViewModel>()
                .ToListAsync();
                //.ToPageAsync(pageSize, page);
        }


        public async Task MarkNotifAsSeenAsync(int id)
        {
            var notifReceiver = await _messageReceiver.FindAsync(id);
            if (notifReceiver != null)
                notifReceiver.HasSeen = true;
        }


    }
}
