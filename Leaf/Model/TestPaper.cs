//using SQLite;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Leaf.Model
{
    class TestPaper
    {
        //试卷Id
        //主键
        [Key]
        public int Id { get; set; }

        //试卷名称

        public string Name { get; set; }

        //单选题列表
        public List<SingleTest> singles { get; set; }

        //填空题列表
        public  List<GapTest> gapfills { get; set; }

        //单选数量
        public int SingleNum { get; set; }

        //填空数量
        public int GapNum { get; set; }

        //等级
        public int Level { get; set; }

        //生成时间
        public string BuildTime { get; set; }

        //成绩
        public int Score { get; set; }

        //时间
        //不为空
        public int Time { get; set; }

        //用户列表
        public List<UserTest> users { get; set; }
        public TestPaper()
        {
            singles = new List<SingleTest>();
            gapfills = new List<GapTest>();
            users = new List<UserTest>();
        }
    }
}
