using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.Model
{
    class GapTest
    {
        public int GapId { get; set; }
        public int TestId { get; set; }
        public virtual GapFilling gap { get; set; }
        public virtual TestPaper test { get; set; }
    }
}
