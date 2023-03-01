using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DatabaseIO;
using Model;

namespace CGV.Controllers
{
   
    public class HomeController : Controller
    {
        SeatDao seatD = new SeatDao();
        public ActionResult Index()
        {    
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult IndexUser()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult FilmComingSoon()
        {
            HomeDao homeDao = new HomeDao();
            List<film> list = homeDao.getFilmComingSoon().Distinct().Take(10).ToList();
            if (list != null) {
                return PartialView(list);
            } else {
                ModelState.AddModelError(Constants.Constants.ERROR_SYSTEM, Constants.Constants.ERROR_SYTEM_DETAIL);
            }
            return PartialView(list);
        }
        [ChildActionOnly]
        public ActionResult FilmNowShowing()
        {
            HomeDao homeDao = new HomeDao();
            List<film> list = homeDao.getFilmNowShowing().Distinct().Take(10).ToList();
            if (list != null) {
                var listSeat = seatD.getAll();
                ViewBag.listseat = listSeat;
                return PartialView(list);
            } else {
                ModelState.AddModelError(Constants.Constants.ERROR_SYSTEM, Constants.Constants.ERROR_SYTEM_DETAIL);
            }        
            return PartialView(list);
        }
        [ChildActionOnly]
        public ActionResult FilmPromotion()
        {
            HomeDao homeDao = new HomeDao();
            List<post> list = homeDao.getPromotion();
            if (list != null) {
                return PartialView(list);
            } else {
                ModelState.AddModelError(Constants.Constants.ERROR_SYSTEM, Constants.Constants.ERROR_SYTEM_DETAIL);
            }
            return PartialView(list);
        }
        public ActionResult Film()
        {
            return View("IndexUser");
        }

        /**
         * get information for user
         * @param email
         * @return
         */
        public ActionResult ProfileUser(string email)
        {
            var user = Session[Constants.Constants.USER_SESSION];
            if (user == null) {
                return RedirectToAction("IndexUser", "Home");
            } else {
                UserDao userD = new UserDao();
                var model = userD.getInformation(email);
                return View(model);
            }         
        }
    }
}