using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Services;

namespace UIMS.Web.Hubs
{
    public class NotificationHub:Hub
    {
        SemesterService _SemesterService;
        public NotificationHub(SemesterService semesterService)
        {
            _SemesterService = semesterService;
        }
        public async Task SendMessage(string user, string message)
        {
            //Clients.All.
            var current = (await _SemesterService.GetCurrentAsycn()).Name;
            var t = await _SemesterService.GetAll(1,100);
            var w = t.Items.Select(x => x.Id.ToString()).ToList().AsReadOnly();
            
            //Clients.Users(w).(;
            await Clients.All.SendAsync("ReceiveMessage", user, $"{message} - {current}");
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

    }
}
