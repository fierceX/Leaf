using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm1.Model
{
    class server
    {
        public static bool authenticate(User mobel)
        {
            DbService db = new DbService();
            Md5 md5 = new Md5();
            string mobelpassword = md5.ToMd5(mobel.Password);
            User q = db.Query(mobel.Username);
            if (q == null)
                return false;
            if (q.Password == mobelpassword)
                return true;
            return false;
        }
    }
}
