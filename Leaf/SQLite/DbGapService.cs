using Leaf.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.SQLite
{
    class DbGapService:DbService
    {
        public override int Delete(object item)
        {
            throw new NotImplementedException();
        }

        public override int Insert(object item)
        {
            int result = 0;
            //using (var db = DB.GetDbConnection())
            //{
            //    try
            //    {
            //        result = db.Insert(item);
            //    }
            //    catch (Exception exception)
            //    {
            //        // 捕获重复插入异常
            //        Debug.WriteLine(exception);
            //    }
            //}
            using (var db = new MyDBContext())
            {
                GapFilling Item = (GapFilling)item;
                db.GapFillings.Add(Item);
                db.SaveChanges();
            }
            return result;
        }
        public override int InsertOrIgnore(object item)
        {
            throw new NotImplementedException();
        }
        public override object QueryObject(params string[] value)
        {
            string sqlstring = "select " + value[0] + " from GapFilling";
            if (value.Length >= 2)
                sqlstring = sqlstring + " where Type=\"" + value[1] + "\"";
            if (value.Length >= 3)
                sqlstring = sqlstring + " and Level=" + value[2];
            if (value.Length >= 4)
                sqlstring = sqlstring + " and Subject=\"" + value[3] + "\"";
            if(value.Length == 5)
                sqlstring = sqlstring + " ORDER BY RANDOM() limit " + value[4];
            List<GapFilling> Gaplist = new List<GapFilling>();
            using (var db = DB.GetDbConnection())
            {
                try
                {
                    Gaplist = db.Query<GapFilling>(sqlstring);
                }
                catch (Exception exception)
                {
                    // 捕获重复插入异常
                    Debug.WriteLine(exception);
                }
                
            }
            return Gaplist;
        }

        public object QuerySql(params string[] value)
        {
            string sqlstring = "select " + value[0] + " from GapFilling "+value[1];
            List<GapFilling> Gaplist = new List<GapFilling>();
            using (var db = DB.GetDbConnection())
            {
                try
                {
                    Gaplist = db.Query<GapFilling>(sqlstring);
                }
                catch (Exception exception)
                {
                    // 捕获重复插入异常
                    Debug.WriteLine(exception);
                }

            }
            return Gaplist;
        }


        public override object Query(params string[] value)
        {
            var result = 0;
            string sqlString = "select count(*) from GapFilling";
            if (value.Length >= 1)
                sqlString = sqlString + " where Type=\"" + value[0] + "\"";
            if (value.Length >= 2)
                sqlString = sqlString + " and Level=" + value[1];
            if (value.Length == 3)
                sqlString = sqlString + " and Subject=\"" + value[2] + "\"";
            using (var db = DB.GetDbConnection())
            {
                result = db.ExecuteScalar<int>(sqlString);
            }
            return result;
        }
    }
}
