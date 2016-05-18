using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm1.Model
{
    public abstract class DbService
    {
        public abstract int Insert(object item);
        public abstract object Query(string value);
        public abstract int Delete(object item);
    }
}
