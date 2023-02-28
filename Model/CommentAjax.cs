using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CommentAjax
    {
        public int id { get; set; }

        public int? film_id { get; set; }

       
        public string rate { get; set; }

        public int? id_user { get; set; }
        public int? number_start { get; set; }

        public string created_time { get; set; }

        public string name_user { get; set; }
    }
}
