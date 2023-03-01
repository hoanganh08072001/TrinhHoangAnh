using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Model;

namespace DatabaseIO
{
    
    public class FilmDao
    {
        MyDB mydb = new MyDB();
        RoomDao roomD = new RoomDao();

        /**
         * get detail film by id from database 
         * @return
         */
        public film getDetailFilm(string id)
        {
            try {
                int filmId = Int32.Parse(id);
                return mydb.films.Where(f => f.id == filmId).FirstOrDefault();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }      
        }

        /**
         * get search film by key word from database 
         * @param keySearch
         * @return
         */
        public IEnumerable<film> searchFilm(string keySearch)
        {
            try
            {
                string nameSearch = "%" + keySearch + "%";
                // selecet all films from table films by film_name or actor or director
                string sql = "select * from films WHERE film_name LIKE @keysearch or actor LIKE @keysearch or director LIKE @keysearch ";
                return mydb.Database.SqlQuery<film>(sql, new SqlParameter("@keysearch", nameSearch)).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }        
        }

        /**
         * booking ticket for user
         * @param book
         * @param createTime
         */
        public void bookingTicket(booking book, string createTime)
        {
            //insert data into table booking 
            string SQL = "INSERT INTO booking(id_user, film_id, schedule_id, showtime_id, room_id, seat_id, status,amount,create_time) " +
                "VALUES (@userId,@filmId,@scheduleId,@showtimeId,@roomId,@seatId,@status,@amount,@createTime)";
            mydb.Database.ExecuteSqlCommand(SQL, new SqlParameter("@userId", book.id_user),
                new SqlParameter("@filmId", book.film_id),
                new SqlParameter("@scheduleId", book.schedule_id),
                new SqlParameter("@showtimeId", book.showtime_id),
                new SqlParameter("@roomId", book.room_id),
                new SqlParameter("@status", book.status),
                new SqlParameter("@seatId", book.seat_id),
                new SqlParameter("@amount", book.amount),
                 new SqlParameter("@createTime", createTime)
                );

        }

        /**
         * get list order booking
         * @param datenow
         * @param id
         * @return
         */
        public List<booking> getOrder(string datenow, int id)
        {
            try {
                return mydb.bookings.Where(b => b.create_time == datenow && b.id_user == id).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
          
        }

        /**
         * get object films by id from database
         * @param id
         * @return
         */
        public film getName(int id)
        {
            try {
                return mydb.films.Where(f => f.id == id).FirstOrDefault();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
          
        }

        /**
         * get list bookings by id  sort des from database
         * @param id
         * @return
         */
        public List<booking> getBooking(int id)
        {
            try {
                return mydb.bookings.Where(b => b.id_user == id).OrderByDescending(b => b.id).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }       
        }

        /**
         * get all list films from database
         * @param id
         * @return
         */
        public List<film> getAll()
        {
            try {
                return mydb.films.ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }
        public void add(string description, string director, string actor, string duration, string film_name, string image, string trailer, string idcfilm, string ngaycc)
        {
            //insert data into table films in database
            string SQL = "INSERT INTO films(description, director, actor, duration, film_name, image, trailer, id_cfilm, premiere_date ) VALUES(N'" + description + "',N'" + director + "',N'" + actor + "',N'" + duration + "',N'" + film_name + "','" + image + "',N'" + trailer + "','" + idcfilm + "','" + ngaycc + "' )";
            mydb.Database.ExecuteSqlCommand(SQL);

        }
        public void update(string description, string director, string actor, string duration, string film_name, string image, string trailer, string idcfilm, string id, string ngaycc)
        {
            //update description from table films 
            string SQL = "UPDATE films SET description = N'" + description + "',director = N'" + director + "',actor = N'" + actor + "',duration = N'" + duration + "',film_name = N'" + film_name + "',image = '" + image + "',trailer = N'" + trailer + "',id_cfilm= '" + idcfilm + "', premiere_date = '" + ngaycc + "'  WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
        public void delete(string id)
        {
            //delete film from table films by id
            string SQL = "DELETE FROM films WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }

        /**
         * check name film exists in database 
         * @param name
         * @return
         */
        public bool checkName(string name)
        {
            //select all films from table films with film_name equal name
            string sql = "SELECT * FROM films WHERE film_name = N'" + name + "'";
            var user = mydb.Database.SqlQuery<film>(sql).FirstOrDefault();
            if (user != null) {
                return true;
            }
            return false;
        }


        /**
         * Check schedules is working
         * @param id
         * @return
         */
        public bool checkActive(int id)
        {
            string sql = "SELECT * FROM schedules WHERE film_id = @id";
            List<schedule> user = mydb.Database.SqlQuery<schedule>(sql, new SqlParameter("@id", id)).ToList();
            if (user.Count == 0) {
                return true;
            }
            return false;
        }


        /**
         * Handle get seat from user
         * @param listSeatRoom
         * @param listSeat
         * @return
         */
        public List<SeatActive> handleGetSeat(List<seat> listSeatRoom, List<seat> listSeat)
        {
           
            int lengthArrSeatRoom = listSeatRoom.Count;
            var listSeatActive = new List<SeatActive>();
            for (int i = 0; i < lengthArrSeatRoom; i++) {
                SeatActive sa = new SeatActive();
                sa.id = listSeatRoom[i].id;
                sa.seat_name = listSeatRoom[i].seat_name;
                sa.active = 0;
                listSeatActive.Add(sa);
            }
            int lengthArrSeatactive = listSeatActive.Count;
            for (int k = 0; k < lengthArrSeatactive; k++) {
                for (int j = 0; j < listSeat.Count; j++) {
                    if (listSeatActive[k].id == listSeat[j].id) {
                        listSeatActive[k].active = 1;
                    }
                }
            }
            return listSeatActive;
        }

        /**
         * Handle render seat for ajax 
         * @param listSeatActive
         * @return
         */
        public string renderSeat(List<SeatActive> listSeatActive)
        {
            string html = "";
            int lengthArrSeatactive = listSeatActive.Count;
            for (int j = 0; j < lengthArrSeatactive; j++) {
                var nameId = "in" + listSeatActive[j].id;
                var nameIdDiv = "div" + listSeatActive[j].id;
                var nameIdSeat = "id" + listSeatActive[j].id;
                var check = "false";
                if (listSeatActive[j].active == 0) {
                    html += "<div style='display: flex; align-content: center; justify-content: center; justify-items: center; border: 1px solid; background-color:#DDDDDD;line-height: 50px; height: 50px; width: 104.2px; margin-right: 6.4px; margin-bottom: 6.4px '><p>🪑" + listSeatActive[j].seat_name + "</p></div>";
                } else {
                    html += "<div id='" + nameIdDiv + "' onclick=onChoose(" + listSeatActive[j].id + ") style='display: flex; align-content: center; justify-content: center;justify-items: center; border: 1px solid red; line-height: 50px; height: 50px; width: 104.2px; margin-right: 6.4px; margin-bottom: 6.4px '><p>🪑" + listSeatActive[j].seat_name + "</p><input type='hidden' value='" + check + "' id='" + nameId + "'/><input type='hidden' value='" + listSeatActive[j].id + "' id='" + nameIdSeat + "' /></div>";
                }
            }
            return html;
        }

        /**
        * Handle render room for ajax 
        * @param listRoom
        * @return
        */
        public string renderRoom(List<room> listRoom)
        {
            string html = " <option> Chọn phòng </option>";
            foreach (var item in listRoom) {
                html += "<option value=" + item.id + ">" + item.room_name + "</option>";
            }
            return html;
        }

        /**
        * Handle render showtime for ajax 
        * @param listShowtime
        * @return
        */
        public string renderShowtime(List<showtime> listShowtime)
        {
            string html = " <option>Chọn suất chiếu</option>";
            foreach (var item in listShowtime) {
                string showtimeString = item.start_time + " - " + item.end_time;
                html += "<option value=" + item.id + ">" + showtimeString + "</option>";
            }
            return html;
        }

        /**
         * Handle render schedule for ajax 
         * @param list
         * @return
         */
        public string renderSchedule(List<schedule> list)
        {
            string html = "<option value=" + 0 + ">Chọn lịch chiếu</option>";
            foreach (var item in list) {
                //get schedule by id from database
                var roomOb = roomD.getNameRoomSchedule(item.id);
                string dateschedule = String.Format("{0:yyyy-MM-dd}", item.dateschedule);
                html += "<option value=" + item.id + ">" + dateschedule + "" + "(" + roomOb.room_name + ")" + "</option>";
            }
            return html;
        }
        /**
         * change status for film
         * @param id
         */
        public void changStatus(int id)
        {
            var film = mydb.films.Where((u) => u.id == id).FirstOrDefault();
            film.trash = film.trash == 1 ? 0 :  1;
            mydb.SaveChanges();
        }

        public List<film> getByCategory(int id)
        {
            try
            {
                return mydb.films.Where(f => f.id_cfilm == id).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

    }
}
