using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm1.Model
{
    class SingleChoice
    {
        //Id，主键，自增
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        //题干，不为空
        [NotNull]
        public string Stems { get; set; }

        //选项1，不为空
        [NotNull]
        public string Choices1 { get; set; }

        //选项2，不为空
        [NotNull]
        public string Choices2 { get; set; }

        //选项3，不为空
        [NotNull]
        public string Choices3 { get; set; }

        //答案，不为空
        [NotNull]
        public string Answer { get; set; }
    }
}
