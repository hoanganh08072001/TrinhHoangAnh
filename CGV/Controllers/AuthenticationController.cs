using System.Web.Mvc;
using CGV.Utils;
using DatabaseIO;
using Model;


namespace CGV.Controllers
{
    
    public class AuthenticationController : Controller
    {
        GenericDao genericD = new GenericDao();
        AuthenticationDao authenticationD = new AuthenticationDao();
        UserDao userD = new UserDao();
        MailUtils mailUtil = new MailUtils();
        ValidateUtils validateUtil = new ValidateUtils();
       
        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            var user = Session[Constants.Constants.USER_SESSION];
            if (user != null) {
                return RedirectToAction("IndexUser", "Home");
            } else {
                return View();
            }            
        }

        /**
         * User account registration
         * @param form
         * @return
         */
        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            var email = form["email"];
            var username = form["username"];
            var password = form["password"];
            var phonenumber = form["phonenumber"];
            var rePassword = form["rePassword"];
            var passworodMd5 = authenticationD.md5(password);
            string strongPassword = authenticationD.checkPasswordStrong(password);
            int timeOut = 2;
            int statusActive = 0;
            int roleId = 3;
            bool checkFormatEmail = validateUtil.checkFormatEmail(email);
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phonenumber) || string.IsNullOrEmpty(rePassword)) {
                ViewBag.message = Constants.Constants.FILL_OUT_ERROR;
                return View();
            } else if (checkFormatEmail) {
                ViewBag.message = Constants.Constants.FORMAT_EMAIL_ERROR;
                return View();
            } else if (!password.Equals(rePassword)) {
                ViewBag.message = Constants.Constants.PASSWORD_ERROR;
                return View();
            } else if (!string.IsNullOrEmpty(strongPassword)) {
                ViewBag.message = strongPassword;
                return View();
            } else {
                bool result = authenticationD.checkEmail(email);
                if (result) {
                    ViewBag.message = Constants.Constants.EMAIL_EXIST;
                    return View();
                } else {
                    usercgv user = new usercgv();                
                    user.email = email;
                    user.password = passworodMd5;
                    user.phonenumber = phonenumber;
                    user.username = username;
                    user.is_active = statusActive;
                    user.role_id = roleId;
                    authenticationD.register(user);
                    string subjectEmail = Constants.Constants.SUBJECT_EMAIL;
                    string code = RandomCode.GenerateRandomNo().ToString();
                    string bodyEmail = Constants.Constants.BODY_EMAIL + " " + code;
                    mailUtil.sendMail(email,subjectEmail,bodyEmail);
                    Session.Add(Constants.Constants.CODE_VERIFY, code);
                    Session.Timeout = timeOut;
                    ViewBag.email = email;
                    return View("Verify");
                }             
            }          
        }

        public ActionResult Login()
        {
            var user = Session[Constants.Constants.USER_SESSION];
            if (user != null) {
                return RedirectToAction("IndexUser","Home");
            } else {
                return View();
            }          
        }
        public ActionResult Verify()
        {
            return View();
        }

        /**
         * verify account for user
         * @param code
         * @param email
         * @return
         */
        [HttpPost]
        public JsonResult Verify(string code,string email)
        {
            var codeSession = (string)Session[Constants.Constants.CODE_VERIFY];
            if (!code.Equals(codeSession)) {             
                return Json(new { status = Constants.Constants.STATUS_ERROR, msg = Constants.Constants.VERIFY_INCORRECT, JsonRequestBehavior.AllowGet });
            } else if (string.IsNullOrEmpty(codeSession)) {
                return Json(new { status = Constants.Constants.STATUS_ERROR, msg = Constants.Constants.VERIFY_INCORRECT, JsonRequestBehavior.AllowGet });
            } else {
                userD.activeAccount(email);
                return Json(new { status = Constants.Constants.STATUS_OK, msg = Constants.Constants.VERIFY_SUCCESS, JsonRequestBehavior.AllowGet });
            }       
        }

        /**
         * provide code verify again for user
         * @param email
         * @return
         */
        [HttpPost]
        public ActionResult getCodeAgain(string email)
        {
            string code = RandomCode.GenerateRandomNo().ToString();
            string bodyEmail = Constants.Constants.BODY_EMAIL + " " + code;
            string subjectEmail = Constants.Constants.SUBJECT_EMAIL;
            mailUtil.sendMail(email,subjectEmail,bodyEmail);
            ViewBag.msg = Constants.Constants.GET_CODE_VERIFY_SUCCESS;
            ViewBag.email = email;
            Session.Add(Constants.Constants.CODE_VERIFY, code);
            Session.Timeout = 2;
            return View("Verify");
        }

        [HttpGet]
        public ActionResult getCodeAgain()
        {         
           return View("Login");
        }

        /**
         * User login into system
         * @param form
         * @return
         */
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            var email = form["email"];
            var password = form["password"];
            var passworodMd5 = authenticationD.md5(password);
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) {
                ViewBag.message = Constants.Constants.FILL_OUT_ERROR;
                return View();
            } else {
                bool result = authenticationD.checklogin(email, passworodMd5);
                if (result) {
                    //check account user is active
                    bool resultActive = authenticationD.checkActive(email);
                    if (resultActive) {
                        var userInformation = userD.getInformation(email);
                        Session.Add(Constants.Constants.USER_SESSION, userInformation);
                        return RedirectToAction("IndexUser", "Home");
                    } else {
                        ViewBag.email = email;
                        return View("Verify");
                    }                   
                } else {
                    ViewBag.message = Constants.Constants.PASSWORD_AND_USERNAME_INCORRECT;
                    return View();
                }
            }           
        }

        /**
         * User logout system
         */
        public ActionResult Logout()
        {
            Session.Remove(Constants.Constants.USER_SESSION);
            return Redirect("/");
        }
    }
}