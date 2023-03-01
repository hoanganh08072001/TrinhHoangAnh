using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DatabaseIO;
using Model;
using CGV.Utils;
using System.Linq;

namespace CGV.Controllers
{
    
    
    public class FilmController : Controller
    {
        // GET: Film
        FilmDao filmD = new FilmDao();
        ScheduleDao scheduleD = new ScheduleDao();
        RoomDao roomD = new RoomDao();
        ShowtimeDao showtimeD = new ShowtimeDao();
        BookingDao bookingDao = new BookingDao();
        SeatDao seatD = new SeatDao();
        CommentDao commentD = new CommentDao();
        MailUtils mailUtil = new MailUtils();
        PaymentStripe paymentOnline = new PaymentStripe();
        CategoryFilmDao cateD = new CategoryFilmDao();
      
        public ActionResult Index()
        {
            return View();
        }

        /**
         * get detail film by id for user
         * @param id
         * @return
         */
        public ActionResult DetailFilm(string id)
        {
            //select film from database by id
            film film = filmD.getDetailFilm(id);
            var listSeat = seatD.getAll();
            if(listSeat != null || film != null) {
                ViewBag.listseat = listSeat;
                return View(film);
            } else {
                ModelState.AddModelError(Constants.Constants.ERROR_SYSTEM, Constants.Constants.ERROR_SYTEM_DETAIL);
            }
            return View(film);
        }

        /**
         * search film by keyseach for user
         * @param keySearch
         * @return
         */
        public ActionResult SearchFilm(string keySearch)
        {
            if (string.IsNullOrEmpty(keySearch)) {
                return RedirectToAction("IndexUser","Home");
            } else {
                //select film from database by key word
                var listSearch = filmD.searchFilm(keySearch);
                ViewBag.keySearch = keySearch;
                var listSeat = seatD.getAll();
                if( listSeat !=null || listSearch != null) {
                    ViewBag.listseat = listSeat;
                    return View(listSearch);
                } else {
                    ModelState.AddModelError(Constants.Constants.ERROR_SYSTEM, Constants.Constants.ERROR_SYTEM_DETAIL);
                }             
                return View(listSearch);
            }      
        }

        /**
         * get schedule by id for user
         * @param id
         * @return
         */
        [HttpPost]
        public  JsonResult  getSchedule(string id)
        {
            //select list schedule by id
             List<schedule> list = scheduleD.getSchedule(id);      
             if (Int32.Parse(id) == 0) {
                  return Json(new { status = Constants.Constants.STATUS_ERROR, msg = Constants.Constants.FILL_OUT_SCHEDULE, JsonRequestBehavior.AllowGet });
             } else {
                  string html = filmD.renderSchedule(list);
                  return Json(new { status = Constants.Constants.STATUS_OK, data = html, msg = Constants.Constants.MSG_SUCCESS, JsonRequestBehavior.AllowGet });
             }    
        }

        /**
         * get showtime by id and idRoom for user
         * @param id
         * @param idRoom
         * @return
         */
        [HttpPost]
        public JsonResult getShowtime(string id,int idRoom)
        {
            List<showtime> listShowtime = null;
            string html = null;
            var idSche = Int32.Parse(id); 
            var scheduledate = scheduleD.getName(idSche);
            string dtnow = DateTime.Today.ToString(Constants.Constants.FORMAT_DATE_STRING);
            int todayd = Int32.Parse(dtnow);
            string dateint = String.Format(Constants.Constants.FORMAT_DATE_SCHEDULE, scheduledate.dateschedule);
            int datesche = Int32.Parse(dateint);
            if (datesche == todayd) {
                listShowtime = showtimeD.getShowtime(id, idRoom);
                html = filmD.renderShowtime(listShowtime);
                return Json(new { status = Constants.Constants.STATUS_OK, data = html, msg = Constants.Constants.MSG_SUCCESS, JsonRequestBehavior.AllowGet });
            } else {
                listShowtime = showtimeD.getShowtimes(id, idRoom);
                html = filmD.renderShowtime(listShowtime);
                return Json(new { status = Constants.Constants.STATUS_OK, data = html, msg = Constants.Constants.MSG_SUCCESS, JsonRequestBehavior.AllowGet });
            }           
        }

        /**
         * get room by id for user
         * @param id
         * @return
         */
        [HttpPost]
        public JsonResult getRoom(int id)
        {
            //select list room from database by id
            List<room> listRoom = roomD.getRoom(id);
            string html = filmD.renderRoom(listRoom);        
            return Json(new { status = Constants.Constants.STATUS_OK, data = html, msg = Constants.Constants.MSG_SUCCESS, JsonRequestBehavior.AllowGet });
        }

        /**
         * get seat by id for user
         * @param id
         * @param showtimeId
         * @param filmId
         * @param scheduleId
         * @return
         */
        [HttpPost]
        public JsonResult getSeat(int roomId, int showtimeId, int filmId, int scheduleId)
        {
            //select list seat done by roomId, showtimeId, filmId, scheduleId
            List<seat> listSeat = seatD.getSeatDone(roomId, showtimeId, filmId, scheduleId);
            //select list seat  by roomId, showtimeId, filmId, scheduleId
            List<seat> listSeatRoom = seatD.getSeatRoom(roomId);
            var listSeatActive = filmD.handleGetSeat(listSeatRoom, listSeat); ;
            string html = filmD.renderSeat(listSeatActive);        
            return Json(new { status = Constants.Constants.STATUS_OK, data = html, msg = Constants.Constants.MSG_SUCCESS, JsonRequestBehavior.AllowGet });
        }


        /**
         * booking ticket for user 
         * @param film_id
         * @param schedule_id
         * @param showtime_id
         * @param room_id
         * @param seat_id
         * @return
         */
        [HttpPost]
        public JsonResult bookingTicket( int film_id,int schedule_id,int showtime_id, int room_id, int[] seat_id )
        {
            var userInfomatiom = (usercgv)Session[Constants.Constants.USER_SESSION];
            if (userInfomatiom == null ) {
                return Json(new { status = Constants.Constants.STATUS_ERROR, msg = Constants.Constants.YOU_NEED_LOGIN, JsonRequestBehavior.AllowGet });
            } else {
                //check if the data has changed
                bool checkExist = bookingDao.checkBooking(film_id,schedule_id,showtime_id,room_id,seat_id);
                //check data showtime is working
                bool checkExistShowtime = showtimeD.checkExistShowtime(showtime_id);
                if (checkExist || !checkExistShowtime) {
                    return Json(new { status = Constants.Constants.STATUS_ERROR_ONE, msg = Constants.Constants.CHANGE_DATABASE, JsonRequestBehavior.AllowGet });
                } else {
                    int amount = 0;
                    var listHis = new List<HistoryBooking>();
                    var listG = new List<String>();
                    int lengthSeat = seat_id.Length;
                    string stringDay = null;
                    HistoryBooking his = null;
                    string dateschedule = null;
                    for (int i = 0; i < lengthSeat; i++) {
                        dateschedule = String.Format(Constants.Constants.FORMAT_DATE, scheduleD.getName(schedule_id).dateschedule);
                        stringDay = showtimeD.getName(showtime_id).start_time + "-" + showtimeD.getName(showtime_id).end_time;
                        amount += Constants.Constants.PRICE_TICKET;
                        his = bookingDao.addHistoryBooking(film_id, i, room_id, seat_id[i], dateschedule, amount, stringDay, 0);
                        listHis.Add(his);
                        listG.Add(seatD.getName(seat_id[i]).seat_name);
                    }
                    DateTime now = DateTime.Now;
                    booking book = bookingDao.newObjectBooking(schedule_id, film_id, room_id, showtime_id, userInfomatiom.id, now.Ticks.ToString());
                    Session.Add(Constants.Constants.DATE_NOW_STRING, now.Ticks.ToString());
                    Session.Add(Constants.Constants.ORDER, book);
                    Session.Add(Constants.Constants.LENGTH_SEAT, seat_id);
                    Session session = paymentOnline.paymentOnline(filmD.getName(film_id).film_name, lengthSeat);
                    return Json(new { status = Constants.Constants.STATUS_OK, data1 = listHis, data2 = listG, data3 = session, msg = Constants.Constants.BOOKINGTICKET_SUCCESS, JsonRequestBehavior.AllowGet });
                }
               
            }        
        }

        /**
         * booking ticket for user 
         * @param film_id
         * @param schedule_id
         * @param showtime_id
         * @param room_id
         * @param seat_id
         * @return
         */
        [HttpPost]
        public JsonResult bookingTicke(int film_id, int schedule_id, int showtime_id, int room_id, int[] seat_id)
        {
            int status = 0;
            int idUserDefault = 1;
            //check if the data has changed
            bool checkExist = bookingDao.checkBooking(film_id, schedule_id, showtime_id, room_id, seat_id);
            //check data showtime is working
            bool checkExistShowtime = showtimeD.checkExistShowtime(showtime_id);
            if (checkExist || !checkExistShowtime) {
                return Json(new { status = Constants.Constants.STATUS_ERROR, msg = Constants.Constants.CHANGE_DATABASE, JsonRequestBehavior.AllowGet });
            } else {
                int amount = 0;
                var listHis = new List<HistoryBooking>();
                var listG = new List<String>();
                int lengthSeat = seat_id.Length;
                string stringDay = null;
                string dateschedule = null;
                HistoryBooking his = null;
                for (int i = 0; i < lengthSeat; i++) {
                    dateschedule = String.Format(Constants.Constants.FORMAT_DATE, scheduleD.getName(schedule_id).dateschedule);
                    stringDay = showtimeD.getName(showtime_id).start_time + "-" + showtimeD.getName(showtime_id).end_time;
                    amount += Constants.Constants.PRICE_TICKET;
                    his = bookingDao.addHistoryBooking(film_id, i, room_id, seat_id[i], dateschedule, amount, stringDay, status);
                    listHis.Add(his);
                    listG.Add(seatD.getName(seat_id[i]).seat_name);
                }
                DateTime now = DateTime.Now;
                booking book = bookingDao.newObjectBooking(schedule_id, film_id, room_id, showtime_id, idUserDefault, now.Ticks.ToString());
                Session.Add(Constants.Constants.LENGTH_SEAT, seat_id);
                Session.Add(Constants.Constants.ORDER, book);
                return Json(new { status = Constants.Constants.STATUS_OK, data1 = listHis, data2 = listG, msg = Constants.Constants.BOOKINGTICKET_SUCCESS, JsonRequestBehavior.AllowGet });
            }          
        }

        /**
         * User payment success
         * @return
         */
        public ActionResult paymentSuccess()
        {
            int status = 1;
            var userInfomatiom = (usercgv)Session[Constants.Constants.USER_SESSION];
            var arrSeat = (int[])Session[Constants.Constants.LENGTH_SEAT];
            var order = (booking)Session[Constants.Constants.ORDER];
            var dateNowString = (string)Session[Constants.Constants.DATE_NOW_STRING];
            if (order != null && dateNowString != null) {
                //insert booking ticket into database
                bookingDao.addBookingTicket(arrSeat, order, dateNowString, userInfomatiom.id, status);
                string stringLink = Constants.Constants.BOOKING_URL + dateNowString + "/" + userInfomatiom.id;
                var link = Constants.Constants.GOOGLE_URL + stringLink;
                string content = System.IO.File.ReadAllText(Server.MapPath(Constants.Constants.PATH_SENDMAIL));
                content = content.Replace("{{link}}", link);
                ViewBag.link = stringLink;
                mailUtil.sendMail(userInfomatiom.email,Constants.Constants.SUBJECT_EMAIL_QR,content);             
                Session.Remove(Constants.Constants.ORDER);
                Session.Remove(Constants.Constants.DATE_NOW_STRING);
                return View();
            } else { 
                return RedirectToAction("IndexUser", "Home");
            }      
        }

        /**
         * payment for user at cinema 
         * @return
         */
        public ActionResult paymentAtCine()
        {
            int status = 0;
            var userInfomatiom = (usercgv)Session[Constants.Constants.USER_SESSION];
            var arrSeat = (int[])Session[Constants.Constants.LENGTH_SEAT];
            var order = (booking)Session[Constants.Constants.ORDER];
            var dateNowString = (string)Session[Constants.Constants.DATE_NOW_STRING];
            if (order != null && dateNowString != null && userInfomatiom != null) {
                bookingDao.addBookingTicket(arrSeat, order, dateNowString, userInfomatiom.id, status);
                string stringLink = Constants.Constants.BOOKING_URL + dateNowString + "/" + userInfomatiom.id;
                var link = Constants.Constants.GOOGLE_URL + stringLink;
                string content = System.IO.File.ReadAllText(Server.MapPath(Constants.Constants.PATH_SENDMAIL));
                content = content.Replace("{{link}}", link);
                ViewBag.link = stringLink;
                mailUtil.sendMail(userInfomatiom.email, Constants.Constants.SUBJECT_EMAIL_QR, content);
                Session.Remove(Constants.Constants.ORDER);
                Session.Remove(Constants.Constants.DATE_NOW_STRING);
                return View();
            } else {
                return RedirectToAction("IndexUser", "Home");
            }
        }

        /**
         * Admin booking ticket for user at cinema 
         * @return
         */
        public JsonResult paymentAtCinema()
        {
            int idUserDefault = 1;
            int status = 1;
            var arrSeat = (int[])Session[Constants.Constants.LENGTH_SEAT];
            var order = (booking)Session[Constants.Constants.ORDER];
            DateTime now = DateTime.Now;
            bookingDao.addBookingTicket(arrSeat, order, now.Ticks.ToString(), idUserDefault, status);
            return Json(new { status = Constants.Constants.STATUS_OK, msg = Constants.Constants.BOOKINGTICKET_SUCCESS, JsonRequestBehavior.AllowGet });
        }

        /**
         * User payment error
         * @return
         */
        public ActionResult paymentError()
        {
            var order = (booking)Session[Constants.Constants.ORDER];
            var dateNowString = (string)Session[Constants.Constants.DATE_NOW_STRING];
            if (order != null && !string.IsNullOrEmpty(dateNowString)) {
                return View();
            } else {
                return RedirectToAction("IndexUser", "Home");
            }
        }

        /**
         * get QR code for user
         * @param dataNow
         * @param id
         * @return
         */
        public ActionResult qrResult(string dateNow,int id)
        {
            //select list order from database
            var list = filmD.getOrder(dateNow, id);
            var listHis = new List<HistoryBooking>();
            int lengthListOrder = list.Count;
            string stringDay = null;
            HistoryBooking his = null;
            string schedulename = null;
            for (int i = 0; i < lengthListOrder; i++) {      
                schedulename = scheduleD.getName(list[i].schedule_id).dateschedule.ToString();
                stringDay = showtimeD.getName(list[i].showtime_id).start_time + "-" + showtimeD.getName(list[i].showtime_id).end_time;
                his = bookingDao.addHistoryBooking(list[i].film_id, i, list[i].room_id, list[i].seat_id, schedulename, list[i].amount, stringDay, list[i].status);
                listHis.Add(his);
            }
            return View(listHis);
        }

        /**
         * post comment film for user
         * @param idFilm
         * @param textComment
         * @param numberstar
         * @return
         */
        [HttpPost]
        public JsonResult postCommnet(int idFilm, string textComment, int numberstar)
        {
            var userInfomatiom = (usercgv)Session[Constants.Constants.USER_SESSION];
            if (userInfomatiom == null) {
                return Json(new { status = Constants.Constants.STATUS_ERROR, msg = Constants.Constants.YOU_NEED_LOGIN, JsonRequestBehavior.AllowGet });
            } else {
                rating rating = new rating();
                rating.film_id = idFilm;
                rating.id_user = userInfomatiom.id;
                rating.name_user = userInfomatiom.username;
                rating.rate = textComment;
                rating.number_start = numberstar;
                commentD.comment(rating);
                //select list comment from database by id
                var listcomment = commentD.getCommentById(idFilm);
                var listAjax = commentD.addCommentAjax(listcomment);
                return Json(new { status = Constants.Constants.STATUS_SUCCESS, data = listAjax, msg = Constants.Constants.MSG_SUCCESS, JsonRequestBehavior.AllowGet });
            }
        }

        /**
         * delete comment film for user
         * @param idFilm
         * @param id
         * @return
         */
        [HttpPost]
        public JsonResult deleteComment(int id,int idFilm)
        {           
            commentD.deleteComment(id);
            var listcomment = commentD.getCommentById(idFilm);
            int lengthList = listcomment.Count;
            return Json(new { status = Constants.Constants.STATUS_SUCCESS,data = lengthList, msg = Constants.Constants.MSG_SUCCESS, JsonRequestBehavior.AllowGet });
        }
        /**
         * get history booking ticket of user
         * @return
         */
        public ActionResult HistoryBooking()
        {
            var user = Session[Constants.Constants.USER_SESSION];
            if (user == null) {
                return RedirectToAction("IndexUser", "Home");
            } else {
                var userInfomatiom = (usercgv)Session[Constants.Constants.USER_SESSION];
                var list = filmD.getBooking(userInfomatiom.id);
                var listHis = new List<HistoryBooking>();
                int lengthListOrder = list.Count;
                string stringDay = null;
                string dateschedule = null;
                HistoryBooking his = null;
                for (int i = 0; i < lengthListOrder; i++) {
                    dateschedule = String.Format(Constants.Constants.FORMAT_DATE, scheduleD.getName(list[i].schedule_id).dateschedule);
                    stringDay = showtimeD.getName(list[i].showtime_id).start_time + "-" + showtimeD.getName(list[i].showtime_id).end_time;
                    his = bookingDao.addHistoryBooking(list[i].film_id, i, list[i].room_id, list[i].seat_id, dateschedule, 3, stringDay, list[i].status);
                    listHis.Add(his);
                }
                return View(listHis);
            }
            
        }

        public ActionResult FilmComingSoon()
        {
            HomeDao homeDao = new HomeDao();
            List<film> list = homeDao.getFilmComingSoon().Distinct().ToList();
            if (list != null)
            {
                return View(list);
            }
            else
            {
                ModelState.AddModelError(Constants.Constants.ERROR_SYSTEM, Constants.Constants.ERROR_SYTEM_DETAIL);
            }
            return View(list);
        }

        public ActionResult FilmNowShowing()
        {
            HomeDao homeDao = new HomeDao();
            List<film> list = homeDao.getFilmComingSoon().Distinct().ToList();
            if (list != null)
            {
                var listSeat = seatD.getAll();
                ViewBag.listseat = listSeat;
                return View(list);
            }
            else
            {
                ModelState.AddModelError(Constants.Constants.ERROR_SYSTEM, Constants.Constants.ERROR_SYTEM_DETAIL);
            }
            return View(list);
        }

        public ActionResult FilmByCategory(int id)
        {
            List<film> list = filmD.getByCategory(id).Distinct().ToList();
            var name = cateD.getName(id).name;
            if (list != null)
            {
                var listSeat = seatD.getAll();
                ViewBag.listseat = listSeat;
                ViewBag.CateFim = name;
                return View(list);
            }
            else
            {
                ModelState.AddModelError(Constants.Constants.ERROR_SYSTEM, Constants.Constants.ERROR_SYTEM_DETAIL);
            }
            return View(list);
        }

    }
}