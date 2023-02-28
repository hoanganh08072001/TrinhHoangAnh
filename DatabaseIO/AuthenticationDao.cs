using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Model;

namespace DatabaseIO
{
    
    public class AuthenticationDao
    {
        MyDB mydb = new MyDB();

        /**
         * md5 standard password encryption
         * @param password
         * @return
         */
        public string md5(string password)
        {
                MD5 md = MD5.Create();
                byte[] inputString = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] hash = md.ComputeHash(inputString);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i<hash.Length;i++) {
                    sb.Append(hash[i].ToString("x"));
                }
                return sb.ToString();
         }

        /**
         * Check email in database
         * @param email
         * @return
         */
        public bool checkEmail(string email)
        {
            var user = mydb.usercgvs.Where(u => u.email == email).FirstOrDefault();
            if (user != null) {
                return true;
            }
            return false;
        }

       /**
        * Check email and password in database
        * @param email
        * @param password
        * @return
        */
        public bool checklogin(string email,string password)
        {
            var result = mydb.usercgvs.Where(u => u.email == email && u.password == password && u.role_id ==3).FirstOrDefault();
            if (result != null) {
                return true;           
            } else {
                return false;
            }
        }
        /**
         * Check account user is working
         * @param email
         * @return
         */
        public bool checkActive(string email)
        {
            var result = mydb.usercgvs.Where(u => u.email == email).FirstOrDefault();
            if (result.is_active == 0) {
                return false;
            } else {
                return true;
            }
        }

        /**
         * User account registration
         * @param user
         */
        public void register(usercgv user)
        {
            string SQL = "INSERT INTO usercgv(email,is_active,password,phonenumber,role_id,username) VALUES('" + user.email + "',0,'" + user.password + "','" + user.phonenumber + "','" + user.role_id + "','" + user.username + "')";
            mydb.Database.ExecuteSqlCommand(SQL);
            mydb.SaveChanges();
        }

        /**
         * Check is it a number
         * @param value
         * @return
         */
        static bool IsNumeric(string value)
        {
            try {
                char[] chars = value.ToCharArray();
                foreach (char c in chars) {
                    if (char.IsNumber(c))
                        return true;
                }
                return false;
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }

        /**
         * check password strong
         * @param password
         * @return
         */
        public string checkPasswordStrong(string password)
        {
            string temp = "";
            bool check = IsNumeric(password);
            if (password.Length < 6) {
                return "Mật khẩu phải ít nhất 6 kí tự";
            }
            return temp;
        }

        /**
         * forgot password for user
         * @param password
         */
        public void forgotPassword(string email,string password)
        {
            var user = mydb.usercgvs.Where(u => u.email == email).FirstOrDefault();
            var passwordmd5 = md5(password);
            user.password = passwordmd5;
            mydb.SaveChanges();
        }
    }
}
