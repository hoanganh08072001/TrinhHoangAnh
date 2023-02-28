namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("post")]
    public partial class post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Required]
        [StringLength(250)]
        public string title { get; set; }

        [StringLength(250)]
        [Required]
        public string description { get; set; }

        [StringLength(250)]
        public string image { get; set; }

        public DateTime created_at { get; set; }

        public int id_cpost { get; set; }

        public virtual category_post category_post { get; set; }
    }
}
