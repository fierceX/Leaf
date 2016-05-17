using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm1.Model
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

    }
}
