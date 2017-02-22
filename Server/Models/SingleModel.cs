namespace Server.Models
{
    using System.Data.Entity;

    public partial class SingleModel : DbContext
    {
        public SingleModel()
            : base("name=SingleModel")
        {
        }

        public virtual DbSet<SingleChoice> SingleChoice { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
