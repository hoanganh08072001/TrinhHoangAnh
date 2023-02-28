using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseIO;
using Model;
using System.Web.Security;
using Newtonsoft;
using System.Net;
using System.IO;

using System.Text;
using System.Runtime.Serialization.Json;

namespace CGV.Controllers.Admin
{
    public class AdminAuthenController : Controller
    {
        MyDB mydb = new MyDB();
        AuthenticationDao authenticationD = new AuthenticationDao();
        // GET: AdminAuthen
        public ActionResult Index(string mess)
        {
            ViewBag.Msg = mess;
            return PartialView();
        }
        public ActionResult Login(FormCollection form)
        {
            var email = form["email"];
            var password = form["password"];
            var passworodMd5 = authenticationD.md5(password);
            usercgv cs = mydb.usercgvs.SingleOrDefault(u => u.email == email && u.password == passworodMd5 && u.role_id != 3);
            if (cs != null)
            {
                Session["usr"] = cs;
                return RedirectToAction("Index", "AdminHome");
                var response = Request["g-recaptcha-response"];
                var secretKey = "6Lf2AO0bAAAAABTjXd7b2X4AqZYCVUtLK-12PvTO";//Mã bí mật
                string remoteIp = Request.ServerVariables["REMOTE_ADDR"];
                string myParameters = String.Format("secret={0}&response={1}&remoteip={2}", secretKey, response, remoteIp);
                RecaptchaResult captchaResult;
                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    var json = wc.UploadString("https://www.google.com/recaptcha/api/siteverify", myParameters);
                    var js = new DataContractJsonSerializer(typeof(RecaptchaResult));
                    var ms = new MemoryStream(Encoding.ASCII.GetBytes(json));
                    captchaResult = js.ReadObject(ms) as RecaptchaResult;
                    if (captchaResult != null && captchaResult.Success)
                    {
                        Session["usr"] = cs;
                        return RedirectToAction("Index", "AdminHome");
                    }
                    else
                    {
                        var messs = "Vui lòng vượt qua Captcha";
                        return RedirectToAction("Index", new { mess = messs });
                    }
                }
            }
            var message = "Email hoặc password không đúng";
            return RedirectToAction("Index", new { mess = message });
        }
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index");

        }

    }
}