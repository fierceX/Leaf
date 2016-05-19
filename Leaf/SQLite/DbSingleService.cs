using Leaf.Model;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
        public override int InsertOrIgnore(object item)
        {
            throw new NotImplementedException();
        }
        public override object Query(params string[] value)
        {
            string sqlstring = "select * frome SingleChoice where type=\"" + value[0] + "\" and level=" + value[1];
            List<SingleChoice> Singlelist = new List<SingleChoice>();
            using (var db = DB.GetDbConnection())
            {
                Singlelist = db.Query<SingleChoice>(sqlstring);
            }
            return Singlelist;
        }
    }
}
