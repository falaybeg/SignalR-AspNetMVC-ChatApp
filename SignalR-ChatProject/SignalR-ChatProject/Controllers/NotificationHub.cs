using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using SignalR_ChatProject.Models;

namespace SignalR_ChatProject.Controllers
{
    public class NotificationHub : Hub
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void SendNotification(string message)
        {
            string username = Context.User.Identity.GetUserName();
            string userId = Context.User.Identity.GetUserId();

          //  AddNotification(userId, message);
            Clients.All.receiveNotification(username, message);
        }

        private void AddNotification(string id, string message)
        {
            db.Notification.Add(new Notification { UserId = id, NotificationMessage = message, SendTime = DateTime.Now });
            db.SaveChanges();
        }

    }
}