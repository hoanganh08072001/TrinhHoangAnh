using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
namespace CGV.Controllers.Admin
{
    public class AdminSeatController : Controller
    {
        SeatDao seat = new SeatDao();
        public MyDB db = new MyDB();
        // GET: AdminSeat
        public ActionResult Index(string mess)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            ViewBag.Msg = mess;
            Session["checkactive"] = "seat";
            Utils.CheckActive.checkActive();
            List<seat> list = db.seats.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var name = form["seatname"];
            bool result = seat.checkName(name);
            if (result)
            {
                var message = "1";
                return RedirectToAction("Index", new { mess = message });
            }
            else
            {
                seat.add(name);

                var message = "2";
                return RedirectToAction("Index", new { mess = message });
            }
        }
        public ActionResult Update(FormCollection form)
        {
            var name = form["seatname"];
            var id = form["id"];
            var seatObj = seat.getName(Int32.Parse(id));
            if (seatObj.seat_name == name)
            {
                seat.update(name, id);
                var message = "2";
                return RedirectToAction("Index", new { mess = message });
            }
            else
            {
                bool result = seat.checkName(name);
                if (result)
                {
                    var message = "1";
                    return RedirectToAction("Index", new { mess = message });
                }
                else
                {
                    seat.update(name, id);
                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });
                }
               
            }
           
        }
        public ActionResult Delete(FormCollection form)
        {

            var id = form["id"];
            bool checkActive = seat.checkActive(Int32.Parse(id));
            if (checkActive)
            {
                var message = "3";
                return RedirectToAction("Index", new { mess = message });
            }
            else
            {
                seat.delete(id);
                var message = "2";
                return RedirectToAction("Index", new { mess = message });
            }
            

        }
    }
}