using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class HistoryBooking
    {
        public int id { get; set; }
        public string nameFilm { get; set; }

        public string schedulename { get; set; }

        public string showtimeName { get; set; }

        public string roomName { get; set; }

        public string seatName { get; set; }

        public string amount { get; set; }
        public int status { get; set; }
    }
}
