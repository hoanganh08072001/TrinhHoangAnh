using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Model;

namespace DatabaseIO
{
    
    public class CommentDao
    {
        MyDB mydb = new MyDB();

        /**
         * get list comment by id from database 
         * @return
         */
        public List<rating> getCommentById(int id)
        {
            try {
                return mydb.ratings.Where(r => r.film_id == id).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }     
        }

        /**
         * get object comment by id from database 
         * @return
         */
        public rating getObjectCommentById(int id)
        {
            try {
                return mydb.ratings.Where(r => r.id == id).FirstOrDefault();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }          
        }
        public void comment(rating rating)
        {
            // insert comment into table rattings for user
            string SQL = "INSERT INTO ratings( film_id, rate,id_user,name_user,number_start) VALUES (@filmId,@rate,@userId,@nameuser,@number)";
            mydb.Database.ExecuteSqlCommand(SQL, new SqlParameter("@filmId", rating.film_id),
                new SqlParameter("@rate", rating.rate),
                new SqlParameter("@userId", rating.id_user),
                new SqlParameter("@number", rating.number_start),
                new SqlParameter("@nameuser", rating.name_user)
            );
        }

        /**
         * delete comment by id from database 
         */
        public void deleteComment(int id)
        {
            //get object comment by id from database
            var ObjectM = getObjectCommentById(id);
            mydb.ratings.Remove(ObjectM);
            mydb.SaveChanges();
        }

        /**
         * add list object CommentAjax
         * @param listcomment
         * @return
         */
        public List<CommentAjax> addCommentAjax(List<rating> listComment)
        {
            var listAjax = new List<CommentAjax>();
            CommentAjax commentA = null;
            int lengthComment = listComment.Count;
            for (int i = 0; i <lengthComment; ++i) {
                commentA = new CommentAjax();
                commentA.id = listComment[i].id;
                commentA.film_id = listComment[i].film_id;
                commentA.id_user = listComment[i].id_user;
                commentA.number_start = listComment[i].number_start;
                commentA.rate = listComment[i].rate;
                commentA.name_user = listComment[i].name_user;
                commentA.created_time = listComment[i].created_time.ToString();
                listAjax.Add(commentA);
            }
            return listAjax;
        }
    }
}
