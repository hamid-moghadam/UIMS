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
    public class SettingsService : BaseService<Settings, SettingsInsertViewModel, SettingsUpdateViewModel, SettingsViewModel>
    {
        public SettingsService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
