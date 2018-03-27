using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalR_ChatProject.Models;

namespace SignalR_ChatProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetHistory(string name = "", string dtfrom = "", string dtto = "")
        {
            var messageslist = (from m in db.Message
                           join u in db.Users on m.UserId equals u.Id
                           select new
                           {
                               SendTime = m.SendTime,
                               Message = m.Message,
                               Username = u.UserName
                           });

            DateTime dtf;
            DateTime dtt;

            if (DateTime.TryParse(dtfrom, out dtf))
                messageslist = messageslist.Where(x => x.SendTime >= dtf); 

            if (DateTime.TryParse(dtto, out dtt))
                messageslist = messageslist.Where(x => x.SendTime <= dtt); 

            if (!string.IsNullOrEmpty(name))
                messageslist = messageslist.Where(x => x.Username == name);

            var messages = messageslist.ToList().Select(s => new
            {
                sendTime = s.SendTime.ToString("dd/MM/yyyy HH:mm:ss"),
                Text = s.Message,
                UserName = s.Username
            });
             
            return Json(messages, JsonRequestBehavior.AllowGet);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}