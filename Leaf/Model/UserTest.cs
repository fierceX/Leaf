namespace Leaf.Model
{
    internal class UserTest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public virtual User user { get; set; }
        public virtual TestPaper testpaper { get; set; }
        public int singnum { get; set; }
        public int gapnum { get; set; }
        public string Time { get; set; }
        public double Score { get; set; }
    }
}