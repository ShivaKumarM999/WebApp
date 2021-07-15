using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private POCEntities db = new POCEntities();
        public ActionResult Index()
        {
            string userName = Session["UserName"].ToString();
            if (userName != "")
            {
                var user = db.UserMasters.Where(x => x.UserName == userName).FirstOrDefault();
                user.Country = db.Countries.Where(u => u.CountryId == user.CountryId).Select(u => u.CountryName).FirstOrDefault();
                user.State = db.States.Where(u => u.CountryId == user.CountryId).Select(u => u.StateName).FirstOrDefault();
                return View(user);
            }
            else
                return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}