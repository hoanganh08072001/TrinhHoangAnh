using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
namespace CGV.Controllers.Admin
{
    public class AdminCategoryFilmController : Controller
    {
        CategoryFilmDao cfilm = new CategoryFilmDao();

        private MyDB db = new MyDB();
        // GET: AdminCategoryFilm
        public ActionResult Index(string mess)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            ViewBag.Msg = mess;
            Session["checkactive"] = "category_film";
            Utils.CheckActive.checkActive();
            List<category_film> list = db.category_film.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var name = form["categoryfilm"];
            bool result = cfilm.checkName(name);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            if (result)
            {

                return RedirectToAction("Index", new { mess = "1" });
            }
            else
            {
                cfilm.add(name);

                return RedirectToAction("Index", new { mess = "2" });
            }
        }
        public ActionResult Update(FormCollection form)
        {
            var name = form["categoryfilm"];
            var id = form["id"];
            var idc = Int32.Parse(id);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            bool checku = cfilm.checkUpdate(idc, name);
            if (checku)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var dele = db.category_film.Where(c => c.id == idc).FirstOrDefault();
                if (dele != null)
                {
                    bool result = cfilm.checkName(name);
                    if (result)
                    {

                        return RedirectToAction("Index", new { mess = "1" });
                    }
                    else
                    {
                        cfilm.update(name, id);
                        return RedirectToAction("Index", new { mess = "2" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", new { mess = "4" });
                }
            }
        }
        public ActionResult Delete(FormCollection form)
        {

            var id = form["id"];
            var idc = Int32.Parse(id);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            bool result = cfilm.checkActive(idc);
            if (result)
            {
                var dele = db.category_film.Where(c => c.id == idc).FirstOrDefault();
                if (dele != null)
                {
                    cfilm.delete(id);
                    return RedirectToAction("Index", new { mess = "2" });
                }
                else
                {
                    return RedirectToAction("Index", new { mess = "4" });
                }

            }
            else
            {
                return RedirectToAction("Index", new { mess = "3" });
            }


        }
    }
}