using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace UIMS.Web.Services
{
    public class NotificationAccessService : BaseService<NotificationAccess, NotificationAccessInsertModel, NotificationAccessUpdateViewModel, NotificationAccessViewModel>
    {
        public NotificationAccessService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<NotificationAccessViewModel>> GetAllByRoleAsync(string role)
        {
            return await Entity
                .Where(x => x.AppRole.Name == role)
                .ProjectTo<NotificationAccessViewModel>()
                .ToListAsync();
        }

        public async Task<List<NotificationAccessViewModel>> GetAllByRolesAsync(List<string> roles)
        {
            return await Entity
                .Where(x => roles.Contains(x.AppRole.Name))
                .ProjectTo<NotificationAccessViewModel>()
                .ToListAsync();
        }

        public async Task<bool> IsExistsAsync(NotificationAccessInsertModel notificationAccessInsertModel)
        {
            return await Entity.AnyAsync(x => x.AppRoleId == notificationAccessInsertModel.AppRoleId.Value && x.NotificationTypeId == notificationAccessInsertModel.NotificationTypeId.Value);
        }

    }
}
