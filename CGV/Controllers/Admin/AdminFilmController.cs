using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Net;
namespace CGV.Controllers.Admin
{
    public class AdminFilmController : Controller
    {
        FilmDao post = new FilmDao();
        public MyDB db = new MyDB();
        // GET: AdminFilm
        public ActionResult Index(string mess)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            ViewBag.Msg = mess;
            Utils.CheckActive.checkActive();
            Session["checkactive"] = "film";
            List<film> list = db.films.Where(f => f.trash == 1).OrderByDescending(f => f.id).ToList();
            return View(list);
        }
        public ActionResult Trash()
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            Utils.CheckActive.checkActive();
            Session["checkactive"] = "film";
            List<film> list = db.films.Where(f => f.trash == 0).OrderByDescending(f => f.id).ToList();
            return View(list);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(FormCollection form)
        {
            var tenphim = form["tenphim"];
            var dienvien = form["dienvien"];
            var daodien = form["daodien"];
            var thoiluong = form["thoiluong"];
            var ngaycc = form["ngaycc"];
            var theloai = form["theloai"];
            var trailer = form["trailer"];
            var file = Request.Files["file"];
            var noidung = form["noidung"];
            Random random = new Random();
            int num = random.Next();
            bool result = post.checkName(tenphim);
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
                String filename = "film" + num + file.FileName.Substring(file.FileName.LastIndexOf("."));
                String Strpath = Path.Combine(Server.MapPath("~/Content/Assets/images/"), filename);
                file.SaveAs(Strpath);
                post.add(noidung, daodien, dienvien, thoiluong, tenphim, filename, trailer, theloai,ngaycc);
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Admin/mail/mailbody.html"));
                content = content.Replace("{{title}}", tenphim);
                content = content.Replace("{{noidung}}", noidung);
                List<usercgv> listuser = db.usercgvs.Where(p => p.role_id == 3).ToList();
                foreach (var item in listuser)
                {
                    var formEmailAddress = ConfigurationManager.AppSettings["FormEmailAddress"].ToString();
                    var formEmailDisplayName = ConfigurationManager.AppSettings["FormEmailDisplayName"].ToString();
                    var formEmailPassword = ConfigurationManager.AppSettings["FormEmailPassword"].ToString();
                    var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                    var smtpPort = ConfigurationManager.AppSettings["SMTPPost"].ToString();

                    bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());
                    MailMessage message = new MailMessage(new MailAddress(formEmailAddress, formEmailDisplayName), new MailAddress(item.email));

                    message.Subject = "Phim sắp chiếu - HaUI Cinema";
                    message.IsBodyHtml = true;
                    message.Body = content;

                    var client = new SmtpClient();
                    client.Credentials = new NetworkCredential(formEmailAddress, formEmailPassword);
                    client.Host = smtpHost;
                    client.EnableSsl = enableSsl;
                    client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
                    client.Send(message);
                }
                return RedirectToAction("Index", new { mess = "2" });
            }
        }
        [ValidateInput(false)]
        public ActionResult Update(FormCollection form)
        {
            var id = form["id"];
            var idu = Int32.Parse(id);
            var tenphim = form["tenphim"];
            var dienvien = form["dienvien"];
            var daodien = form["daodien"];
            var thoiluong = form["thoiluong"];
            var ngaycc = form["ngaycc"];
            var theloai = form["theloai"];
            var trailer = form["trailer"];
            var file = Request.Files["file"];
            var noidung = form["noidung"];
            var img = form["anh"];
            Random random = new Random();
            int num = random.Next();
            bool result = post.checkName(tenphim);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            film u = db.films.Where(p => p.id == idu).FirstOrDefault();
            if (tenphim != u.film_name)
            {
                var dele = db.films.Where(c => c.id == idu).FirstOrDefault();
                if (dele != null)
                {
                    if (result)
                    {

                        return RedirectToAction("Index", new { mess = "1" });
                    }
                    else
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            String filename = "film" + num + file.FileName.Substring(file.FileName.LastIndexOf("."));
                            String Strpath = Path.Combine(Server.MapPath("~/Content/Assets/images/"), filename);
                            file.SaveAs(Strpath);
                            post.update(noidung, daodien, dienvien, thoiluong, tenphim, filename, trailer, theloai, id, ngaycc);
                            return RedirectToAction("Index", new { mess = "2" });
                        }

                        post.update(noidung, daodien, dienvien, thoiluong, tenphim, img, trailer, theloai, id, ngaycc);
                        return RedirectToAction("Index", new { mess = "2" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", new { mess = "4" });
                }
            }
            else
            {
                var dele = db.films.Where(c => c.id == idu).FirstOrDefault();
                if (dele != null)
                {
                    
                        if (file != null && file.ContentLength > 0)
                        {
                            String filename = "film" + num + file.FileName.Substring(file.FileName.LastIndexOf("."));
                            String Strpath = Path.Combine(Server.MapPath("~/Content/Assets/images/"), filename);
                            file.SaveAs(Strpath);
                            post.update(noidung, daodien, dienvien, thoiluong, tenphim, filename, trailer, theloai, id, ngaycc);
                        return RedirectToAction("Index", new { mess = "2" });
                         }

                        post.update(noidung, daodien, dienvien, thoiluong, tenphim, img, trailer, theloai, id, ngaycc);
                        return RedirectToAction("Index", new { mess = "2" });

                }
                else
                {
                    return RedirectToAction("Index", new { mess = "4" });
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
            post.changStatus(idc);
            return RedirectToAction("Index", new { mess = "2" });
        }
        public ActionResult Delete(FormCollection form)
        {

            var id = form["id"];
            var idc = Int32.Parse(id);
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            bool result = post.checkActive(idc);
            if (result)
            {
                var dele = db.films.Where(c => c.id == idc).FirstOrDefault();
                if (dele != null)
                {
                    post.changStatus(idc);
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