using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.Model
{
    class QuestionView
    {
        //类型
        public string Type { get; set; }
        //颜色
        public string Color { get; set; }
        //选择题数量
        public int SingleNum { get; set; }
        //填空题数量
        public int GapNum { get; set; }
        //难度
        public int Level { get; set; }
        public string Subject { get; set; }
    }
}
