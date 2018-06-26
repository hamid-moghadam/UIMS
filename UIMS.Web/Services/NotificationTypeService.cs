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
        public NotificationTypeService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<NotificationType> CreateIfNotExists(string type)
        {
            var notifType = await GetAsync(x => x.Type == type);
            if (notifType == null)
            {
                var result = await AddAsync(new NotificationType() { Type = type });
                await SaveChangesAsync();
            }
            return notifType;
        }

    }
}
