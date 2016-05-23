using GalaSoft.MvvmLight;
using Leaf.Model;
using Leaf.SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.ViewModel
{
    class TestPaperModel : ViewModelBase
    {
        private List<TestPaper> _testpapaerlist;
        public List<TestPaper> TestList
        {
            get { return _testpapaerlist; }
            set { Set(ref _testpapaerlist, value); }
        }

        private void Init()
        {
            InsertTestData();
            ReadTestData();
        }

        public TestPaperModel()
        {
            Init();
        }

        private void ReadTestData()
        {
            if (TestList == null || TestList.Count == 0)
            {
                var db = new DbTestService();
                var newStr = new[] {"1"};
                TestList = (List<TestPaper>)db.Query(newStr);
            }
        }

        private void InsertTestData()
        {
            var db = new DbTestService();
            if (db.QueryNum() > 10)
            {
                return;
            }
            var num = new[] { 0, 2, 1, 0, 2, 1, 1, 1, 0, 2 };
            foreach (int t in num)
            {

                var singleQuestionNum = t.ToString();
                var gapQuestionNum = t.ToString();
                var singlenum = 1;
                var gapnum = 1;
                var level = 1;
                var model = new TestPaper { SingleQuestionNum = singleQuestionNum, GapQuestionNum = gapQuestionNum, SingleNum = singlenum, GapNum = gapnum, Level = level, BuildTime = "a" };
                var i = db.Insert(model);
                if (i > 0)
                {
                    Debug.WriteLine("yooooo, 加入成功了");
                }
            }
        }
    }
}
