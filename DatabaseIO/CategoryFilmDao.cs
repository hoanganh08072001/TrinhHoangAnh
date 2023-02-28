using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Model;

namespace DatabaseIO
{
    
    public class CategoryFilmDao
    {
        MyDB mydb = new MyDB();

        /**
         * get list category film from database 
         * @return
         */
        public List<category_film> getAllCategoryFilm()
        {
            return mydb.category_film.ToList();
        }
        public void add(string name)
        {
            //insert data into table category_film
            string SQL = "INSERT INTO category_film(name) VALUES(N'" + name + "')";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
        public void update(string name, string id)
        {
            //update name by id in table category_film 
            string SQL = "UPDATE category_film SET name = N'" + name + "' WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
        public void delete(string id)
        {
            //delete categogy_film by id 
            string SQL = "DELETE FROM category_film WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }

        /**
         * check name category film exists in database 
         * @param name
         * @return
         */
        public bool checkName(string name)
        {
            string namef = "%" + name + "%";
            string sql = "SELECT * FROM category_film WHERE name LIKE @name";
            var user = mydb.Database.SqlQuery<category_film>(sql,new SqlParameter("@name",namef)).FirstOrDefault();
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
            //select all form category_film by name and id
            string sql = "SELECT * FROM category_film WHERE name = N'" + name + "' and id = @id";
            var user = mydb.Database.SqlQuery<category_film>(sql, new SqlParameter("@id", id)).FirstOrDefault();
            if (user != null) {
                return true;
            }
            return false;
        }

        /**
         * Check film is working
         * @param id
         * @return
         */
        public bool checkActive(int id)
        {
            string sql = "SELECT * FROM films WHERE id_cfilm = @id";
            List<film> user = mydb.Database.SqlQuery<film>(sql, new SqlParameter("@id", id)).ToList();
            if (user.Count == 0) {
                return true;
            }
            return false;
        }
    }
}
