using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Model;

namespace DatabaseIO
{
    
    public class SeatDao
    {
        MyDB mydb = new MyDB();

        /**
         * get list seat not active from database 
         * @param roomId
         * @param showtimeId
         * @param filmId
         * @param scheduleId
         * @return
         */
        public List<seat> getSeat(int roomId, int showtimeId, int filmId, int scheduleId)
        {
            try {
                return mydb.Database.SqlQuery<seat>("SELECT * from seats WHERE id NOT IN (SELECT seat_id FROM booking WHERE room_id = @rId " +
               "and showtime_id = @sId and film_id = @fId and schedule_id = @scheId)",
               new SqlParameter("@rId", roomId),
               new SqlParameter("@sId", showtimeId),
               new SqlParameter("@fId", filmId),
               new SqlParameter("@scheId", scheduleId)
               ).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }         
        }

        /**
         * get list seat done from database 
         * @param roomId
         * @param showtimeId
         * @param filmId
         * @param scheduleId
         * @return
         */
        public List<seat> getSeatDone(int roomId, int showtimeId, int filmId, int scheduleId)
        {
            try {
                return mydb.Database.SqlQuery<seat>("SELECT * from seats WHERE id IN (SELECT rs.id_seat FROM roomseat rs WHERE rs.id_room = @idRoom )" +
               "and  id NOT IN (SELECT seat_id FROM booking WHERE room_id = @idRoom  and showtime_id = @sId and film_id = @fId and schedule_id = @scheId)",
               new SqlParameter("@idRoom", roomId),
               new SqlParameter("@sId", showtimeId),
               new SqlParameter("@fId", filmId),
               new SqlParameter("@scheId", scheduleId)
               ).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
          
        }
        public seat getName(int id)
        {
            try {
                return mydb.seats.Where(s => s.id == id).FirstOrDefault();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }

        /**
         * get list seat from database       
         * @return
         */
        public List<seat> getAll()
        {
            try {
                return mydb.seats.ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }

        /**
         * get list seatRoom from database  
         * @param id
         * @return
         */
        public List<seat> getSeatRoom(int id)
        {
            try {
                String sql = "select * from seats s, roomseat rs where s.id = rs.id_seat and id_room = @idRoom  ";
                return mydb.Database.SqlQuery<seat>(sql, new SqlParameter("@idRoom ", id)).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
        public void add(string name)
        {
            //insert data into table seats 
            string SQL = "INSERT INTO seats(seat_name) VALUES(N'" + name + "')";
            mydb.Database.ExecuteSqlCommand(SQL);

        }
      
        public void update(string name, string id)
        {
            //update seat_name by id
            string SQL = "UPDATE seats SET seat_name = N'" + name + "' WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
        public void delete(string id)
        {
            //delete data from seats
            string SQL = "DELETE FROM seats WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }

        /**
         * check name seat exists in database 
         * @param name
         * @return
         */
        public bool checkName(string name)
        {
            var user = mydb.seats.Where(u => u.seat_name == name).FirstOrDefault();
            if (user != null) {
                return true;
            }
            return false; ;
        }

        /**
         * Check seat is working in database
         * @param id
         * @return
         */
        public bool checkActive(int id)
        {
            string sql = "select * from roomseat where id_seat = '" + id + "'";
            var roomseat = mydb.Database.SqlQuery<roomseat>(sql).FirstOrDefault();
            if (roomseat != null) {
                return true;
            } else {
                return false;
            }
        }
    }
}
