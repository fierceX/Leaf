namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SingleChoice")]
    public partial class SingleChoice
    {
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string Stems { get; set; }

        [Required]
        [StringLength(100)]
        public string Choices1 { get; set; }

        [Required]
        [StringLength(100)]
        public string Choices2 { get; set; }

        [Required]
        [StringLength(100)]
        public string Choices3 { get; set; }

        [Required]
        [StringLength(100)]
        public string Answer { get; set; }

        public int level { get; set; }

        [Required]
        [StringLength(50)]
        public string type { get; set; }

        [Required]
        [StringLength(100)]
        public string subject { get; set; }
    }
}
