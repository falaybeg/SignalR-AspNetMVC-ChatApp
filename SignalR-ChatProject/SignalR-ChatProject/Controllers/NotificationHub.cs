using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using SignalR_ChatProject.Models;
using System.Threading.Tasks;

namespace SignalR_ChatProject.Controllers
{
    public class NotificationHub : Hub
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        static List<NotificationDetail> noti = new List<NotificationDetail>();

        public override Task OnConnected()
        {
            Clients.All.listNotifications(noti);
            return base.OnConnected();
        }

        public void SendNotification(string message)
        {
            string username = Context.User.Identity.GetUserName();
            string userId = Context.User.Identity.GetUserId();
            string time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            noti.Add(new NotificationDetail {
                Username = username,
                Message = message,
                Time = time
            });

          //  AddNotification(userId, message);
            Clients.All.receiveNotification(username,message,time);
        }

        private void AddNotification(string id, string message)
        {
            db.Notification.Add(new Notification { UserId = id, NotificationMessage = message, SendTime = DateTime.Now });
            db.SaveChanges();
        }
    }
}