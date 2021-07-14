using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class UserMastersController : Controller
    {
        private POCEntities db = new POCEntities();

        public ActionResult Index()
        {
            return View(db.UserMasters.ToList());
        }

        public ActionResult SignUp()
        {

            ViewBag.CountryList = new SelectList(GetCountryList(), "CountryId", "CountryName");
            return View();
        }

        public List<Country> GetCountryList()
        {
            List<Country> countries = db.Countries.ToList();
            return countries;
        }

        public ActionResult GetStateList(int Cid)
        {
            try
            {
                List<State> selectList = db.States.Where(x => x.CountryId == Cid).ToList();
                ViewBag.Slist = new SelectList(selectList, "SId", "StateName");
            }
            catch (Exception e)
            {
                string exception = e.Message;
            }
            return PartialView("DisplayStates");
        }

        public string Decrypt(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);

            }
            catch (FormatException fe)
            {
                decrypted = fe.Message;
            }
            return decrypted;
        }
        public string Enrypt(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        [HttpPost]
        public ActionResult SignUp(UserMaster userMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int UserExists = db.UserMasters.Where(u => u.UserName == userMaster.UserName).Count();
                    if (UserExists > 0)
                    {
                        ViewBag.ErrorMessage = "User already exists, Please enter new User Name";
                        ViewBag.CountryList = new SelectList(GetCountryList(), "CountryId", "CountryName");
                        return View(userMaster);
                    }
                    userMaster.Password = Enrypt(userMaster.Password);
                    db.UserMasters.Add(userMaster);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                string exception = e.Message;
            }
            ViewBag.CountryList = new SelectList(GetCountryList(), "CountryId", "CountryName");
            return View(userMaster);
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(UserMaster userMaster)
        {
            
            var validUser = db.UserMasters.Where(u => u.UserName == userMaster.UserName).FirstOrDefault();
            string pwd = Decrypt(validUser.Password);
            if (pwd==userMaster.Password)
            { 
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Please enter valid User Name and Password";
                return View(userMaster);
            }
        }
        public ActionResult Details(UserMaster userMaster)
        {
            if (userMaster.UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster userMaster1 = db.UserMasters.Where(u=>u.UserName==userMaster.UserName).FirstOrDefault();
            userMaster1.Password = Decrypt(userMaster1.Password);
            if (userMaster == null)
            {
                return HttpNotFound();
            }
            return View(userMaster1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
