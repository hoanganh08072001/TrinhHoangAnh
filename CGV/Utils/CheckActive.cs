using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGV.Utils
{
    public static class CheckActive
    {
        public static void checkActivePost()
        {
            HttpContext.Current.Session.Remove("checkactive");
            HttpContext.Current.Session.Remove("activeParent");
        }
        public static void checkActiveParent()
        {
            HttpContext.Current.Session.Remove("checkactive");
            HttpContext.Current.Session.Remove("checkactivepost");
        }
        public static void checkActive()
        {
            HttpContext.Current.Session.Remove("activeParent");
            HttpContext.Current.Session.Remove("checkactivepost");
        }
    }
}