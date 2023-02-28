using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGV.Utils
{
    public class ValidateUtils
    {
        /**
         * check format email for user 
         * @param string email
         * @return
         */
        public bool checkFormatEmail(string email)
        {
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!rEmail.IsMatch(email)) {
                return true;
            } else {
                return false;
            }
        }
    }
}