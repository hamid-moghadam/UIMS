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
    public class PresentationService : BaseService<Presentation, PresentationInsertViewModel, PresentationUpdateViewModel, PresentationViewModel>
    {
        public PresentationService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override void Remove(Presentation model)
        {
            model.Enable = false;
        }
    }
}
