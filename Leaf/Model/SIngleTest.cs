using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.Model
{
    class SingleTest
    {
        public int SingleId { get; set; }
        public int TestId { get; set; }
        public virtual SingleChoice single { get; set; }
        public virtual TestPaper test { get; set; }
    }
}
