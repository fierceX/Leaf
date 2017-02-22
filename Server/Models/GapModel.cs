namespace Server.Models
{
    using System.Data.Entity;

    public partial class GapModel : DbContext
    {
        public GapModel()
            : base("name=GapModel")
        {
        }

        public virtual DbSet<GapFilling> GapFilling { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
