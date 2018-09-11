using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;

namespace UIMS.Web.Services
{
    public class NotificationTypeService : BaseService<NotificationType, NotificationTypeInsertViewModel, NotificationTypeViewModel, NotificationTypeViewModel>
    {
        private readonly UserService _userService;
        private readonly NotificationAccessService _notificationAccessService;

        public NotificationTypeService(DataContext context, IMapper mapper, UserService userService, NotificationAccessService notificationAccessService) : base(context, mapper)
        {
            _userService = userService;
            _notificationAccessService = notificationAccessService;
        }

        public NotificationType CreateIfNotExists(string type)
        {
            var notifType = GetAsync(x => x.Type == type).Result;
            if (notifType == null)
            {
                var result = AddAsync(new NotificationType() { Type = type }).Result;
                SaveChanges();
                notifType = result;
            }
            return notifType;
        }

        public async Task<List<NotificationTypeViewModel>> GetAttachedNotificationTypesAsync(int userId)
        {
            var user = _userService.Get(x => x.Id == userId);
            var roles = await _userService.GetRolesAsync(user);
            var notifAccesses = await _notificationAccessService.GetAllByRolesAsync(roles);
            return notifAccesses.Select(x => x.NotificationType).OrderBy(x=>x.Priority).ToList();
        }


    }
}
