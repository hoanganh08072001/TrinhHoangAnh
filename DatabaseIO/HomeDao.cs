using System;
using System.Collections.Generic;
using System.Linq;
using Model;


namespace DatabaseIO
{
    
    public class HomeDao
    {
        MyDB mydb = new MyDB();

        /**
         * get list film comming soon from database 
         * @return
         */
        public List<film> getFilmComingSoon()
        {
            try {
                var today = DateTime.Now;
                return mydb.films.Where(x => x.premiere_date > today).OrderBy(x => x.premiere_date).ToList();
                //return mydb.Database.SqlQuery<film>("SELECT * FROM films WHERE CONVERT(varchar, premiere_date, 101) > CONVERT(varchar, getdate(), 101)").ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }       
        }

        /**
         * get list film now showing from database 
         * @return
         */
        public List<film> getFilmNowShowing()
        {
            try {
                var today = DateTime.Now;
                var dueday = today.AddDays(7);
                var idfilms = mydb.schedules.Where(x => x.dateschedule >= today && x.dateschedule <= dueday).Select(x => x.film_id).Distinct().ToList();
                return mydb.films.Where(x => idfilms.Contains(x.id)).ToList();
                /*return mydb.Database.SqlQuery<film>("SELECT * FROM films WHERE CONVERT(varchar, premiere_date, 101) <= CONVERT(varchar, getdate(), 101)").ToList();*/
                // return mydb.Database.SqlQuery<film>("SELECT * FROM films WHERE id in (Select film_id From schedules Where CONVERT(varchar, dateschedule, 101) = CONVERT(varchar, getdate(), 101) )").ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }        
        }

        /**
         * get list introduce from database 
         * @return
         */
        public List<post> getPromotion()
        {
            try {
                return mydb.posts.Where(p => p.id_cpost == 1).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }         
        }

        /**
         * get sum amount from database 
         * @param month
         * @return
         */
        public int statictis(int month)
        {
            string SQL = "Select SUM(a.amount)  FROM booking as a,schedules as b WHERE a.schedule_id = b.id GROUP BY  MONTH(b.dateschedule) HAVING MONTH(b.dateschedule) = '" + month + "'";
            int result = mydb.Database.SqlQuery<int>(SQL).FirstOrDefault();
            return result;
        }

        /**
         * get number ticket by month from database 
         * @param month
         * @return
         */
        public int countTicket(int month)
        {
            string SQL = "Select COUNT(a.id)  FROM booking as a,schedules as b WHERE a.schedule_id = b.id GROUP BY  MONTH(b.dateschedule) HAVING MONTH(b.dateschedule) = '" + month + "'";
            int result = mydb.Database.SqlQuery<int>(SQL).FirstOrDefault();
            return result;
        }

        /**
         * get list films now from database 
         * @return
         */
        public List<film> getFilmNow()
        {
            try {
                var today = DateTime.Now;
                //return mydb.films.Where(x => x.premiere_date > today).ToList();
                return mydb.Database.SqlQuery<film>("SELECT DISTINCT  a.* FROM films as a,schedules as b, showtimes as c WHERE b.film_id = a.id AND c.schedule_id = b.id AND FORMAT(b.dateschedule, 'dd/MM/yyyy' ) = FORMAT(getdate(), 'dd/MM/yyyy' ) and c.start_time >= convert(varchar(32),getdate(),108)").ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }           
        }
    }
}
