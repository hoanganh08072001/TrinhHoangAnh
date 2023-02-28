using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    
    public class ScheduleAjax
    {
        public int id { get; set; }

        public int? film_id { get; set; }

        public DateTime? dateschedule { get; set; }

        public List<showtime> listschedule { get; set; }
    }
}
