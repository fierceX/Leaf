namespace Server.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Subject")]
    public partial class Subject
    {
        public int id { get; set; }

        [Column("subject")]
        [Required]
        [StringLength(100)]
        public string subject { get; set; }

        public int gapnum { get; set; }

        public int singlenum { get; set; }

        [Required]
        [StringLength(50)]
        public string type { get; set; }

        public int level { get; set; }
    }
}
