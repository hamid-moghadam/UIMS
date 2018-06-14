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
    public class MessageTypeService : BaseService<MessageType, MessageTypeInsertViewModel, MessageTypeUpdateViewModel, MessageTypeViewModel>
    {
        public MessageTypeService(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
