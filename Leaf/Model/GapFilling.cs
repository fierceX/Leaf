//using SQLite;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Leaf.Model
{
    /// <summary>
    /// 填空题实体模型
    /// </summary>
    class GapFilling
    {
        //Id,主键，自增

        [Key]
        public int Id { get; set; }

        //题干
        public string Stems { get; set; }

        //答案
        public string Answer { get; set; }

        //难度
        public int Level { get; set; }

        //类型
        public string Type { get; set; }

        //主题
        public string Subject { get; set; }

        //所属试卷
        public virtual ICollection<GapTest> testpapers { get; set; }

        //构造函数，初始化
        public GapFilling()
        {

            testpapers = new HashSet<GapTest>();
        }
    }
}
