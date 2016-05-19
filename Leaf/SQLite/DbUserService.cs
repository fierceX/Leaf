using Leaf.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.SQLite
{
    /// <summary>
    /// 
    /// </summary>
    class DbUserService : DbService
    {
        public override int InsertOrIgnore(object item)
        {
            int result = 0;
            using (var db = DB.GetDbConnection())
            {
                    result = db.Insert(item, "OR IGNORE", item.GetType());
            }
            return result;
        }

        public override int Insert(object item)
        {
            int result = 0;
            using (var db = DB.GetDbConnection())
            {
                try
                {
                    result = db.Insert(item);
                }
                catch (Exception exception)
                {
                    // 捕获重复插入异常
                    Debug.WriteLine(exception);
                }
            }
            return result;
        }

        public override object Query(string value)
        {
            User model = null;
            using (var db = DB.GetDbConnection())
            {
                string sqlstring = "select * from user where username=\"" + value + "\"";
                List<User> queryobject = db.Query<User>(sqlstring);
                if (queryobject.Count > 0)
                    model = queryobject[0];
            }
            return model;
        }
        public override int Delete(object item)
        {
            return 0;
        }

        public int QueryNum()
        {
            var result = 0;
            const string sqlString = "select count(*) from User";
            using (var db = DB.GetDbConnection())
            {
                var usernum = db.ExecuteScalar<int>(sqlString);
                result = usernum > 0 ? 1 :  0;
            }
            return result;
        }
    }
}
