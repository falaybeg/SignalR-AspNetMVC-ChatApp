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

            if (conUsers.Count(x => x.ConnectedId == id) == 0)
            {
                conUsers.Add(new UserDetail
                {
                    ConnectedId = id,
                    UserName = userName
                });

                Clients.All.updatecounter(conUsers.Count);
                Clients.All.totalMember(totalMember);

                Clients.All.totalMessages(TotalMyMessages(true));
                Clients.All.todayMessages(TodayMyMessages(true));

                Clients.Caller.totalMymessages(TotalMyMessages(false));
                Clients.Caller.todayMyMessages(TodayMyMessages(false));

                Clients.Caller.onConnected(id, userName, conUsers, currentMessages);
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = conUsers.FirstOrDefault(c => c.ConnectedId == Context.ConnectionId);
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
            Clients.All.todayMessages(TodayMyMessages(true));

            Clients.Caller.totalMymessages(TotalMyMessages(false));
            Clients.Caller.todayMyMessages(TodayMyMessages(false));

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
            DateTime today = DateTime.Now.Date;

            if (total == true)
            {
                messages = db.Message
                    .Where(b => (b.SendTime.Year == today.Year && b.SendTime.Month == today.Month && b.SendTime.Day == today.Day))
                    .Count();
            }
            else
            {
                messages = db.Message
                    .Where(u=> u.UserId == userId)
                    .Where(b => (b.SendTime.Year == today.Year && b.SendTime.Month == today.Month && b.SendTime.Day == today.Day))
                    .Count();
            }

            return messages;
        }


        private void SendPrivateMessage(string toUserId, string message)
        {
            string fromUserId = Context.ConnectionId;

            var toUser = conUsers.FirstOrDefault(x => x.ConnectedId == toUserId);
            var fromUser = conUsers.FirstOrDefault(x => x.ConnectedId == fromUserId);

            if (toUserId != null && fromUser != null)
            {
                Clients.Caller(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
            }
        }


        public void SendNotification(string message)
        {
            string username = Context.User.Identity.GetUserName();
            string userId = Context.User.Identity.GetUserId();

            AddNotification(userId, message);
            Clients.All.receiveNotification(username, message, DateTime.Now.ToString());
        }

        private void AddNotification(string id, string message)
        {
            db.Notification.Add(new Notification { UserId = id, NotificationMessage = message, SendTime = DateTime.Now });
            db.SaveChanges();
        }

        public void getTime()
        {
            Clients.Caller.setTime(DateTime.Now.ToString("h:mm:ss tt"));
        }
    }
}