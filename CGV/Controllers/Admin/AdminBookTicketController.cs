using DatabaseIO;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGV.Controllers.Admin
{
    public class AdminBookTicketController : Controller
    {
        HomeDao homeDao = new HomeDao();
        SeatDao seatD = new SeatDao();
        // GET: AdminBookTicket
        public ActionResult Index()
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            var listSeat = seatD.getAll();
            ViewBag.listseat = listSeat;
            Session["activeParent"] = "bookingticket";
            Utils.CheckActive.checkActiveParent();

            List<film> list = homeDao.getFilmNow().Distinct().ToList();
            return View(list);
        }
    }
}