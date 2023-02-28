using DatabaseIO;
using System;
using System.Web.Mvc;
using CGV.Utils;
using Model;

namespace CGV.Controllers
{
    
    public class UserController : Controller
    {
        // GET: User
        MyDB db = new MyDB();
        UserDao userD = new UserDao();
        GenericDao genericD = new GenericDao();
        AuthenticationDao authenticationD = new AuthenticationDao();
        MailUtils mailUtil = new MailUtils();     
        
        public ActionResult Index()
        {
            return View();
        }

        /**
         * edit password for user
         * @param passwordOld
         * @param passwordNew
         * @param rePasswordNew
         * @param email
         * @return
         */
        [HttpPost]
        public JsonResult EditPassword(string passwordOld, string passwordNew, string rePasswordNew,string email)
        {
            JsonResult js = new JsonResult();
            Console.WriteLine(passwordOld);
            var userSession = Session[Constants.Constants.USER_SESSION];
            if (userSession != null){
                if (String.IsNullOrEmpty(passwordOld) || String.IsNullOrEmpty(passwordNew) || String.IsNullOrEmpty(rePasswordNew)) {
                    js.Data = new {
                        status = Constants.Constants.STATUS_ERROR,
                        message = Constants.Constants.FILL_OUT_ERROR
                    };
                } else if (passwordNew!= rePasswordNew) {
                    js.Data = new {
                        status = Constants.Constants.STATUS_ERROR,
                        message = Constants.Constants.PASSWORD_ERROR
                    };
                } else {
                    string passwordMd5 = authenticationD.md5(passwordOld);
                    string passwordMd5New = authenticationD.md5(passwordNew);
                    var user = userD.getUpdateProfile(email, passwordMd5);
                    if (user != null) {
                        userD.updatePassword(email, passwordMd5, passwordMd5New);
                        js.Data = new{
                            status = Constants.Constants.STATUS_SUCCESS,
                            message = Constants.Constants.UPDATE_PASSWORD_SUCCESS,                     
                        };
                    }
                }
            } else {
                  js.Data = new {
                     status = Constants.Constants.STATUS_ERROR,
                      message = Constants.Constants.YOU_LOGGED_OUT_SOMEWHERE_ELES
                  };
            }
            return Json(js,JsonRequestBehavior.AllowGet);
        }

        /**
         * update profile for user
         * @param email
         * @param username
         * @param phonenumber
         * @return
         */
        [HttpPost]
        public JsonResult updateProfile(string email, string username,string phonenumber)
        {
            JsonResult js = new JsonResult();
            usercgv user = new usercgv();
            user.phonenumber = phonenumber;
            user.username = username;
            var userSession = Session[Constants.Constants.USER_SESSION];
            if (userSession != null) {
                if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(phonenumber)) {
                    js.Data = new {
                        status = Constants.Constants.STATUS_ERROR,
                        message = Constants.Constants.FILL_OUT_ERROR
                    };
                } else {
                    userD.updateProfile(email, user);
                    js.Data = new {
                        status = Constants.Constants.STATUS_OK,
                        message = Constants.Constants.UPDATE_INFORMATION_SUCCESS,
                    };
                }
            } else {
                js.Data = new {
                    status = Constants.Constants.STATUS_ERROR,
                    message = Constants.Constants.YOU_LOGGED_OUT_SOMEWHERE_ELES
                };
            }      
            return Json(js, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Forgot(string msg)
        {
            ViewBag.msg = msg;
            return View();
        }
        [HttpGet] 
        public ActionResult GetForgot()
        {
            return View();
        }

        /**
         * forgot password for user
         * @param email      
         * @return
         */
        [HttpPost]
        public ActionResult PostForgot(string email)
        {    
            bool checkemail = authenticationD.checkEmail(email);
            if (checkemail) {
                string subjectEmail = Constants.Constants.SUBJECT_EMAIL_FORGOT;
                string code = RandomCode.GenerateRandomNo().ToString();
                string bodyEmail = Constants.Constants.BODY_EMAIL_FORGOT + " " + code;
                mailUtil.sendMail(email,subjectEmail, bodyEmail);
                Session.Add(Constants.Constants.FOR_GOT, code);
                Session.Add(Constants.Constants.EMAIL_SESSION, email);
                Session.Timeout = 2;
                return RedirectToAction("GetForgot");
            } else {
                return RedirectToAction("Forgot", new { msg = Constants.Constants.EMAIL_NOT_EXIST });
            }      
        }

        /**
         * Verify forgot password for user
         * @param code      
         * @return
         */
        [HttpPost]
        public JsonResult VerifyForgot(string code)
        {
            var codeSession = (string)Session[Constants.Constants.FOR_GOT];
            if (!code.Equals(codeSession)) {
                return Json(new { status = Constants.Constants.STATUS_ERROR, msg = Constants.Constants.VERIFY_INCORRECT, JsonRequestBehavior.AllowGet });
            } else if (string.IsNullOrEmpty(codeSession) || string.IsNullOrEmpty(codeSession)) {
                return Json(new { status = Constants.Constants.STATUS_ERROR, msg = Constants.Constants.VERIFY_INCORRECT, JsonRequestBehavior.AllowGet });
            } else {             
                return Json(new { status = Constants.Constants.STATUS_OK, msg = Constants.Constants.VERIFY_SUCCESS, JsonRequestBehavior.AllowGet });
            }
        }

        /**
         * get new password for user   
         * @return
         */
        [HttpGet]
        public ActionResult NewPassword()
        {
            var email = (string)Session[Constants.Constants.EMAIL_SESSION];
            if (!string.IsNullOrEmpty(email)) {
                return View();
            } else {
                return RedirectToAction("Login", "Authentication");
            }        
        }

        /**
         * post new password for user   
         * @param password
         * @return
         */
        [HttpPost]
        public JsonResult PostNewPassword(string password)
        {
            var email = (string)Session[Constants.Constants.EMAIL_SESSION];
            if (!string.IsNullOrEmpty(email)) {
                authenticationD.forgotPassword(email,password);
                Session.Remove(Constants.Constants.EMAIL_SESSION);
                return Json(new { status = Constants.Constants.STATUS_OK, msg = Constants.Constants.UPDATE_PASSWORD_SUCCESS, JsonRequestBehavior.AllowGet });
            } else {
                return Json(new { status = Constants.Constants.STATUS_ERROR, msg = Constants.Constants.UPDATE_PASSWORD_ERROR, JsonRequestBehavior.AllowGet });
            }       
        }
       
    }
}