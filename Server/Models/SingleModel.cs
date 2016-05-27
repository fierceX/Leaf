namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

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
