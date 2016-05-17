using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm1.Model
{
    class DbService
    {
        public int InserUser(object item)
        {
            int result = 0;
            using (var db = DB.GetDbConnection())
            {
                result = db.Insert(item);
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
