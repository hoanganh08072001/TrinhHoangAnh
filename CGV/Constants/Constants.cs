using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGV.Constants
{
    public static class Constants
    {
        public static  string USER_SESSION = "USER_SESSION";
        public static int PRICE_TICKET = 65000;
        public static string EMAIL_SESSION = "EMAIL_SESSION";
        public static string CODE_VERIFY = "CODE_VERIFY";
        public static string ORDER = "ORDER";
        public static string DATE_NOW_STRING = "DATE_NOW_STRING";
        public static string LENGTH_SEAT = "LENGTH_SEAT";
        public static string FOR_GOT = "FOR_GOT";
        public static string FILL_OUT_ERROR = "Cần điền đầy đủ thông tin";
        public static string FORMAT_EMAIL_ERROR = "Cần nhập đúng định dạng email";
        public static string PASSWORD_ERROR = "Hai mật khẩu không trùng khớp";
        public static string EMAIL_EXIST = "Email đã tôn tại ";
        public static string EMAIL_NOT_EXIST = "Email không tôn tại ";
        public static string VERIFY_INCORRECT = "Mã xác thực không chính xác";
        public static string VERIFY_SUCCESS = "Xác thực thành công";
        public static string STATUS_ERROR = "ERROR";
        public static string STATUS_ERROR_ONE = "ERROR_ONE";
        public static string STATUS_SUCCESS = "SUCCESS";
        public static string STATUS_OK = "OK";
        public static string GET_CODE_VERIFY_SUCCESS = "Lấy lại mã thành công vui lòng check mail ";
        public static string SUBJECT_EMAIL = "Verify Account";
        public static string BODY_EMAIL = "This is code verify : ";
        public static string PASSWORD_AND_USERNAME_INCORRECT = "Tài khoản hoặc mật khẩu không chính xác";
        public static string CHOOSE_SCHEDULE = "Chọn lịch chiếu";
        public static string FILL_OUT_SCHEDULE = "Cần chọn lịch chiếu";
        public static string CHOOSE_SHOWTIME = "Chọn suất chiếu";
        public static string CHOOSE_ROOM = "Chọn phòng";
        public static string CHOOSE_SEAT = "Chọn ghế";
        public static string MSG_SUCCESS = "Thành Công";
        public static string MSG_ERROR = "Thất bại";
        public static string SUBJECT_EMAIL_QR = "QR về hóa đơn của quý khách";
        public static string BOOKINGTICKET_SUCCESS = "✅ Đặt vé thành công";
        public static string YOU_NEED_LOGIN = "❌ Bạn cần phải đăng nhập";
        public static string API_KEY = "sk_test_51Itn76AY7zpl2tqotBGt23IEZmOSCZOmOnpgAhVQWIvua4g5c4G74Au5P54rWqNofPUw1DZ7TdHzlBhCWJCJa81W00V76C7Z2n";
        public static string DESCRIPTION_BOOKING = "Phim hot 2021";
        public static string USD = "usd";
        public static string UPDATE_PASSWORD_SUCCESS = "Cập nhật mật khẩu thành công";
        public static string UPDATE_PASSWORD_ERROR = "Cập nhật mật khẩu thất bại";
        public static string UPDATE_INFORMATION_SUCCESS = "Cập nhật thông tin thành công";
        public static string YOU_LOGGED_OUT_SOMEWHERE_ELES = "Bạn đã bị đăng xuất ở nơi khác vui lòng reload lại trang";
        public static string SUCCESS_URL = "https://localhost:44313/payment/success";
        public static string CANCEL_URL = "https://localhost:44313/payment/error";
        public static string IMAGE_URL = "https://ci4.googleusercontent.com/proxy/HYuBnblArUYo9VwBr3QIRdi26frt7JG-rbJ2s5fYfFeNu8i_SVLXb4SQWPxsZNj-qVOmbFJT3Sy_XmSJQPmVCDPX3MFJCcE1ct5YGZ8O_E6vw-uL3Hr7vK22FOVrqgs=s0-d-e1-ft#https://i.pinimg.com/originals/fb/45/ba/fb45baac1eed3c1b19d4aad23b054fa8.jpg";
        public static string GOOGLE_URL = "https://chart.googleapis.com/chart?chs=300x300&cht=qr&chl=";
        public static string BOOKING_URL = "https://localhost:44313/booking/ticket/";
        public static string SUBJECT_EMAIL_FORGOT = "Quên mật khẩu";
        public static string BODY_EMAIL_FORGOT = "Mã quên mật khẩu";
        public static string FORMAT_DATE = "{0:yyyy-MM-dd}";
        public static string FORMAT_DATE_SCHEDULE = "{0:yyyyMMdd}";
        public static string PATH_SENDMAIL = "~/Content/Assets/html/sendQRMail.html";
        public static string FORMAT_DATE_STRING = "yyyyMMdd";
        public static string CHANGE_DATABASE = "❌ Ghế bạn chọn đã đặt bởi người khác vui lòng reload lại trang";
        public static string ERROR_SYSTEM = "Lỗi hệ thống";
        public static string ERROR_SYTEM_DETAIL = "Vui lòng liên hệ quản trị viên để được hổ trợ";
    }
}