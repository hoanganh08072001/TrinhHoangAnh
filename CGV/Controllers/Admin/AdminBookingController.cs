using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
using System.Net.Mail;
using System.Configuration;
using System.Net;
namespace CGV.Controllers.Admin
{
    public class AdminBookingController : Controller
    {
        BookingDao bk = new BookingDao();
        private MyDB db = new MyDB();
        // GET: AdminBooking
        public ActionResult Index()
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "AdminAuthen");
            }
            Utils.CheckActive.checkActiveParent();
            Session["activeParent"] = "booking";
            List<booking> list = db.bookings.OrderByDescending(b => b.id).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Accept(FormCollection form)
        {
            var id = form["id"];
            var idbook = Int32.Parse(id);
            booking b = db.bookings.Where(c => c.id == idbook).FirstOrDefault();
            var iduser = b.id_user; 
            usercgv u = db.usercgvs.Where(p => p.id == iduser).FirstOrDefault();
            var useremail = u.email;
            var a = "https://localhost:44313/booking/ticket/" + b.create_time + "/" + u.id;
            var link = "https://chart.googleapis.com/chart?chs=300x300&cht=qr&chl=" + a;
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Admin/mail/payment.html"));
            content = content.Replace("{{link}}", link);
            
            var formEmailAddress = ConfigurationManager.AppSettings["FormEmailAddress"].ToString();
            var formEmailDisplayName = ConfigurationManager.AppSettings["FormEmailDisplayName"].ToString();
            var formEmailPassword = ConfigurationManager.AppSettings["FormEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPost"].ToString();

            bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());
            MailMessage message = new MailMessage(new MailAddress(formEmailAddress, formEmailDisplayName), new MailAddress(useremail));

            message.Subject = "Thanh toán thành công!";
            message.IsBodyHtml = true;
            message.Body = content;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(formEmailAddress, formEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enableSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
            bk.accpect(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Remove()
        {
            List<booking> list = bk.getBooking();
            if (list.Count() != 0){
                for (int i = 0; i < list.Count(); i++){
                    bk.Delete(list[i].id);
                }
            }
            return Json(new { status = "SUCCESS", msg = "THÀNH CÔNG", JsonRequestBehavior.AllowGet });
        }
    }
}