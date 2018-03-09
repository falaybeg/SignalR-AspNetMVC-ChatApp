using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalR_ChatProject.Controllers
{
    public class ChatHub : Hub
    {
        public override Task OnConnected()
        {
            Clients.All.users(Context.User.Identity.Name);
            return base.OnConnected();
        }


        public void SenMessage(string message)
        {
            Clients.Caller.messages("You: " + message);
            Clients.Others.messages(Context.User.Identity.Name+" :" + message);

        }
    }
}