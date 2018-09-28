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
        private readonly DbSet<NotificationType> _notificationType;


        public NotificationService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _messageReceiver = context.Set<NotificationReceiver>();
            _notificationType = context.Set<NotificationType>();
        }

        public async Task<PaginationViewModel<NotificationViewModel>> GetAll(int typeId,string semester,int page, int pageSize,int userId,string notificationTypeName)
        {
            IQueryable<NotificationReceiver> query = _messageReceiver
                .Where(x => x.Notification.NotificationTypeId == typeId && x.Notification.Semester.Name == semester && x.UserId == userId)
                .Include(x=>x.Notification)
                .Include(x=>x.Notification.Semester)
                .Include(x=>x.Notification.Sender);

            if (notificationTypeName != null && notificationTypeName != "")
            {
                var notifType = await _notificationType.SingleOrDefaultAsync(x => x.Type == notificationTypeName);

                if (notifType != null)
                {
                    query = _messageReceiver.Where(x => x.Notification.Semester.Name == semester && x.UserId == userId && x.Notification.NotificationTypeId == notifType.Id);
                }

            }


            if (typeId == 0)
            {
                query = _messageReceiver.Where(x => x.Notification.Semester.Name == semester && x.UserId == userId);
            }

            return await query
                .OrderByDescending(x => x.Created)
                .ThenBy(x => !x.HasSeen)
                .Select(x => new NotificationViewModel()
                {
                    HasSeen = x.HasSeen,
                    SenderId = x.Notification.SenderId,
                    Sender = _mapper.Map<UserPartialViewModel>(x.Notification.Sender),
                    Content = x.Notification.Content,
                    Created = x.Created,
                    Subtitle = x.Notification.Subtitle,
                    Title = x.Notification.Title,
                    Enable = x.Notification.Enable,
                    Id = x.NotificationId,
                    SemesterId = x.Notification.SemesterId,
                    Semester = _mapper.Map<SemesterViewModel>(x.Notification.Semester)
                })
                //.ProjectTo<NotificationViewModel>()
                .ToPageAsync(pageSize, page);
        }

        public async Task<int> GetNotificationsCount(string semester,int userId)
        {
            return await _messageReceiver
                .Where(x => x.UserId == userId && x.Notification.Semester.Name == semester && !x.HasSeen)
                .CountAsync();
        }

        public async Task<int> GetLastWeekSuspendedPresentationCount(string semester)
        {
            return await Entity.CountAsync(x => x.NotificationType.Type == "لغو کلاس ها" && x.Semester.Name == semester && x.Created >= DateTime.Now.AddDays(-7));
            //return await _messageReceiver
            //    .Where(x => x.UserId == userId && x.Notification.Semester.Name == semester && !x.HasSeen)
            //    .CountAsync();
        }

        public async Task<int> GetTodaySuspendedPresentations(string semester)
        {
            return await Entity.CountAsync(x => x.NotificationType.Type == "لغو کلاس ها" && x.Semester.Name == semester && x.Created.IsToday());
            //return await _messageReceiver
            //    .Where(x => x.UserId == userId && x.Notification.Semester.Name == semester && !x.HasSeen)
            //    .CountAsync();
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


        public async Task MarkNotifAsSeenAsync(int id,int userId)
        {
            var notification = await Entity.FindAsync(id);
            if (notification == null)
                return;

            var notifReceiver = await _messageReceiver.SingleOrDefaultAsync(x => x.NotificationId == notification.Id && x.UserId == userId);
            if (notifReceiver != null)
                notifReceiver.HasSeen = true;

        }


    }
}
