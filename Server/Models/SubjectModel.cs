namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SubjectModel : DbContext
    {
        public SubjectModel()
            : base("name=SubjectModel")
        {
        }

        public virtual DbSet<Subject> Subject { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
