using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;

namespace CGV.Controllers.Admin
{
    public class AdminHomeController : Controller
    {
        public MyDB db = new MyDB();
        HomeDao home = new HomeDao();

        // GET: AdminHome
        public ActionResult Index()
        {
            if(Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            Session["activeParent"] = "home";
            Utils.CheckActive.checkActiveParent();
            ViewBag.Month1 = home.statictis(1);
            ViewBag.Month2 = home.statictis(2);
            ViewBag.Month3 = home.statictis(3);
            ViewBag.Month4 = home.statictis(4);
            ViewBag.Month5 = home.statictis(5);
            ViewBag.Month6 = home.statictis(6);
            ViewBag.Month7 = home.statictis(7);
            ViewBag.Month8 = home.statictis(8);
            ViewBag.Month9 = home.statictis(9);
            ViewBag.Month10 = home.statictis(10);
            ViewBag.Month11 = home.statictis(11);
            ViewBag.Month12 = home.statictis(12);
            ViewBag.Count1 = home.countTicket(1);
            ViewBag.Count2 = home.countTicket(2);
            ViewBag.Count3 = home.countTicket(3);
            ViewBag.Count4 = home.countTicket(4);
            ViewBag.Count5 = home.countTicket(5);
            ViewBag.Count6 = home.countTicket(6);
            ViewBag.Count7 = home.countTicket(7);
            ViewBag.Count8 = home.countTicket(8);
            ViewBag.Count9 = home.countTicket(9);
            ViewBag.Count10 = home.countTicket(10);
            ViewBag.Count11 = home.countTicket(11);
            ViewBag.Count12 = home.countTicket(12);
            ViewBag.CountFilm= db.films.Count(); 
            ViewBag.CountUser = db.usercgvs.Count();
            ViewBag.CountBooking = db.bookings.Count();
            ViewBag.SumMoney = db.bookings.ToList().Count > 0 ? db.bookings.Sum(b => b.amount) : 0;
            return View();
        }
    }
}