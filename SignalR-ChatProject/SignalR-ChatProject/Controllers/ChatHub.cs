using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using SignalR_ChatProject.Models;

namespace SignalR_ChatProject.Controllers
{

    public class UserDetail
    {
        public string ConnectionId { get; set; }    
        public string UserName { get; set; }
    }


    [Authorize]
    public class ChatHub : Hub
    {
       

        private static int counter = 0;
        static List<UserDetail> connectedUser = new List<UserDetail>();
        static ApplicationDbContext db = new ApplicationDbContext();

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            if (connectedUser.Count(x => x.ConnectionId == connectionId) == 0)
            {
                counter = counter + 1;
                Clients.All.updatecounter(counter);

                connectedUser.Add(new UserDetail { ConnectionId = connectionId,UserName = name });

                Clients.Caller.onConnected(connectionId, name, connectedUser);
                Clients.AllExcept(connectionId).onNewUserConnected(connectionId, name);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = connectedUser.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                counter = counter - 1;
                Clients.All.updatecounter(counter);

                connectedUser.Remove(item);
                var id = Context.ConnectionId;

                Clients.Caller.onUserDisconnected(id, item.UserName);
            }

            return base.OnDisconnected(stopCalled);
        }

        public void SendMessages(string message)
        {
            // here we saved our messages in Database 
            db.Message.Add(new Messages
            {
                UserId = Context.User.Identity.GetUserId(),
                Message = message,
                SendTime = DateTime.Now
            });
            db.SaveChangesAsync();

            Clients.Caller.message("You: "+message);
            Clients.Others.message(Context.User.Identity.Name+" : "+ message);

            // This is second way of receiving messages 
            // var name = Context.User.Identity.Name;
            // Clients.Caller.message(name, message);
        }

   
    }
}