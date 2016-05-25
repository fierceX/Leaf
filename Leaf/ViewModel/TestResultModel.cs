using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaf.Model;

namespace Leaf.ViewModel
{
    class TestResultModel
    {
        public TestPaper TestPaperModel = new TestPaper();
        public List<bool> SingleResult = new List<bool>();
        public List<bool> GapResult = new List<bool>();
    }
}
