using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
namespace CGV.Controllers.Admin
{
    public class AdminRoomController : Controller
    {
        RoomDao room = new RoomDao();
        SeatDao seatD = new SeatDao();
        public MyDB db = new MyDB();
        // GET: AdminRoom
        public ActionResult Index(string mess)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            ViewBag.Msg = mess;
            Session["checkactive"] = "room";
            Utils.CheckActive.checkActive();
            List<room> list = db.rooms.Where(r => r.trash == 1).ToList();        
            return View(list);
        }
        public ActionResult Trash()
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            
            Session["checkactive"] = "room";
            Utils.CheckActive.checkActive();
            List<room> list = db.rooms.Where(r => r.trash == 0).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var listAll = seatD.getAll();
            var name = form["roomname"];
            var soluong = form["number_seat"];
            bool result = room.checkName(name);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            else
            {
                if (result)
                {
                    var message = "1";
                    return RedirectToAction("Index", new { mess = message });
                }
                else
                {
                    room.add(name);
                    var roomByID = room.getId(name);
                    for (int i = 0; i < Int32.Parse(soluong); i++)
                    {
                        room.addRoomSeat(roomByID.id, listAll[i].id);
                    }
                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });
                }
            }
          
        }
        public ActionResult Update(FormCollection form)
        {
            var name = form["roomname"];
            var id = form["id"];
            var number = form["number_seat1"];
         
            var roomname = room.getName(Int32.Parse(id));
            var listAll = seatD.getAll();
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            else
            {
                if (roomname.room_name == name)
                {
                    room.deleteRoomSeat(Int32.Parse(id));
                    room.update(name, id);
                    var roomByID = room.getId(name);
                    for (int i = 0; i < Int32.Parse(number); i++)
                    {
                        room.addRoomSeat(roomByID.id, listAll[i].id);
                    }
                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });
                }
                else
                {
                    bool result = room.checkName(name);
                    if (result)
                    {
                        var message = "1";
                        return RedirectToAction("Index", new { mess = message });
                    }
                    else
                    {
                        room.deleteRoomSeat(Int32.Parse(id));
                        room.update(name, id);
                        var roomByID = room.getId(name);
                        for (int i = 0; i < Int32.Parse(number); i++)
                        {
                            room.addRoomSeat(roomByID.id, listAll[i].id);
                        }
                        var message = "2";
                        return RedirectToAction("Index", new { mess = message });
                    }
                }


            }


        }
        [HttpPost]
        public ActionResult ChangeStatus(FormCollection form)
        {
            var id = form["id"];
            var idc = Int32.Parse(id);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            room.changStatus(idc);
            return RedirectToAction("Index", new { mess = "2" });
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
                bool checkActive = room.checkActive(Int32.Parse(id));
                if (checkActive)
                {
                    var message = "3";
                    return RedirectToAction("Index", new { mess = message });
                }
                else
                {
                   /* room.deleteRoomSeat(Int32.Parse(id));*/
                    room.changStatus(Int32.Parse(id));
                    var message = "2";
                    return RedirectToAction("Index", new { mess = message });
                }
              
               
            }
           

        }
    }
}