namespace Server.Models
{
    using System.Data.Entity;

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
