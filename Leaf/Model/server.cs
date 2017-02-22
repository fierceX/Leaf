using System.Collections.Generic;
using System.Linq;

namespace Leaf.Model
{
    //用户登陆验证
    class server
    {
        public static bool authenticate(User mobel)
        {
            Md5 md5 = new Md5();
            //加密登陆密码
            string mobelpassword = md5.ToMd5(mobel.Password);
            User user = null;
            //检索相同用户名的用户
            using (var db = new MyDBContext())
            {
                IEnumerable<User> m = db.Users.Where(p => p.Username == mobel.Username);
                if(m.Count()>0)
                    user = m.ToList()[0];
            }
            //对比密码是否相同
            if (mobelpassword == user.Password)
            {
                ViewModelLocator.User = user;
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
