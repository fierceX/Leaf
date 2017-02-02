using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.Model
{
    class MyDBContext : DbContext
    {
        public DbSet<GapFilling> GapFillings { get; set; }
        public DbSet<SingleChoice> SingleChoices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TestPaper> TestPapers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Leaf.db");
        }

    }
}
