//using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.Model
{
    class TestPaper
    {
        //试卷Id
        //主键，不为空
        //[PrimaryKey]
        //[AutoIncrement]
        [Key]
        public int Id { get; set; }

        //试卷名称
        //不为空
        //[NotNull]
        public string Name { get; set; }

        //单选内容Id号
        //不为空
        //[NotNull]
        //public string SingleQuestionNum { get; set; }
        public List<SingleTest> singles { get; set; }

        //填空内容Id号
        //不为空
        //[NotNull]
        //public string GapQuestionNum { get; set; }
        public  List<GapTest> gapfills { get; set; }

        //单选数量
        //不为空
        //[NotNull]
        public int SingleNum { get; set; }

        //填空数量
        //不为空
        //[NotNull]
        public int GapNum { get; set; }

        //等级
        //不为空
        //[NotNull]
        public int Level { get; set; }

        //生成时间
        //不为空
        //[NotNull]
        public string BuildTime { get; set; }

        //成绩
        //不为空
        //[NotNull]
        public int Score { get; set; }

        //时间
        //不为空
        public int Time { get; set; }

        public TestPaper()
        {
            singles = new List<SingleTest>();
            gapfills = new List<GapTest>();
        }
    }
}
