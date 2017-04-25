//using SQLite;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Leaf.Model
{
    internal class SingleChoice
    {
        //Id，主键
        [Key]
        public int Id { get; set; }

        //题干
        public string Stems { get; set; }

        //选项1
        public string Choices1 { get; set; }

        //选项2
        public string Choices2 { get; set; }

        //选项3
        public string Choices3 { get; set; }

        //答案
        public string Answer { get; set; }

        public string ImgPath { get; set; }

        //难度
        public int Level { get; set; }

        //类型
        public string Type { get; set; }

        //主题
        public string Subject { get; set; }

        //所属是试卷
        public virtual ICollection<SingleTest> testpapers { get; set; }

        //构造函数，初始化
        public SingleChoice()
        {
            testpapers = new HashSet<SingleTest>();
        }
    }
}