using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

//using SQLite;

namespace Leaf.Model
{
    internal class User
    {
        //Id，主键，
        [Key]
        public int Id { get; set; }

        //用户名
        public string Username { get; set; }

        //密码
        public string Password { get; set; }

        //成绩列表
        public string Score { get; set; }

        //管理员标识

        public int Admin { get; set; }

        //注册时间

        public string BuildTime { get; set; }

        //试卷列表
        public List<UserTest> TestPapers { get; set; }

        public User()
        {
            TestPapers = new List<UserTest>();
        }

        public void Clear()
        {
            Id = 0;
            Username = "";
            Password = "";
            Admin = -1;
        }
    }
}