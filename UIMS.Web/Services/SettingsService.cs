using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace UIMS.Web.Services
{
    public class SettingsService : BaseService<Settings, SettingsInsertViewModel, SettingsUpdateViewModel, SettingsViewModel>
    {
        public SettingsService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }


        public async Task<string> GetValueAsync(string accessName)
        {
            var settings = await Entity.SingleOrDefaultAsync(x => x.AccessName == accessName);
            if (settings == null)
                return null;

            return settings.Value;
        }
    }
}
