using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Leaf.Model;
using Leaf.SQLite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Leaf.ViewModel
{
    class TestPaperModel : ViewModelBase
    {

        private List<int> num = new List<int>();
        private int SingleMax;
        private int GapMax;


        //试卷名称
        private string _testname;
        public string TestName
        {
            get { return _testname; }
            set { Set(ref _testname, value); }
        }
        //选择题类型
        private List<string> _singleTypeList = new List<string>();
        public List<string> SingleTypeList
        {
            get { return _singleTypeList; }
            set { Set(ref _singleTypeList, value); }
        }

        //选择题难度
        private List<string> _singleLevel = new List<string>();
        public List<string> SingleLevel
        {
            get { return _singleLevel; }
            set { Set(ref _singleLevel, value); }
        }

        //选择题数量
        private int _singleNum;
        public int SingleNum
        {
            get { return _singleNum; }
            set { Set(ref _singleNum, value); }
        }

        //选择题类型索引
        private int _singleTypeNum;
        public int SingleTypeNum
        {
            get { return _singleTypeNum; }
            set
            {
                Set(ref _singleTypeNum, value);
                //有bug的代码
                //GetQuestLevel();
            }
        }
        //选择题难度索引
        private int _singleLevelNum;
        public int SingleLevelNum
        {
            get { return _singleLevelNum; }
            set { Set(ref _singleLevelNum, value); }
        }


        //填空题类型
        private List<string> _gapTypeList = new List<string>();
        public List<string> GapTypeList
        {
            get { return _gapTypeList; }
            set { Set(ref _gapTypeList, value); }
        }

        //填空题难度
        private List<string> _gapLevel = new List<string>();
        public List<string> GapLevel
        {
            get { return _gapLevel; }
            set { Set(ref _gapLevel, value); }
        }

        //填空题数量
        private int _gapNum;
        public int GapNum
        {
            get { return _gapNum; }
            set { Set(ref _gapNum, value); }
        }

        //填空题类型索引
        private int _gapTypeNum;
        public int GapTypeNum
        {
            get { return _gapTypeNum; }
            set
            {
                Set(ref _gapTypeNum, value);
                //有bug的代码
                //GetQuestLevel();
            }
        }
        //填空题难度索引
        private int _gapLevelNum;
        public int GapLevelNum
        {
            get { return _gapLevelNum; }
            set { Set(ref _gapLevelNum, value); }
        }

        //弹出菜单状态
        private bool _paneopen;
        public bool PaneOpen
        {
            get { return _paneopen; }
            set { Set(ref _paneopen, value); }
        }

        //试卷列表索引
        private int _test;
        public int Test
        {
            get { return _test; }
            set { Set(ref _test, value); }
        }

        //试卷列表
        private List<TestPaper> _testpapaerlist;
        public List<TestPaper> TestList
        {
            get { return _testpapaerlist; }
            set { Set(ref _testpapaerlist, value); }
        }

        //开始答题
        public ICommand RunCommand { get; set; }
        private void run()
        {
            //num.Add(1);
            //num.Add(2);
            //string a = JsonConvert.SerializeObject(num);
        }

        //打开弹出菜单
        public ICommand OpenCommand { get; set; }
        private void Open()
        {
            PaneOpen = !PaneOpen;
        }

        //添加新试卷
        public ICommand AddCommand { get; set; }
        private void Add()
        {
            if(GapLevelNum >=0 && SingleLevelNum >=0 && GapTypeNum >=0 && SingleTypeNum >=0)
            {
                GetQuestNum();
                if(GapNum>=GapMax)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("该类型和难度下填空题没有足够数量题目，只有"+GapMax.ToString(), "AddNo");
                }
                else if(SingleNum>=SingleMax)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("该类型和难度下选择题没有足够数量题目，只有" + SingleMax.ToString(), "AddNo");
                }
                else
                {
                    TestPaper model = new TestPaper();
                    var sdb = new DbSingleService();
                    var snewstr = new[] {"Id",SingleTypeList[SingleTypeNum],SingleLevel[SingleLevelNum],SingleNum.ToString() };
                    List<SingleChoice> _singlelist = (List<SingleChoice>)sdb.QueryObject(snewstr);
                    var _newsinglenum = new List<int>();
                    for(int i=0;i<_singlelist.Count;i++)
                    {
                        _newsinglenum.Add(_singlelist[i].Id);
                    }
                    model.SingleQuestionNum= JsonConvert.SerializeObject(_newsinglenum);
                    model.SingleNum = SingleNum;

                    var gdb = new DbGapService();
                    var gnewstr = new[] { "Id", GapTypeList[GapTypeNum], GapLevel[GapLevelNum], GapNum.ToString() };
                    List<GapFilling> _gaplist = (List<GapFilling>)gdb.QueryObject(gnewstr);
                    var _newgapnum = new List<int>();
                    for (int i = 0; i < _gaplist.Count; i++)
                    {
                        _newgapnum.Add(_gaplist[i].Id);
                    }
                    model.GapQuestionNum = JsonConvert.SerializeObject(_newgapnum);
                    model.GapNum = GapNum;
                    model.Level = (GapNum * Convert.ToInt32(GapLevel[GapLevelNum]) + SingleNum * Convert.ToInt32(SingleLevel[SingleLevelNum])) / (SingleNum + GapNum);
                    model.Name = TestName;
                    model.BuildTime = DateTime.Now.ToString();
                    var db = new DbTestService();
                    int n = db.Insert(model);
                    if(n>0)
                    {
                        ReadTestData();
                        GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("试卷添加成功", "AddYes");
                    }
                    else
                    {
                        GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("添加失败", "AddNo");
                    }
                }
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("请先选择难度和类型", "AddNo");
            }
        }

        //初始化
        private void Init()
        {
            //InsertTestData();
            ReadTestData();
            GetQuestType();
            GetQuestLevel();
        }

        //构造函数
        public TestPaperModel()
        {
            Init();
            RunCommand = new RelayCommand(run);
            OpenCommand = new RelayCommand(Open);
            AddCommand = new RelayCommand(Add);
        }


        //测试读取
        private void ReadTestData()
        {
            var db = new DbTestService();
            TestList = (List<TestPaper>)db.QueryObject();
        }

        //获取题目类型
        private void GetQuestType()
        {
            if (GapTypeList == null || GapTypeList.Count == 0)
            {
                var db = new DbGapService();
                var _gaplist = (List<GapFilling>)db.QueryObject("distinct Type");
                GapTypeList.Add("   ");
                for (int i = 0; i < _gaplist.Count;i++)
                {
                    GapTypeList.Add(_gaplist[i].Type);
                }
            }
            if (SingleTypeList == null || SingleTypeList.Count == 0)
            {
                var db = new DbSingleService();
                var _singlelist = (List<SingleChoice>)db.QueryObject("distinct Type");
                SingleTypeList.Add("  ");
                for (int i = 0; i < _singlelist.Count; i++)
                {
                    SingleTypeList.Add(_singlelist[i].Type);
                }
            }


        }
        //获取题目难度
        private void GetQuestLevel()
        {

            if (GapLevel == null || GapLevel.Count == 0)
            {
                var db = new DbGapService();
                var _gaplist = (List<GapFilling>)db.QueryObject("distinct Level");
                GapLevel.Add("  ");
                for (int i = 0; i < _gaplist.Count; i++)
                {
                    GapLevel.Add(_gaplist[i].Level.ToString());
                }
            }
            if (SingleLevel == null || SingleLevel.Count == 0)
            {
                var db = new DbSingleService();
                var _singlelist = (List<SingleChoice>)db.QueryObject("distinct Level");
                SingleLevel.Add("  ");
                for (int i = 0; i < _singlelist.Count; i++)
                {
                    SingleLevel.Add(_singlelist[i].Level.ToString());
                }
            }

            //有bug的代码
            //if (GapTypeList != null && GapTypeList.Count != 0 && GapTypeNum >= 0)
            //{
            //    var db = new DbGapService();
            //    var newstr = new []{ "distinct Level",GapTypeList[GapTypeNum] };
            //    var _gaplist = (List<GapFilling>)db.QueryObject(newstr);
            //    for (int i = 0; i < _gaplist.Count; i++)
            //    {
            //        GapLevel.Add(_gaplist[i].Level.ToString());
            //    }
            //}
            //if (SingleTypeList != null && SingleTypeList.Count != 0 && SingleTypeNum >= 0)
            //{
            //    var db = new DbSingleService();
            //    var newstr = new[] { "distinct Level", SingleTypeList[SingleTypeNum] };
            //    var _singlelist = (List<SingleChoice>)db.QueryObject(newstr);
            //    for (int i = 0; i < _singlelist.Count; i++)
            //    {
            //        SingleLevel.Add(_singlelist[i].Level.ToString());
            //    }
            //}
        }
        //获取题目数量
        private void GetQuestNum()
        {
            var gdb = new DbGapService();
            var gnewstr = new[] {GapTypeList[GapTypeNum], GapLevel[GapLevelNum] };
            GapMax = (int)gdb.Query(gnewstr);
            var sdb = new DbSingleService();
            var snewstr = new[] {SingleTypeList[SingleTypeNum], SingleLevel[SingleLevelNum] };
            SingleMax = (int)sdb.Query(snewstr);
        }

        //插入新试卷
        private void Insert()
        {

        }


        //测试插入
        //private void InsertTestData()
        //{
        //    var db = new DbTestService();
        //    if (db.QueryNum() >= 10)
        //    {
        //        return;
        //    }
        //    var num = new[] { 0, 2, 1, 0, 2, 1, 1, 1, 0, 2 };
        //    foreach (int t in num)
        //    {

        //        var singleQuestionNum = t.ToString();
        //        var gapQuestionNum = t.ToString();
        //        var singlenum = t;
        //        var gapnum = t*2;
        //        var level = 1;
        //        var name = t.ToString();
        //        var model = new TestPaper {Name=name, SingleQuestionNum = singleQuestionNum, GapQuestionNum = gapQuestionNum, SingleNum = singlenum, GapNum = gapnum, Level = level, BuildTime = "a" };
        //        var i = db.Insert(model);
        //        if (i > 0)
        //        {
        //            Debug.WriteLine("yooooo, 加入成功了");
        //        }
        //    }
        //}
    }
}
