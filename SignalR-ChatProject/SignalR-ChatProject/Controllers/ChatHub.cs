using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
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


    [Microsoft.AspNet.SignalR.Authorize]
    public class ChatHub : Hub
    {
        static List<UserDetail> conUsers = new List<UserDetail>();
        static List<MessageDetail> currentMessages = new List<MessageDetail>();
        static ApplicationDbContext db = new ApplicationDbContext();


        public override Task OnConnected()
        {
            var id = Context.ConnectionId;
            var userName = Context.User.Identity.Name;
            int totalMember = db.Users.Count();
          

            if (userName == null)
                return null;

            if (conUsers.Count(x => x.ConnectionId == id) == 0)
            {
                conUsers.Add(new UserDetail
                {
                    ConnectionId = id,
                    UserName = userName
                });

                Clients.All.updatecounter(conUsers.Count);
                Clients.All.totalMember(totalMember);

                Clients.All.totalMessages(TotalMyMessages(true));
                Clients.All.totalMymessages(TotalMyMessages(false));

                //Clients.All.todayMessages(totalMessage);
                //Clients.All.todayMyMessages(totalMessage);
                //Clients.All.totalMember(totalMessage);

                Clients.Caller.onConnected(id, userName, conUsers, currentMessages);
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = conUsers.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                conUsers.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);
                Clients.All.updatecounter(conUsers.Count);

            }

            return base.OnDisconnected(stopCalled);
        }

        public void SendMessageToAll(string userName, string message)
        {
            AddMessageInCache(userName, message);
            Clients.All.messageReceived(userName, message);

            Clients.All.totalMessages(TotalMyMessages(true));
            Clients.All.totalMymessages(TotalMyMessages(false));

            Clients.All.todayMessages(TodayMyMessages(true));
            Clients.All.todayMyMessages(TodayMyMessages(false));


        }

        private void AddMessageInCache(string userName, string message)
        {
            var userId = Context.User.Identity.GetUserId();

            currentMessages.Add(new MessageDetail
            {
                UserName = userName,
                Message = message
            });

            db.Message.Add(new Messages { UserId = userId, Message = message, SendTime = DateTime.Now });
            db.SaveChanges();

            if (currentMessages.Count > 100)
                currentMessages.RemoveAt(0);
        }


        private int TotalMyMessages(bool total)
        {
            int messages = 0;
            string userId =  Context.User.Identity.GetUserId();

            if (total == true)
            {
                messages = db.Message.Count();
            }
            else
            {
                messages = db.Message.Where(x => x.UserId == userId).Count();
            }

            return messages;
        }

        private int TodayMyMessages(bool total)
        {
            int messages = 0;
            string userId = Context.User.Identity.GetUserId();
            DateTime today = DateTime.Today;

            if (total == true)
            {
                messages = db.Message
                    .Where(x=> DbFunctions.TruncateTime(x.SendTime.Date) == DateTime.Now.Date)
                    .Count();
            }
            else
            {
                messages = db.Message.Where(x => x.UserId == userId).Where(x=> x.SendTime == DateTime.Now.Date).Count();
            }

            return messages;
        }

    }
}