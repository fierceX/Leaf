using Leaf.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.SQLite
{
    //class DbTestService:DbService
    //{
    //    public override int Insert(object item)
    //    {
    //        int result = 0;
    //        using (var db = DB.GetDbConnection())
    //        {
    //            try
    //            {
    //                result = db.Insert(item);
    //            }
    //            catch (Exception exception)
    //            {
    //                // 捕获重复插入异常
    //                Debug.WriteLine(exception);
    //            }
    //        }
    //        return result;
    //    }
    //    public override object QueryObject(params string[] value)
    //    {
    //        string sqlstring = "select * from TestPaper";
    //        List<TestPaper> Testlist = new List<TestPaper>();
    //        using (var db = DB.GetDbConnection())
    //        {
    //            Testlist = db.Query<TestPaper>(sqlstring);
    //        }
    //        return Testlist;
    //    }
    //    public override int Delete(object item)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public override int InsertOrIgnore(object item)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public override object Query(params string[] value)
    //    {
    //        var result = 0;
    //        const string sqlString = "select count(*) from TestPaper";
    //        using (var db = DB.GetDbConnection())
    //        {
    //            var singlenum = db.ExecuteScalar<int>(sqlString);
    //            result = singlenum;
    //        }
    //        return result;
    //    }
    //}
}
