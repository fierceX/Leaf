namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

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
