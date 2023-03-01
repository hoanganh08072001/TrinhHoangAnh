using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
namespace CGV.Controllers.Admin
{
    public class AdminScheduleController : Controller
    {
        ScheduleDao sche = new ScheduleDao();
        public MyDB db = new MyDB();
        // GET: AdminSchedule
        public ActionResult Index(string mess)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            ViewBag.Msg = mess;
            Session["checkactive"] = "schedule";
            Utils.CheckActive.checkActive();
            List<schedule> list = db.schedules.OrderByDescending(s => s.dateschedule ).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var dateschedule = form["dateschedule"];
            DateTime datesche =  DateTime.Parse(dateschedule);
            var filmid = form["filmid"];
            var roomid = form["idroom"];
            int idR = Int32.Parse(roomid);
            int idF = Int32.Parse(filmid);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            else
            {
                bool result = sche.checkScheduleRoom(datesche, idR,idF);
                if (result)
                {
                    var message = "1";
                    return RedirectToAction("Index", new { mess = message });
                }
                else
                {
                    sche.add(filmid, datesche);
                    int idSchedule = sche.getSCheduleByDate();
                    sche.addScheduleRoom(idSchedule, Int32.Parse(roomid));
                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });
                }
            }
          
        }
        public ActionResult Update(FormCollection form)
        {
            var dateschedule = form["dateschedule"];
            var filmid = form["filmid"];
            var id = form["id"];
            var roomid = form["idroom"];
            DateTime datesche = DateTime.Parse(dateschedule);
            int idR = Int32.Parse(roomid);
            int idSchedule = Int32.Parse(id);
            int idF = Int32.Parse(filmid);
            var scheduleO = sche.getName(idSchedule);
            var roomO = sche.getNameRoom(idSchedule);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            else
            {
                if (scheduleO.film_id == idF && scheduleO.dateschedule == datesche && roomO.id == idR)
                {
                    sche.deleteScheduleRoom(idSchedule);
                    sche.addScheduleRoom(idSchedule, idR);
                    sche.update(filmid, datesche, id);
                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });
                }
                else
                {
                    bool checkExit = sche.checkScheduleRoomUpdate(datesche, idR, idF);
                    if (checkExit)
                    {
                        var message = "3";
                        return RedirectToAction("Index", new { mess = message });
                    }
                    else
                    {
                        
                            sche.deleteScheduleRoom(idSchedule);
                            sche.addScheduleRoom(idSchedule, idR);
                            if (scheduleO.showtimes.Any())
                            {
                                sche.editRoomShowTime(idSchedule, idR);
                            }
                            sche.update(filmid, datesche, id);
                            var message = "2";
                            return RedirectToAction("Index", new { mess = message });
                       
                      
                    }

                }

            }



        }
        public ActionResult Delete(FormCollection form)
        {
            var id = form["id"];
            var idR = form["idRoom"];
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            else
            {

                bool checkA = sche.checkShowtimeActive(Int32.Parse(id), Int32.Parse(idR));
                if (checkA)
                {
                    var message = "3";
                    return RedirectToAction("Index", new { mess = message });
                }
                else
                {
                    sche.deleteScheduleRoom(Int32.Parse(id));
                    sche.delete(id);

                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });
                }
               
            }
          

        }
       
    }
}