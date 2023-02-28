namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class rating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? film_id { get; set; }

        [StringLength(255)]
        public string rate { get; set; }

        public int? id_user { get; set; }

        public int? number_start { get; set; }

        public DateTime created_time { get; set; }

        [Required]
        [StringLength(255)]
        public string name_user { get; set; }

        public virtual film film { get; set; }

        public virtual usercgv usercgv { get; set; }
    }
}
