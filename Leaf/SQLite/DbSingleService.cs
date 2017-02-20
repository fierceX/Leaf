using Leaf.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.SQLite
{
    //class DbSingleService:DbService
    //{
    //    public override int Delete(object item)
    //    {
    //        throw new NotImplementedException();
    //    }

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
    //    public override int InsertOrIgnore(object item)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public override object QueryObject(params string[] value)
    //    {
    //        string sqlstring = "select " + value[0] + " from SingleChoice";
    //        if (value.Length >= 2)
    //            sqlstring = sqlstring + " where Type=\"" + value[1] + "\"";
    //        if (value.Length >= 3)
    //            sqlstring = sqlstring + " and Level=" + value[2];
    //        if (value.Length >= 4)
    //            sqlstring = sqlstring + " and Subject=\"" + value[3] + "\"";
    //        if (value.Length == 5)
    //            sqlstring = sqlstring + " ORDER BY RANDOM() limit " + value[4];
    //        List<SingleChoice> Singlelist = new List<SingleChoice>();
    //        using (var db = DB.GetDbConnection())
    //        {
    //            try
    //            {
    //                Singlelist = db.Query<SingleChoice>(sqlstring);
    //            }
    //            catch (Exception exception)
    //            {
    //                // 捕获重复插入异常
    //                Debug.WriteLine(exception);
    //            }
    //        }
    //        return Singlelist;
    //    }

    //    public object Querysql(params string[] value)
    //    {
    //        string sqlstring = "select " + value[0] + " from SingleChoice " + value[1];
    //        List<SingleChoice> Singlelist = new List<SingleChoice>();
    //        using (var db = DB.GetDbConnection())
    //        {
    //            try
    //            {
    //                Singlelist = db.Query<SingleChoice>(sqlstring);
    //            }
    //            catch (Exception exception)
    //            {
    //                // 捕获重复插入异常
    //                Debug.WriteLine(exception);
    //            }
    //        }
    //        return Singlelist;
    //    }

    //    public override object Query(params string[] value)
    //    {

    //        string sqlString = "select count(*) from SingleChoice";
    //        if (value.Length >= 1)
    //            sqlString = sqlString + " where Type=\"" + value[0] + "\"";
    //        if (value.Length >= 2)
    //            sqlString = sqlString + " and Level=" + value[1];
    //        if (value.Length == 3)
    //            sqlString = sqlString + " and Subject=\"" + value[2] + "\"";
    //        var result = 0;
    //        using (var db = DB.GetDbConnection())
    //        {
    //            var singlenum = db.ExecuteScalar<int>(sqlString);
    //            result = singlenum;
    //        }
    //        return result;
    //    }
    //}
}
