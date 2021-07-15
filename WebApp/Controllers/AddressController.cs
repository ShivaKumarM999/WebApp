using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AddressController : Controller
    {
        private POCEntities db = new POCEntities();
        public ActionResult Country()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Country(Country country)
        {
            string exception;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Countries.Add(country);
                    db.SaveChanges();
                    return RedirectToAction("Details");
                }
                else
                    return View(country);
            }
            catch (Exception e)
            {
                exception = e.Message;
                if (e.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    ViewBag.ErrorMessage = "Country already exist!";
                return View(country);
            }

        }

        public ActionResult State()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            return View();
        }

        [HttpPost]
        public ActionResult State(State state)
        {
            string exception;
            try
            {
                db.States.Add(state);
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            catch (Exception e)
            {
                exception = e.Message;
                if (e.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    ViewBag.ErrorMessage = "State already exist!";
                ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", state.CountryId);
                return View(state);
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", state.CountryId);
            return View(state);
        }
        public ActionResult Details()
        {
            var q = from c in db.Countries
                    join s in db.States on c equals s.Country into cs
                    from substate in cs.DefaultIfEmpty()
                    select new { Country = c.CountryName, State = substate.StateName ?? String.Empty };

            List<Address> address = new List<Address>();
            Address address1 = null;
            foreach (var x in q)
            {
                address1 = new Address();
                address1.CountryName = x.Country;
                address1.StateName = x.State;
                address.Add(address1);
            }

            return View(address);
        }

        public ActionResult UploadState()
        {
            State state = new State();
            string[] lines = { };
            string curFile = "E:\\state.txt";

            lines = System.IO.File.ReadAllLines(curFile);

            foreach (var i in lines)
            {
                state = JsonConvert.DeserializeObject<State>(i);
                State(state);
            }
            return View(state);
        }
    }
}