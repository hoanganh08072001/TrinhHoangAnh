using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
namespace CGV.Controllers.Admin
{
    public class AdminUserController : Controller
    {
        UserDao user = new UserDao();
        AuthenticationDao authen = new AuthenticationDao();
        public MyDB db = new MyDB();
        // GET: AdminUser
        public ActionResult Index(string mess)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            ViewBag.Msg = mess;
            Utils.CheckActive.checkActiveParent();
            Session["activeParent"] = "userAdmin";
            List<usercgv> list = db.usercgvs.OrderBy(u => u.role_id).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var email = form["email"];
            var username = form["username"];
            var password = form["password"];
            var roleid = form["roleid"];
            var tt = form["trangthai"];
            var phonenumber = form["phonenumber"];
            var passworodMd5 = authen.md5(password);
            bool result = authen.checkEmail(email);
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

                user.add(email, passworodMd5, phonenumber, roleid, username, tt);

                return RedirectToAction("Index", new { mess = "2" });
            }

        }
        public ActionResult Update(FormCollection form)
        {
            var email = form["email"];
            var username = form["username"];
            var password = form["password"];
            var roleid = form["roleid"];
            var tt = form["trangthai"];
            var phonenumber = form["phonenumber"];
            var id = form["id"];
            var idu = Int32.Parse(id);
            var passworodMd5 = authen.md5(password);
            usercgv u = db.usercgvs.Where(p => p.id == idu).FirstOrDefault();
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            if (email != u.email)
            {
                var dele = db.usercgvs.Where(c => c.id == idu).FirstOrDefault();
                if (dele != null)
                {
                    bool result = authen.checkEmail(email);
                    if (result)
                    {

                        return RedirectToAction("Index", new { mess = "1" });
                    }
                    else
                    {
                        if (password == u.password)
                        {
                            user.update(email, password, phonenumber, roleid, username, id, tt);

                            return RedirectToAction("Index", new { mess = "2" });
                        }
                        else
                        {
                            user.update(email, passworodMd5, phonenumber, roleid, username, id, tt);

                            return RedirectToAction("Index", new { mess = "2" });
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Index", new { mess = "4" });
                }
            }
            else
            {
                var dele = db.usercgvs.Where(c => c.id == idu).FirstOrDefault();
                if (dele != null)
                {

                    if (password == u.password)
                    {
                        user.update(email, password, phonenumber, roleid, username, id, tt);

                        return RedirectToAction("Index", new { mess = "2" });
                    }
                    else
                    {
                        user.update(email, passworodMd5, phonenumber, roleid, username, id, tt);

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
            bool result = user.checkActive(idc);
            if (result)
            {
                return RedirectToAction("Index", new { mess = "3" });

            }
            else
            {
                var dele = db.usercgvs.Where(c => c.id == idc).FirstOrDefault();
                if (dele != null)
                {
                    user.delete(idc);
                    return RedirectToAction("Index", new { mess = "2" });
                }
                else
                {
                    return RedirectToAction("Index", new { mess = "4" });
                }

            }


        }
        /*[HttpPost]
        public JsonResult Delete(int id)
        {

           
            user.Delete(id);
            return Json(new { status = "SUCCESS", msg = "THÀNH CÔNG", JsonRequestBehavior.AllowGet });

        }*/
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            user.changStatus(id);
            return Json(new { status = "SUCCESS", msg = "THÀNH CÔNG", JsonRequestBehavior.AllowGet });
        }
    }
}