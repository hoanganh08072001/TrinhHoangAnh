using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
namespace CGV.Controllers.Admin
{
    public class AdminCategoryPostController : Controller
    {
        CategoryPostDao cpost = new CategoryPostDao();
        public MyDB db = new MyDB();
        // GET: AdminCategoryPost
        public ActionResult Index(string mess)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            Utils.CheckActive.checkActivePost();
            Session["checkactivepost"] = "category_post";
            ViewBag.Msg = mess;
            List<category_post> list = db.category_post.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var name = form["categoryfilm"];
            bool result = cpost.checkName(name);
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
                cpost.add(name);
                return RedirectToAction("Index", new { mess = "2" });
            }
            
        }
        public ActionResult Update(FormCollection form)
        {
            var name = form["categorypost"];
            var id = form["id"];
            var idc = Int32.Parse(id);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            bool checku = cpost.checkUpdate(idc, name);
            if (checku)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var dele = db.category_post.Where(c => c.id == idc).FirstOrDefault();
                if (dele != null)
                {
                    bool result = cpost.checkName(name);
                    if (result)
                    {
                        return RedirectToAction("Index", new { mess = "1" });
                    }
                    else
                    {
                        cpost.update(name, id);
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
            bool result = cpost.checkActive(idc);
            if (result)
            {
                var dele = db.category_post.Where(c => c.id == idc).FirstOrDefault();
                if (dele != null)
                {
                    cpost.delete(id);
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