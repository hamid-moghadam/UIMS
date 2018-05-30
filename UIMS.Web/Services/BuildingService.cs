using AutoMapper;
using UIMS.Web.Data;
using UIMS.Web.DTO;
using UIMS.Web.Models;

namespace UIMS.Web.Services
{
    public class BuildingService : BaseService<Building, BuildingInsertViewModel, BuildingUpdateViewModel, BuildingViewModel>
    {
        public BuildingService(DataContext context, IMapper mapper, UserService userService) : base(context, mapper)
        {
        }

        



    }
}
