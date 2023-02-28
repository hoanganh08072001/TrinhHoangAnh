using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
namespace CGV.Controllers.Admin
{
    public class AdminShowtimeController : Controller
    {
        ShowtimeDao show = new ShowtimeDao();
        public MyDB db = new MyDB();
        // GET: AdminShowtime
        public ActionResult Index(string mess)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            ViewBag.Msg = mess;
            Session["checkactive"] = "showtime";
            Utils.CheckActive.checkActive();
            List<showtime> list = db.showtimes.OrderByDescending(s => s.id).ToList();
            return View(list);
        }
        public string addTime(string day)
        {
            string[] arrListStr = day.ToString().Split(':');
            DateTime aDateTime = new DateTime(2021, 8, 30, Int32.Parse(arrListStr[0]), Int32.Parse(arrListStr[1]), 0);
            return aDateTime.AddHours(2).ToString();
        }
        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var start = form["start"];
            
            var scheid = form["id"];
            var idRoom = form["idroom"];
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            else
            {
                var result = show.checkShow(start, scheid, idRoom);
                if (result.Count != 0)
                {
                    var message = "1";
                    return RedirectToAction("Index", new { mess = message });

                }
                else
                {
                    show.add(scheid, start, addTime(start), Int32.Parse(idRoom));
                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });

                }
            }
           
        }
        public ActionResult Update(FormCollection form)
        {
            var start = form["start"];
            var end = form["end"];
            var id = form["id"];
            var idSchedule = form["idschedule"];
            var idroom = form["idroom1"];
            var showtimeObj = show.getName(Int32.Parse(id));
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            else
            {
                if(showtimeObj.start_time.ToString() == start)
                {
                    show.update(start, addTime(start), id);
                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });
                }
                else
                {
                    var result = show.checkShow(start, idSchedule, idroom);
                    if (result.Count != 0)
                    {
                        var message = "1";
                        return RedirectToAction("Index", new { mess = message });

                    }
                    else
                    {
                        show.update(start, addTime(start), id);
                        var message = "2";
                        return RedirectToAction("Index", new { mess = message });

                    }
                }
               
            }
           
        }
        public ActionResult Delete(FormCollection form)
        {

            var id = form["id"];
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            else
            {
                bool checkActive = show.checkActive(Int32.Parse(id));
                if (checkActive)
                {
                    var message = "3";
                    return RedirectToAction("Index", new { mess = message });
                }
                else
                {
                    show.delete(id);
                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });
                }
               
            }
           

        }
        [HttpPost]
        public JsonResult getRoom(int id)
        {
            List<room> listRoom = show.getNameRoom(id);
            string html = "";
            foreach (var item in listRoom)
            {
                html += "<option value=" + item.id + ">" + item.room_name + "</option>";
            }
            return Json(new { status = "OK", data = html, msg = "thanhcong", JsonRequestBehavior.AllowGet });
        }
    }
}