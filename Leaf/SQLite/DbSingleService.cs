using Leaf.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.SQLite
{
    class DbSingleService:DbService
    {
        public override int Delete(object item)
        {
            throw new NotImplementedException();
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
        public override int InsertOrIgnore(object item)
        {
            throw new NotImplementedException();
        }
        public override object Query(params string[] value)
        {
            string sqlstring = "select * from SingleChoice where Type=\"" + value[0] + "\" and Level=" + value[1];
            List<SingleChoice> Singlelist = new List<SingleChoice>();
            using (var db = DB.GetDbConnection())
            {
                Singlelist = db.Query<SingleChoice>(sqlstring);
            }
            return Singlelist;
        }

        public int QueryNum()
        {
            var result = 0;
            const string sqlString = "select count(*) from SingleChoice";
            using (var db = DB.GetDbConnection())
            {
                var singlenum = db.ExecuteScalar<int>(sqlString);
                result = singlenum;
            }
            return result;
        }
    }
}
