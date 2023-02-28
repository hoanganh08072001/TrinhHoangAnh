using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Model;

namespace DatabaseIO
{
    
    public class ShowtimeDao
    {
        MyDB mydb = new MyDB();

        /**
         * get list showtime from database 
         * @param idRoom
         * @param id
         * @return
         */
        public List<showtime> getShowtime(string id, int idRoom)
        {
            try
            {
                int scheduleId = Int32.Parse(id);
                return mydb.Database.SqlQuery<showtime>("SELECT s.* FROM showtimes s, schedules sc WHERE s.schedule_id = '" + scheduleId + "' and s.schedule_id = sc.id and CONVERT(varchar, sc.dateschedule, 101) = CONVERT(varchar, getdate(), 101) and s.id_room = '" + idRoom + "' and s.start_time >= convert(varchar(32),getdate(),108)").ToList();
            } catch (Exception ex){
                Console.WriteLine(ex.Message);
                return null;
            }          
        }
        public List<showtime> getShowtimes(string id, int idRoom)
        {
            try {
                int scheduleId = Int32.Parse(id);
                return mydb.Database.SqlQuery<showtime>("select * FROM showtimes WHERE schedule_id = '" + scheduleId + "' and id_room = '" + idRoom + "'").ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }          
        }

        /**
         * get object showtime by id from database 
         * @param id
         * @return
         */
        public showtime getName(int id)
        {
            try {
                return mydb.showtimes.Where(s => s.id == id).FirstOrDefault();
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }       
        }

        /**
         * get list showtime from database 
         * @return
         */
        public List<showtime> getAll()
        {
            try {
                return mydb.showtimes.ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }
        public void add(string scheid, string start, string end, int idRoom)
        {
            //insert data into table showtimes
            string SQL = "INSERT INTO showtimes(schedule_id,start_time,end_time,id_room) VALUES('" + scheid + "','" + start + "','" + end + "','"+idRoom+"')";
            mydb.Database.ExecuteSqlCommand(SQL);

        }
        public void update(string start, string end, string id)
        {
            //update start_time , end_time in showtimes by id
            string SQL = "UPDATE showtimes SET start_time = '" + start + "', end_time = '" + end + "' WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
        public void delete(string id)
        {
            //delete from showtimes by id
            string SQL = "DELETE FROM showtimes WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }

        /**
         * get list showtime by idRoom from database 
         * @param idRoom
         * @param start
         * @param id
         * @return
         */
        public List<showtime> checkShow(string start,string id,string idRoom)
        {
            try {
                string sql = "Select * From showtimes where schedule_id = @idsche  AND id_room = @idroom  AND (@start BETWEEN start_time AND end_time OR (DATEDIFF(minute, @start, start_time) <= 120 AND DATEDIFF(minute, @start, start_time) >= 0 ))";
                return mydb.Database.SqlQuery<showtime>(sql, new SqlParameter("@start", start), new SqlParameter("@idsche", id), new SqlParameter("@idroom", idRoom)).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }         
        }

        /**
         * get object room by idSchedule from database 
         * @param idSchedule
         * @return
         */
        public List<room> getNameRoom(int idSchedule)
        {
            try {
                string sql = "select r.* from scheduleroom s, room r where  s.id_schedule = '" + idSchedule + "' and r.id = s.id_room";
                return mydb.Database.SqlQuery<room>(sql).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }         
        }

        /**
         * Check showtime is working in database
         * @param idShowtime
         * @return
         */
        public bool checkActive(int idShowtime)
        {
            string sql = "select * from booking where showtime_id = '" + idShowtime + "'";
            var showtimeOBj = mydb.Database.SqlQuery<booking>(sql).FirstOrDefault();
            if (showtimeOBj != null) {
                return true;
            }
            return false;
        }

        /**
         * Check showtime is exist in database
         * @param showthime_id
         * @return
         */
        public bool checkExistShowtime(int showthime_id)
        {
            var showtime = mydb.showtimes.Where(s => s.id == showthime_id).FirstOrDefault();
            if (showtime != null) {
                return true;
            } else {
                return false;
            }
        }
    }
}
