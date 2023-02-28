namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("booking")]
    public partial class booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int id_user { get; set; }

        public int film_id { get; set; }

        public int schedule_id { get; set; }

        public int showtime_id { get; set; }

        public int room_id { get; set; }

        public int seat_id { get; set; }

        public int amount { get; set; }

        public int status { get; set; }

        public string create_time { get; set; }

        public virtual film film { get; set; }

        public virtual usercgv usercgv { get; set; }

        public virtual schedule schedule { get; set; }

        public virtual showtime showtime { get; set; }
    }
}
