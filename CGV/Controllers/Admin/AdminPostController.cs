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
    public class AdminPostController : Controller
    {
        PostDao post = new PostDao();
        public MyDB db = new MyDB();
        // GET: AdminPost
        public ActionResult Index(string mess)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            ViewBag.Msg = mess;
            Session["checkactivepost"] = "post";
            Utils.CheckActive.checkActivePost();
            List<post> list = db.posts.OrderByDescending(p => p.created_at).ToList();
            return View(list);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(FormCollection form)
        {
            var title = form["title"];
            var theloai = form["theloai"];
            var file = Request.Files["file"];
            var noidung = form["noidung"];
            Random random = new Random();
            int num = random.Next();
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            String filename = "post" + num + file.FileName.Substring(file.FileName.LastIndexOf("."));
            String Strpath = Path.Combine(Server.MapPath("~/Content/Assets/images/"), filename);
            file.SaveAs(Strpath);
            post.add(title,theloai,filename,noidung);
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Admin/mail/mailbody.html"));
            content = content.Replace("{{title}}", title);
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
      
                message.Subject = "Khuyến mãi từ HaUI Cinema";
                message.IsBodyHtml = true;
                message.Body = content;

                var client = new SmtpClient();
                client.Credentials = new NetworkCredential(formEmailAddress, formEmailPassword);
                client.Host = smtpHost;
                client.EnableSsl = enableSsl;
                client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
                client.Send(message);
            }
            var messag = "2";
            return RedirectToAction("Index", new { mess = messag });
        }




        [ValidateInput(false)]
        public ActionResult Update(FormCollection form)
        {
            var id = form["id"];
            var idu = Int32.Parse(id);
            var title = form["title"];
            var theloai = form["theloai"];
            var file = Request.Files["file"];
            var img = form["anh"];
            var noidung = form["noidung"];
            Random random = new Random();
            int num = random.Next();
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            var dele = db.posts.Where(c => c.id == idu).FirstOrDefault();
            if (dele != null)
            {
                if (file != null && file.ContentLength > 0)
                {
                    String filename = "post" + num + file.FileName.Substring(file.FileName.LastIndexOf("."));
                    String Strpath = Path.Combine(Server.MapPath("~/Content/Assets/images/"), filename);
                    file.SaveAs(Strpath);
                    post.update(title, theloai, filename, noidung, id);
                    return RedirectToAction("Index");
                }

                post.update(title, theloai, img, noidung, id);
                return RedirectToAction("Index", new { mess = "2" });
            }
            else
            {
                return RedirectToAction("Index", new { mess = "4" });
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
            var dele = db.posts.Where(c => c.id == idc).FirstOrDefault();
            if (dele != null)
            {
                post.delete(id);
                return RedirectToAction("Index", new { mess = "2" });
            }
            else
            {
                return RedirectToAction("Index", new { mess = "4" });
            }
            

        }
    }
}