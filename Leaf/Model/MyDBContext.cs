using Microsoft.EntityFrameworkCore;

namespace Leaf.Model
{
    /// <summary>
    /// 数据库上下文类
    /// </summary>
    class MyDBContext : DbContext
    {
        //填空题表
        public DbSet<GapFilling> GapFillings { get; set; }
        //单选题表
        public DbSet<SingleChoice> SingleChoices { get; set; }
        //用户表
        public DbSet<User> Users { get; set; }
        //试卷表
        public DbSet<TestPaper> TestPapers { get; set; }
        //单选题和试卷级联表
        public DbSet<SingleTest> SingleTest { get; set; }
        //填空题和试卷级联表
        public DbSet<GapTest> GapTest { get; set; }
        //用户和试卷级联表
        public DbSet<UserTest> UserTest { get; set; }

        //实体模型到数据库的映射
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SingleTest>().HasKey(t => new { t.SingleId, t.TestId });
            modelBuilder.Entity<SingleTest>()
                .HasOne(pt => pt.single)
                .WithMany(p => p.testpapers).HasForeignKey(pt => pt.SingleId);
            modelBuilder.Entity<SingleTest>()
                .HasOne(pt => pt.test)
                .WithMany(p => p.singles)
                .HasForeignKey(pt => pt.TestId);

            modelBuilder.Entity<GapTest>().HasKey(t => new { t.GapId, t.TestId });
            modelBuilder.Entity<GapTest>()
                .HasOne(pt => pt.gap)
                .WithMany(p => p.testpapers).HasForeignKey(pt => pt.GapId);
            modelBuilder.Entity<GapTest>()
                .HasOne(pt => pt.test)
                .WithMany(p => p.gapfills)
                .HasForeignKey(pt => pt.TestId);


            modelBuilder.Entity<UserTest>().HasKey(t => new { t.Id });
            modelBuilder.Entity<UserTest>()
                .HasOne(pt => pt.testpaper)
                .WithMany(p => p.users).HasForeignKey(pt => pt.UserId);
            modelBuilder.Entity<UserTest>()
                .HasOne(pt => pt.user)
                .WithMany(p => p.TestPapers)
                .HasForeignKey(pt => pt.TestId);

            base.OnModelCreating(modelBuilder);

        }
        //数据库名字
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Leaf.db");
        }
        

    }
}
