using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Leaf.Model;
using Leaf.SQLite;
using Microsoft.Practices.ServiceLocation;
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
        /// <summary>
        /// 选择题最大题量
        /// </summary>
        private int SingleMax;

        /// <summary>
        /// 填空题最大题量
        /// </summary>
        private int GapMax;

        /// <summary>
        /// 添加按钮状态
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                if (ViewModelLocator.User.Admin == 1)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 试卷名称
        /// </summary>
        private string _testname;
        public string TestName
        {
            get { return _testname; }
            set { Set(ref _testname, value); }
        }

        /// <summary>
        /// 选择题类型
        /// </summary>
        private List<string> _singleTypeList = new List<string>();
        public List<string> SingleTypeList
        {
            get { return _singleTypeList; }
            set { Set(ref _singleTypeList, value); }
        }

        /// <summary>
        /// 选择题难度
        /// </summary>
        private List<string> _singleLevel = new List<string>();
        public List<string> SingleLevel
        {
            get { return _singleLevel; }
            set { Set(ref _singleLevel, value); }
        }

        /// <summary>
        /// 选择题数量
        /// </summary>
        private int _singleNum;
        public int SingleNum
        {
            get { return _singleNum; }
            set { Set(ref _singleNum, value); }
        }

        /// <summary>
        /// 选择题类型索引
        /// </summary>
        private int _singleTypeNum;
        public int SingleTypeNum
        {
            get { return _singleTypeNum; }
            set
            {
                Set(ref _singleTypeNum, value);
            }
        }

        /// <summary>
        /// 选择题难度索引
        /// </summary>
        private int _singleLevelNum;
        public int SingleLevelNum
        {
            get { return _singleLevelNum; }
            set { Set(ref _singleLevelNum, value); }
        }

        /// <summary>
        /// 填空题类型
        /// </summary>
        private List<string> _gapTypeList = new List<string>();
        public List<string> GapTypeList
        {
            get { return _gapTypeList; }
            set { Set(ref _gapTypeList, value); }
        }

        /// <summary>
        /// 填空题难度
        /// </summary>
        private List<string> _gapLevel = new List<string>();
        public List<string> GapLevel
        {
            get { return _gapLevel; }
            set { Set(ref _gapLevel, value); }
        }

        /// <summary>
        /// 填空题数量
        /// </summary>
        private int _gapNum;
        public int GapNum
        {
            get { return _gapNum; }
            set { Set(ref _gapNum, value); }
        }

        /// <summary>
        /// 填空题类型索引
        /// </summary>
        private int _gapTypeNum;
        public int GapTypeNum
        {
            get { return _gapTypeNum; }
            set
            {
                Set(ref _gapTypeNum, value);
            }
        }

        /// <summary>
        /// 填空题难度索引
        /// </summary>
        private int _gapLevelNum;
        public int GapLevelNum
        {
            get { return _gapLevelNum; }
            set { Set(ref _gapLevelNum, value); }
        }

        /// <summary>
        /// 弹出菜单状态
        /// </summary>
        private bool _paneopen;
        public bool PaneOpen
        {
            get { return _paneopen; }
            set { Set(ref _paneopen, value); }
        }

        /// <summary>
        /// 试卷列表索引
        /// </summary>
        private int _test;
        public int Test
        {
            get { return _test; }
            set { Set(ref _test, value); }
        }

        /// <summary>
        /// 试卷列表
        /// </summary>
        private List<TestPaper> _testpapaerlist;
        public List<TestPaper> TestList
        {
            get { return _testpapaerlist; }
            set { Set(ref _testpapaerlist, value); }
        }

        /// <summary>
        /// 开始答题
        /// </summary>
        public ICommand RunCommand { get; set; }
        private void run()
        {
            var sdb = new DbSingleService();
            string _singlequest = TestList[Test].SingleQuestionNum;
            string _gapquest = TestList[Test].GapQuestionNum;
            string _singlesql = " where Id in (" + _singlequest.Substring(1, _singlequest.Length - 2) + ")";
            List<SingleChoice> _SingleList = (List<SingleChoice>)sdb.Querysql("*", _singlesql);

            var gdb = new DbGapService();
            string _gapsql = " where Id in (" + _gapquest.Substring(1, _gapquest.Length - 2) + ")";
            List<GapFilling> _GapList = (List<GapFilling>)gdb.QuerySql("*", _gapsql);

            ViewModelLocator.SinglePaper.SingleList = _SingleList;
            ViewModelLocator.SinglePaper.Mode = 1;
            ViewModelLocator.SinglePaper.Init();
            ViewModelLocator.GapPaper.GapList = _GapList;
            ViewModelLocator.GapPaper.Mode = 1;
            ViewModelLocator.GapPaper.Init();
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Single");
        }

        /// <summary>
        /// 打开弹出菜单
        /// </summary>
        public ICommand OpenCommand { get; set; }
        private void Open()
        {
            PaneOpen = !PaneOpen;
        }

        /// <summary>
        /// 添加新试卷
        /// </summary>
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
                        ReadData();
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

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            ReadData();
            GetQuestType();
            GetQuestLevel();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TestPaperModel()
        {
            Init();
            RunCommand = new RelayCommand(run);
            OpenCommand = new RelayCommand(Open);
            AddCommand = new RelayCommand(Add);
        }


        /// <summary>
        /// 读取数据
        /// </summary>
        private void ReadData()
        {
            var db = new DbTestService();
            TestList = (List<TestPaper>)db.QueryObject();
        }

        /// <summary>
        /// 获取题目类型
        /// </summary>
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

        /// <summary>
        /// 获取题目难度
        /// </summary>
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

            
        }

        /// <summary>
        /// 获取题目数量
        /// </summary>
        private void GetQuestNum()
        {
            var gdb = new DbGapService();
            var gnewstr = new[] {GapTypeList[GapTypeNum], GapLevel[GapLevelNum] };
            GapMax = (int)gdb.Query(gnewstr);
            var sdb = new DbSingleService();
            var snewstr = new[] {SingleTypeList[SingleTypeNum], SingleLevel[SingleLevelNum] };
            SingleMax = (int)sdb.Query(snewstr);
        }

    }
}
