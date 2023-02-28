using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace DatabaseIO
{
    
    public class BookingDao
    {
        MyDB mydb = new MyDB();
        FilmDao filmD = new FilmDao();
        RoomDao roomD = new RoomDao();
        SeatDao seatD = new SeatDao();

        /**
         * confirm booking ticker for user
         * @param id
         */
        public void accpect(string id)
        {
            string SQL = "UPDATE booking SET status = 1 WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }

        /**
         * create new a object booking
         * @param schedule_id
         * @param film_id
         * @param room_id
         * @param showtime_id
         * @param id_user
         * @param create_time
         * @return
         */
        public booking newObjectBooking(int schedule_id,int film_id,int room_id,int showtime_id, int id_user, string create_time)
        {
            int statusBook = 0;
            int amountDefault = 3;
            booking book = new booking();
            book.schedule_id = schedule_id;
            book.film_id = film_id;
            book.room_id = room_id;
            book.showtime_id = showtime_id;
            book.id_user = id_user;
            book.status = statusBook;
            book.create_time = create_time;
            book.amount = amountDefault;
            return book;
        }

        /**
         * add more ticket bookings into database
         * @param arrSeat
         * @param order
         * @param create_time
         * @param id_user
         * @param status
         */
        public void addBookingTicket(int[] arrSeat,booking order, string create_time,int id_user,int status)
        {
            booking book = new booking();
            int amountDefault = 65000;
            int lengthArSeat = arrSeat.Length;
            for (int i = 0; i < lengthArSeat; i++)
            {
                book.schedule_id = order.schedule_id;
                book.film_id = order.film_id;
                book.seat_id = arrSeat[i];
                book.room_id = order.room_id;
                book.showtime_id = order.showtime_id;
                book.id_user = id_user;
                book.status = status;
                book.create_time = order.create_time;
                book.amount = amountDefault;
                // add a booking ticket into database
                filmD.bookingTicket(book, create_time);
            }
        }

        /**
         * create new a object historybooking
         * @param film_id
         * @param id
         * @param room_id
         * @param seat_id
         * @param dateschedule
         * @param amount
         * @param ngay
         * @param status
         * @return
         */
        public HistoryBooking addHistoryBooking(int film_id,int id, int room_id, int seat_id, string dateschedule, int amount,string ngay,int status)
        {

            HistoryBooking his = new HistoryBooking();
            his.nameFilm = filmD.getName(film_id).film_name;
            his.id = id + 1;
            his.roomName = roomD.getName(room_id).room_name;
            his.seatName = seatD.getName(seat_id).seat_name;
            his.status = status;
            his.schedulename = dateschedule;
            his.amount = amount.ToString();
            his.showtimeName = ngay;
            return his;
        }

        /**
         * check data change when booking ticket
         * @param film_id
         * @param schedule_id
         * @param showtime_id
         * @param room_id
         * @param seat_id
         * @return
         */
        public bool checkBooking(int film_id, int schedule_id, int showtime_id, int room_id, int[] seat_id)
        {
            int lengthArrSeatId = seat_id.Length;
            for (int i = 0 ; i < lengthArrSeatId; i ++) {
                int idSeat = seat_id[i];
                var booking = mydb.bookings.Where(b => b.film_id == film_id && b.schedule_id == schedule_id && b.room_id == room_id 
                && b.seat_id == idSeat && b.showtime_id == showtime_id).FirstOrDefault();
                if (booking != null) {
                    return true;
                }
            }
            return false;
        }

        /**
         * get list booking before the premiere time
         * @return
         */
        public List<booking> getBooking()
        {
            try {
                return mydb.Database.SqlQuery<booking>("select a.* from booking a, schedules b, showtimes c where a.schedule_id = b.id and a.status = 0 and a.showtime_id = c.id and CONVERT(varchar, b.dateschedule, 101) = CONVERT(varchar, getdate(), 101) and(cast(CONVERT(datetime, c.start_time, 101) as float) - floor(cast(CONVERT(datetime, c.start_time, 101) as float))) - (cast(getdate() as float) - FLOOR(cast(getdate() as float))) <= 0.0833333333333333 and(cast(CONVERT(datetime, c.start_time, 101) as float) - floor(cast(CONVERT(datetime, c.start_time, 101) as float))) - (cast(getdate() as float) - FLOOR(cast(getdate() as float))) > 0").ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
        public void Delete(int id)
        {
            //delete from booking by id
            string SQL = "DELETE FROM booking WHERE id = '" + id + "'";
            mydb.Database.ExecuteSqlCommand(SQL);
        }
    }
}
