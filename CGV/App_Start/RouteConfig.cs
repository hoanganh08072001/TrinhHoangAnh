using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CGV
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "home user",
               url: "user/home",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "Detail film",
              url: "film/detail/{id}",
              defaults: new { controller = "Film", action = "DetailFilm", id = UrlParameter.Optional }
          );
            routes.MapRoute(
            name: "Detail promotion",
            url: "post/detailpromotion/{id}",
            defaults: new { controller = "Post", action = "DetailPromotion", id = UrlParameter.Optional }
        );
            routes.MapRoute(
            name: "Get code",
            url: "authentication/getcode/{email}",
            defaults: new { controller = "Authentication", action = "getCodeAgain", email = UrlParameter.Optional }
        );
          
            routes.MapRoute(
             name: "Search film",
             url: "film/search/{keySearch}",
             defaults: new { controller = "Film", action = "SearchFilm", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "qr code",
             url: "booking/ticket/{dateNow}/{id}",
             defaults: new { controller = "Film", action = "qrResult", id = UrlParameter.Optional }
         );
            routes.MapRoute(
            name: "history",
            url: "user/history/{id}",
            defaults: new { controller = "Film", action = "HistoryBooking", id = UrlParameter.Optional }
        );
            routes.MapRoute(
           name: "paymentsuccess",
           url: "payment/success",
           defaults: new { controller = "Film", action = "paymentSuccess", id = UrlParameter.Optional }
       );
            routes.MapRoute(
           name: "paymenterror",
           url: "payment/error",
           defaults: new { controller = "Film", action = "paymentError", id = UrlParameter.Optional }
       );
            routes.MapRoute(
             name: "Admin Home",
             url: "admin/home",
             defaults: new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "Admin Category Film",
             url: "admin/loaiphim",
             defaults: new { controller = "AdminCategoryFilm", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "Admin Category Post",
             url: "admin/loaibv",
             defaults: new { controller = "AdminCategoryPost", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "Admin Film",
             url: "admin/film",
             defaults: new { controller = "AdminFilm", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
            name: "Admin Trash Film",
            url: "admin/filmtrash",
            defaults: new { controller = "AdminFilm", action = "Trash", id = UrlParameter.Optional }
        );
            routes.MapRoute(
            name: "Admin Trash Room",
            url: "admin/roomtrash",
            defaults: new { controller = "AdminRoom", action = "Trash", id = UrlParameter.Optional }
        );
            routes.MapRoute(
             name: "Admin Schedule",
             url: "admin/lichchieu",
             defaults: new { controller = "AdminSchedule", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "Admin Showtime",
             url: "admin/suatchieu",
             defaults: new { controller = "AdminShowtime", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "Admin Room",
             url: "admin/room",
             defaults: new { controller = "AdminRoom", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
            name: "Admin Seat",
            url: "admin/ghe",
            defaults: new { controller = "AdminSeat", action = "Index", id = UrlParameter.Optional }
        );
            routes.MapRoute(
           name: "Admin Post",
           url: "admin/baiviet",
           defaults: new { controller = "AdminPost", action = "Index", id = UrlParameter.Optional }
       );
            routes.MapRoute(
             name: "Admin user",
             url: "admin/user",
             defaults: new { controller = "AdminUser", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "Admin Booking",
             url: "admin/booking",
             defaults: new { controller = "AdminBooking", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
            name: "Admin Login",
            url: "admin/login",
            defaults: new { controller = "AdminAuthen", action = "Index", id = UrlParameter.Optional }
        );
            routes.MapRoute(
         name: "Admin Book Ticket",
         url: "admin/bookticket",
         defaults: new { controller = "AdminBookTicket", action = "Index", id = UrlParameter.Optional }
     );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "IndexUser", id = UrlParameter.Optional }
            );
          
        }
    }
}
