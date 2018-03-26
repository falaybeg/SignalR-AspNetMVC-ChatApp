using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalR_ChatProject.Models;

namespace SignalR_ChatProject.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetHistory(string name = "", string dtfrom = "", string dtto = "")
        {
            IQueryable<Messages> messageslist;

            messageslist = db.Message;
            DateTime dtf;
            DateTime dtt;

            // Kodu duzenle ve Join ile yaparak query olustur. 

            if (DateTime.TryParse(dtfrom, out dtf))
            {
                messageslist = messageslist.Where(x => x.SendTime >= dtf);
            }
            if (DateTime.TryParse(dtto, out dtt))
            {
                messageslist = messageslist.Where(x => x.SendTime <= dtt);
            }

            if (!string.IsNullOrEmpty(name))
            {
                messageslist = messageslist.Where(x => x.UserId == name);
            }

            var messages = messageslist.ToList().Select(s => new
            {
                sendTime = s.SendTime.ToString("dd/MM/yyyy HH:mm:ss"),
                Text = s.Message,
                UserName = s.UserId
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