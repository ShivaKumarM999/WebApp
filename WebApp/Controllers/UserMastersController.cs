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
            string exception;
            try
            {
                ViewBag.CountryList = new SelectList(GetCountryList(), "CountryId", "CountryName");
            }
            catch (Exception e)
            {
                exception = e.Message;
            }
            return View();
        }

        public List<Country> GetCountryList()
        {
            List<Country> countries = null;
            string exception;
            try
            {
                countries = db.Countries.ToList();
            }
            catch (Exception e)
            {
                exception = e.Message;
            }
            return countries;
        }

        public string GetPhoneCode(int Cid)
        {
            Country countries = null;
            string exception;
            try
            {
                countries = db.Countries.Where(u => u.CountryId == Cid).FirstOrDefault();
            }
            catch (Exception e)
            {
                exception = e.Message;
            }
            return countries.CountryCode;
        }
        public ActionResult GetStateList(int Cid)
        {
            string exception;
            try
            {
                List<State> selectList = db.States.Where(x => x.CountryId == Cid).ToList();
                ViewBag.Slist = new SelectList(selectList, "SId", "StateName");
            }
            catch (Exception e)
            {
                exception = e.Message;
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
            string exception;
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
                    return RedirectToAction("SignIn");
                }
            }
            catch (Exception e)
            {
                exception = e.Message;
            }
            ViewBag.CountryList = new SelectList(GetCountryList(), "CountryId", "CountryName");
            return View(userMaster);
        }

        public ActionResult SignIn()
        {
            return View();
        }
        public ActionResult SignOut()
        {
            UserMaster userMaster = null;
            return RedirectToAction("Home", "Home", userMaster);
        }

        [HttpPost]
        public ActionResult SignIn(UserMaster userMaster)
        {
            UserMaster validUser;
            string pwd;
            string exception;
            try
            {
                validUser = db.UserMasters.Where(u => u.UserName == userMaster.UserName).FirstOrDefault();
                pwd = Decrypt(validUser.Password);
                if (pwd == userMaster.Password)
                {
                    validUser.Country = db.Countries.Where(u => u.CountryId == validUser.CountryId).Select(u => u.CountryName).FirstOrDefault();
                    validUser.State = db.States.Where(u => u.CountryId == validUser.CountryId).Select(u => u.StateName).FirstOrDefault();
                    Session["UserName"] = validUser.UserName;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Please enter valid User Name and Password";
                    return View(userMaster);
                }
            }
            catch (Exception e)
            {
                exception = e.Message;
                return View(userMaster);
            }
        }
        public ActionResult Details(UserMaster userMaster)
        {
            if (userMaster.UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster userMaster1 = db.UserMasters.Where(u => u.UserName == userMaster.UserName).FirstOrDefault();
            if (userMaster1 != null)
                userMaster1.Password = Decrypt(userMaster1.Password);
            else
                ViewBag.ErrorMessage = "Please enter valid User Name";

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
