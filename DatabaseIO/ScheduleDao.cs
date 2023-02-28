using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace DatabaseIO
{
    
    public class ScheduleDao
    {
        MyDB mydb = new MyDB();

        /**
         * get list schedule by id from database 
         * @param id
         * @return
         */
        public List<schedule> getSchedule(string id)
        {
            try {
                int filmId = Int32.Parse(id);
                string SQL = "Select * from schedules where film_id = '" + id + "' and YEAR(dateschedule) >= YEAR(getdate()) and MONTH(dateschedule) >= MONTH(getdate()) and DAY(dateschedule) >= DAY(getdate())";
                return mydb.Database.SqlQuery<schedule>(SQL).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }          
        }

        /**
         * get object schedule by id from database 
         * @param id
         * @return
         */
        public schedule getName(int id)
        {
            try {
                return mydb.schedules.Where(s => s.id == id).FirstOrDefault();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }          
        }

        /**
         * get list schedule from database 
         * @return
         */
        public List<schedule> getAll()
        {
            try {
                return mydb.schedules.ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }
        public void add(string filmid, DateTime dateSchedule)
        {
            //insert data into table scheudles
            string SQL = "INSERT INTO schedules(film_id,dateschedule) VALUES('" + filmid + "','" + dateSchedule + "')";
            mydb.Database.ExecuteSqlCommand(SQL);

        }
        public void update(string filmid, DateTime dateSchedule, string id)
        {
            //update filmId , dateScheudle by id
            string SQL = "UPDATE schedules SET film_id = '" + filmid + "', dateschedule = '" + dateSchedule + "' WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
        public void delete(string id)
        {
            //delete data from schedules by id
            string SQL = "DELETE FROM schedules WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }

        /**
         * check name film exists in database 
         * @param datesche
         * @param id
         * @return
         */
        public bool checkName(DateTime datesche,int id)
        {
            var user = mydb.schedules.Where(u => u.dateschedule == datesche && u.film_id == id).FirstOrDefault();
            if (user != null){
                return true;
            }
            return false;
        }

        /**
         * get last id schedule from database
         * @return
         */
        public int getSCheduleByDate()
        {
            return Int32.Parse(mydb.schedules.ToList().Last().id.ToString());
        }
        public void addScheduleRoom(int id_schedule, int id_room)
        {
            //insert data into table scheduleroom 
            string sql = "insert into scheduleroom(id_schedule,id_room) values('" + id_schedule + "','" + id_room + "')";
            mydb.Database.ExecuteSqlCommand(sql);
        }

        /**
         * check schedule is working in scheduleroom 
         * @param datesche
         * @param idRoom
         * @param idFilm
         * @return
         */
        public bool checkScheduleRoom(DateTime datesche, int idRoom,int idFilm)
        {
            string sql = "select a.* from schedules a,scheduleroom b, booking c where  a.id = b.id_schedule and a.dateschedule = '"+datesche+"' and b.id_room = '"+idRoom+"' and c.film_id = '"+idFilm+"'";
            var scheduleR = mydb.Database.SqlQuery<schedule>(sql).FirstOrDefault();
            if (scheduleR != null) {
                return true;
            }
            return false;
        }

        /**
         * check data changes before updating
         * @param idRoom
         * @param idFilm
         * @param datesche
         * @return
         */
        public bool checkScheduleRoomUpdate(DateTime datesche, int idRoom,int idFilm)
        {
            string sql = "select a.* from schedules a,scheduleroom b where  a.id = b.id_schedule and a.dateschedule = '" + datesche + "' and b.id_room = '" + idRoom + "' and a.film_id = '"+idFilm+"'";
            var scheduleR = mydb.Database.SqlQuery<schedule>(sql).FirstOrDefault();
            if (scheduleR != null) {
                return true;
            }
            return false;
        }

        /**
         * get object room by idSchedule
         * @param idSchedule
         * @return
         */
        public room getNameRoom(int idSchedule)
        {
            string sql = "select r.* from scheduleroom s, room r where  s.id_schedule = '"+idSchedule+"' and r.id = s.id_room";
            return mydb.Database.SqlQuery<room>(sql).FirstOrDefault();
        }

        public void deleteScheduleRoom(int id_schedule)
        {
            //delete from scheduleroom by idSchedule
            string sql = "delete from scheduleroom where id_schedule = '" + id_schedule + "'";
            mydb.Database.ExecuteSqlCommand(sql);
        }

        /**
         * check showtimes is working from database
         * @param idSchedule
         * @param id_room
         * @return
         */
        public bool checkShowtimeActive(int idSchedule , int id_room)
        {
            string sql = "select st.* from showtimes st, scheduleroom sc where  st.schedule_id = sc.id_schedule and st.id_room = sc.id_room and st.schedule_id ='" + idSchedule + "' and st.id_room ='" + id_room + "'";
            string sql1 = "select * from booking where schedule_id = '" + idSchedule + "' and room_id = '" + id_room + "'";
            var check = mydb.Database.SqlQuery<showtime>(sql).FirstOrDefault();
            var checkObj = mydb.Database.SqlQuery<booking>(sql1).FirstOrDefault();
            if (check != null || checkObj != null) {
                return true;
            } else {
                return false;
            }
        }

        /**
         * edit room showtime by idSchedule from database
         * @param idSchedule
         * @param id_room
         */
        public void editRoomShowTime(int idSchedule,int idRoom)
        {
            var showtime = mydb.showtimes.Where(s => s.schedule_id == idSchedule).FirstOrDefault();
            showtime.id_room = idRoom;
            mydb.SaveChanges();
        }
    }
}
