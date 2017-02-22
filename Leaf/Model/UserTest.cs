using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.Model
{
    class UserTest
    {
        public int UserId { get; set; }
        public int TestId { get; set; }
        public virtual User user { get; set; }
        public virtual TestPaper testpaper { get; set; }
        public int singnum { get; set; }
        public int gapnum { get; set; }
    }
}
