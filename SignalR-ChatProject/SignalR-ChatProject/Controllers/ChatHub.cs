using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalR_ChatProject.Controllers
{
    [Authorize]
    public class ChatHub : Hub
    {
        public override Task OnConnected()
        {
            Clients.All.users(Context.User.Identity.Name);
            return base.OnConnected();
        }


        public void SendMessages(string message)
        {
            Clients.Caller.message("You: " + message);
            Clients.Others.message(Context.User.Identity.Name+" :" + message);
        }
    }
}