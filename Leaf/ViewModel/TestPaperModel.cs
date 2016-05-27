﻿using GalaSoft.MvvmLight;
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

        private List<string> _subjectList = new List<string>();
        public List<string> SubjectList
        {
            get { return _subjectList; }
            set { Set(ref _subjectList, value); }
        }

        /// <summary>
        /// 类型
        /// </summary>
        private List<string> _typeList = new List<string>();
        public List<string> TypeList
        {
            get { return _typeList; }
            set { Set(ref _typeList, value); }
        }

        /// <summary>
        /// 难度
        /// </summary>
        private List<string> _levelList = new List<string>();
        public List<string> LevelList
        {
            get { return _levelList; }
            set { Set(ref _levelList, value); }
        }

        /// <summary>
        /// 选择题数量
        /// </summary>
        private int _singleNum=5;
        public int SingleNum
        {
            get { return _singleNum; }
            set { Set(ref _singleNum, value); }
        }

        /// <summary>
        /// 类型索引
        /// </summary>
        private int _typeIndex;
        public int TypeIndex
        {
            get { return _typeIndex; }
            set
            {
                Set(ref _typeIndex, value);
            }
        }

        /// <summary>
        /// 难度索引
        /// </summary>
        private int _levelIndex;
        public int LevelIndex
        {
            get { return _levelIndex; }
            set { Set(ref _levelIndex, value); }
        }


        /// <summary>
        /// 填空题数量
        /// </summary>
        private int _gapNum=5;
        public int GapNum
        {
            get { return _gapNum; }
            set { Set(ref _gapNum, value); }
        }

        /// <summary>
        /// 题库包索引
        /// </summary>
        private int _subjectIndex;
        public int SubjectIndex
        {
            get { return _subjectIndex; }
            set
            {
                Set(ref _subjectIndex, value);
            }
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
            if (Test < 0 || TestList.Count == 0)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("请先选择试卷", "RunNo");
                return;
            }
            else
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
                ViewModelLocator.GapPaper.GapList = _GapList;
                ViewModelLocator.GapPaper.Mode = 1;
                ViewModelLocator.TestResult.TestPaperModel = TestList[Test];
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                navigation.NavigateTo("Single");
            }
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
            if(SubjectIndex >0 && TypeIndex >0 && LevelIndex >0)
            {
                GetQuestNum();
                if(GapNum>GapMax)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("该类型和难度下填空题没有足够数量题目，只有"+GapMax.ToString(), "AddNo");
                }
                else if(SingleNum>SingleMax)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("该类型和难度下选择题没有足够数量题目，只有" + SingleMax.ToString(), "AddNo");
                }
                else
                {
                    TestPaper model = new TestPaper();
                    var sdb = new DbSingleService();
                    var gdb = new DbGapService();
                    var snewstr = new[] {"Id",TypeList[TypeIndex],LevelList[LevelIndex],SubjectList[SubjectIndex],SingleNum.ToString() };
                    var gnewstr = new[] { "Id", TypeList[TypeIndex], LevelList[LevelIndex], SubjectList[SubjectIndex], GapNum.ToString() };
                    List<SingleChoice> _singlelist = (List<SingleChoice>)sdb.QueryObject(snewstr);
                    List<GapFilling> _gaplist = (List<GapFilling>)gdb.QueryObject(gnewstr);
                    var _newsinglenum = new List<int>();
                    for(int i=0;i<_singlelist.Count;i++)
                    {
                        _newsinglenum.Add(_singlelist[i].Id);
                    }
                    model.SingleQuestionNum= JsonConvert.SerializeObject(_newsinglenum);
                    model.SingleNum = SingleNum;
                    var _newgapnum = new List<int>();
                    for (int i = 0; i < _gaplist.Count; i++)
                    {
                        _newgapnum.Add(_gaplist[i].Id);
                    }
                    model.GapQuestionNum = JsonConvert.SerializeObject(_newgapnum);
                    model.GapNum = GapNum;
                    model.Level = Convert.ToInt32(LevelList[LevelIndex]);
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
            GetQuestSubjec();
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
        /// 获取题目类型难度等
        /// </summary>
        private void GetQuestType()
        {
            if(TypeList == null || TypeList.Count == 0)
            {
                TypeList.Add(" ");
                var gdb = new DbGapService();
                var sdb = new DbSingleService();
                var newstr = new[] { "distinct singlechoice.type", ",GapFilling" };
                List<SingleChoice> _modellist = (List<SingleChoice>)sdb.Querysql(newstr);
                foreach (var _model in _modellist)
                {
                    TypeList.Add(_model.Type);
                }
            }
        }

        /// <summary>
        /// 获取题目难度
        /// </summary>
        private void GetQuestLevel()
        {

            if (LevelList == null || LevelList.Count == 0)
            {
                LevelList.Add(" ");
                var gdb = new DbGapService();
                var sdb = new DbSingleService();
                var newstr = new[] { "distinct singlechoice.level", ",GapFilling" };
                List<SingleChoice> _modellist = (List<SingleChoice>)sdb.Querysql(newstr);
                foreach (var _model in _modellist)
                {
                    LevelList.Add(_model.Level.ToString());
                }
            }
        }
        /// <summary>
        /// 获取题目主题
        /// </summary>
        private void GetQuestSubjec()
        {
            if(SubjectList==null || SubjectList.Count==0)
            {
                SubjectList.Add(" ");
                var gdb = new DbGapService();
                var sdb = new DbSingleService();
                var newstr = new[] { "distinct singlechoice.Subject", ",GapFilling" };
                List<SingleChoice> _modellist = (List<SingleChoice>)sdb.Querysql(newstr);
                foreach (var _model in _modellist)
                {
                    SubjectList.Add(_model.Subject);
                }
            }
        }

        /// <summary>
        /// 获取题目数量
        /// </summary>
        private void GetQuestNum()
        {
            var gdb = new DbGapService();
            var sdb = new DbSingleService();
            var newstr = new[] { TypeList[TypeIndex], LevelList[LevelIndex], SubjectList[SubjectIndex] };
            GapMax = (int)gdb.Query(newstr);
            SingleMax = (int)sdb.Query(newstr);
        }

    }
}