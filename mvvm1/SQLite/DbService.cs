using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm1.Model
{
    public class DbService
    {
        public int InserUser(object item)
        {
            int result = 0;
            using (var db = DB.GetDbConnection())
            {
                try
                {
                      result = db.Insert(item);
                }
                catch (SQLite.SQLiteException exception)
                {
                    // 捕获重复插入异常
                    Debug.WriteLine(exception);
                }
            }
            return result;
        }

        public User Query(string name)
        {
            User model = null;
            using (var db = DB.GetDbConnection())
            {
                string sqlstring = "select * from user where username=\"" + name+"\"";
                List<User> queryobject = db.Query<User>(sqlstring);
                if(queryobject != null)
                    model = queryobject[0];
            }
            return model;
        }

    }
}
