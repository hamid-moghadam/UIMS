using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.Hubs
{
    public class BaseHub:Hub
    {
        protected int UserId => int.Parse(Context.UserIdentifier);
    }
}
