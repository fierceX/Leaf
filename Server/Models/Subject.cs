namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subject")]
    public partial class Subject
    {
        public int id { get; set; }

        [Column("subject")]
        [Required]
        [StringLength(100)]
        public string subject1 { get; set; }

        public int gapnum { get; set; }

        public int singlenum { get; set; }

        [Required]
        [StringLength(50)]
        public string type { get; set; }

        public int level { get; set; }
    }
}
