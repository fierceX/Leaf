using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.Model
{
    public abstract class DbService
    {
        public abstract int Insert(object item);
        /// <summary>
        /// 插入语句 如果已有则忽略
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract int InsertOrIgnore(object item);
        public abstract object Query(string value);
        public abstract int Delete(object item);
    }
}
