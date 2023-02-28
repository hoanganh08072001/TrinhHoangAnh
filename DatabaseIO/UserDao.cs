using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Model;


namespace DatabaseIO
{
    
    public class UserDao
    {
        MyDB mydb = new MyDB();

        /**
         * get object usercgv by email from database
         * @param email
         * @return
         */
        public usercgv getInformation(string email)
        {
            try {
                return mydb.usercgvs.Where(u => u.email == email).FirstOrDefault();
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }         
        }

        /**
         * get object usercgv by email and password from database
         * @param email
         * @return
         */
        public usercgv getUpdateProfile(string email, string password)
        {
            try{
                return mydb.usercgvs.Where(u => u.email == email && u.password == password).FirstOrDefault();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }         
        }

        /**
         * update password for user 
         * @param email
         * @param password
         * @param passwordNew
         */
        public void updatePassword(string email, string password, string passwordNew)
        {
            usercgv u = getUpdateProfile(email, password);
            u.password = passwordNew;
            mydb.SaveChanges();
        }

        /**
         * active account for user 
         * @param email
         */
        public void activeAccount(string email)
        {
            usercgv u = getInformation(email);
            u.is_active = 1;
            mydb.SaveChanges();
        }

        /**
         * update profile for user 
         * @param email
         * @param user
         */
        public void updateProfile(string email, usercgv user)
        {
            usercgv usercgv = getInformation(email);
            usercgv.phonenumber = user.phonenumber;
            usercgv.username = user.username;
            mydb.SaveChanges();
        }

        /**
         * get list usercgv from database
         * @return
         */
        public List<usercgv> getAll()
        {
            try {
                return mydb.usercgvs.ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }        
        }
        public void add(string email, string password, string phonenumber, string role_id, string username, string tt)
        {
            //insert data into usercgv
            string SQL = "INSERT INTO usercgv(email,is_active,password,phonenumber,role_id,username) VALUES('" + email + "','" + tt + "','" + password + "','" + phonenumber + "','" + role_id + "',N'" + username + "')";
            mydb.Database.ExecuteSqlCommand(SQL);

        }
        public void update(string email, string password, string phonenumber, string role_id, string username, string id, string tt)
        {
            //update email, password, phonenumber, username  from usercgv
            string SQL = "UPDATE usercgv SET email = '" + email + "',password = '" + password + "', phonenumber = '" + phonenumber + "', role_id = '" + role_id + "', username = N'" + username + "', is_active = '" + tt + "' WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
        public void delete(int id)
        {
            //delete from usercgv
            string SQL = "DELETE FROM usercgv WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }

        /**
         * change status for user
         * @param id
         */
        public void changStatus(int id)
        {
            var user = mydb.usercgvs.Where((u) => u.id == id).FirstOrDefault();
            user.is_active = user.is_active == 1 ? user.is_active = 0 : user.is_active = 1;
            mydb.SaveChanges();
        }

        /**
         * Check user is working in database
         * @param id
         * @return
        */
        public bool checkActive(int id)
        {
            string sql = "SELECT * FROM ratings WHERE id_user = @id";
            List<rating> user = mydb.Database.SqlQuery<rating>(sql, new SqlParameter("@id", id)).ToList();
            string sqlb = "SELECT * FROM booking WHERE id_user = @id";
            List<booking> userb = mydb.Database.SqlQuery<booking>(sqlb, new SqlParameter("@id", id)).ToList();
            if (user.Count != 0 || userb.Count != 0) {
                return true;
            }
            return false; ;
        }
    }
}