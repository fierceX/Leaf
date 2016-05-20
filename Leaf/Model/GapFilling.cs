using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.Model
{
    class GapFilling
    {
        //Id,主键，自增
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        //题干，不为空
        [NotNull]
        public string Stems { get; set; }

        //答案，不为空
        [NotNull]
        public string Answer { get; set; }

        //难度，不为空
        [NotNull]
        public int Level { get; set; }

        //类型，不为空
        [NotNull]
        public string Type { get; set; }
    }
}
