using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaf.SQLite;

namespace Leaf.Model
{
    class server
    {
        public static bool authenticate(User mobel)
        {
            //var db = new DbUserService();
            Md5 md5 = new Md5();
            string mobelpassword = md5.ToMd5(mobel.Password);
            //User q = (User)db.QueryObject(mobel.Username);
            User user = null;
            using (var db = new MyDBContext())
            {
                IEnumerable<User> m = db.Users.Where(p => p.Username == mobel.Username);
                if(m.Count()>0)
                    user = m.ToList()[0];
            }
            if (mobelpassword == user.Password)
            {
                ViewModelLocator.User = user;
                return true;
            }
            else
            {
                return false;
            }

                //if (q == null)
                //    return false;
                //if (q.Password == mobelpassword)
                //{
                //    ViewModelLocator.User = q;
                //    return true;
                //}
                //return false;
                //return true;
        }
    }
}
