using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Model;

namespace DatabaseIO
{
    
    public class CategoryPostDao
    {
        MyDB mydb = new MyDB();

        /**
        * get list category post from database 
        * @return
        */
        public List<category_post> getAll()
        {
            try {
                return mydb.category_post.ToList();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }
        public void add(string name)
        {
            //insert data into table category_post
            string SQL = "INSERT INTO category_post(name) VALUES(N'" + name + "')";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
        public void update(string name, string id)
        {
            //update name by id in table category_post 
            string SQL = "UPDATE category_post SET name = N'" + name + "' WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
        public void delete(string id)
        {
            //delete categogy_post by id 
            string SQL = "DELETE FROM category_post WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }

        /**
         * check name category post exists in database 
         * @param name
         * @return
         */
        public bool checkName(string name)
        {
            string sql = "SELECT * FROM category_post WHERE name = N'" + name + "'";
            var user = mydb.Database.SqlQuery<category_post>(sql).FirstOrDefault();
            if (user != null) {
                return true;
            }
            return false;
        }

        /**
        * check data changes before updating
        * @param id
        * @param name
        * @return
        */
        public bool checkUpdate(int id, string name)
        {
            string sql = "SELECT * FROM category_post WHERE name = N'" + name + "' and id = @id";
            var user = mydb.Database.SqlQuery<category_post>(sql, new SqlParameter("@id", id)).FirstOrDefault();
            if (user != null) {
                return true;
            }
            return false;
        }

        /**
         * Check post is working
         * @param id
         * @return
         */
        public bool checkActive(int id)
        {
            string sql = "SELECT * FROM post WHERE id_cpost = @id";
            List<post> user = mydb.Database.SqlQuery<post>(sql, new SqlParameter("@id", id)).ToList();
            if (user.Count == 0){
                return true;
            }
            return false;
        }
    }
}
