using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Leaf.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Leaf.ViewModel
{
    class TestPaperModel : ViewModelBase
    {

        TimeSpan timespan;
        private DispatcherTimer timer;
        
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
        /// 时间
        /// </summary>
        private int _time;
        public int Time
        {
            get { return _time; }
            set { Set(ref _time, value); }
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
        private List<int> _levelList = new List<int>();
        public List<int> LevelList
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
                List<GapFilling> _GapList = new List<GapFilling>(); //TestList[Test].gapfills.ToList();
                List<SingleChoice> _SingleList = new List<SingleChoice>(); //TestList[Test].singles.ToList();
                var _gaplist = TestList[Test].gapfills;
                var _singlelist = TestList[Test].singles;
                foreach (var x in _singlelist)
                    _SingleList.Add(x.single);
                foreach (var x in _gaplist)
                    _GapList.Add(x.gap);


                ViewModelLocator.SinglePaper.SingleList = _SingleList;
                ViewModelLocator.SinglePaper.Mode = 1;
                ViewModelLocator.GapPaper.GapList = _GapList;
                ViewModelLocator.GapPaper.Mode = 1;
                ViewModelLocator.TestResult.Clear();
                ViewModelLocator.TestResult.TestPaperModel = TestList[Test];
                ViewModelLocator.SinglePaper.Init();
                ViewModelLocator.GapPaper.Init();
                TestTime();
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
            if (PaneOpen)
            {
                GetQuestType();
                GetQuestLevel();
                GetQuestSubjec();
            }
        }

        /// <summary>
        /// 添加新试卷
        /// </summary>
        public ICommand AddCommand { get; set; }
        private void Add()
        {
            //判断相关选项是否都选中了
            if(SubjectIndex >=0 && TypeIndex >=0 && LevelIndex >=0)
            {
                //获取相关选项下题目最大数量
                GetQuestNum();
                //如果填写的题目大于最大题目，发出错误提示
                if (GapNum > GapMax)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("该类型和难度下填空题没有足够数量题目，只有" + GapMax.ToString(), "AddNo");
                }
                else if (SingleNum > SingleMax)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("该类型和难度下选择题没有足够数量题目，只有" + SingleMax.ToString(), "AddNo");
                }
                else
                {
                    TestPaper model = new TestPaper();
                    List<SingleChoice> _singlelist;
                    List<GapFilling> _gaplist;

                    //检索习题
                    using (var mydb = new MyDBContext())
                    {
                        //检索符合要求的所有单选题
                        var _singles = from c in mydb.SingleChoices
                                       where c.Type == TypeList[TypeIndex] &&
                                       c.Level == LevelList[LevelIndex] &&
                                       c.Subject == SubjectList[SubjectIndex]
                                       select c;
                        //随机选择习题
                        _singlelist = _singles.OrderBy(s => Guid.NewGuid()).Take(SingleNum).ToList();

                        //检索符合要求的所有填空题
                        var _gapfills = from c in mydb.GapFillings
                                        where c.Type == TypeList[TypeIndex] &&
                                        c.Level == LevelList[LevelIndex] &&
                                        c.Subject == SubjectList[SubjectIndex]
                                        select c;
                        //随机选择习题
                        _gaplist = _gapfills.OrderBy(s => Guid.NewGuid()).Take(GapNum).ToList();
                    }


                    //设置试卷相关属性
                    model.Level = Convert.ToInt32(LevelList[LevelIndex]);
                    model.Name = TestName;
                    model.BuildTime = DateTime.Now.ToString();
                    model.Time = Time;
                    model.SingleNum = SingleNum;
                    model.GapNum = GapNum;

                    int n;
                    //添加试卷
                    using (var mydb = new MyDBContext())
                    {
                        //添加试卷
                        mydb.TestPapers.Add(model);
                        n = mydb.SaveChanges();

                        //循环添加习题
                        foreach (var s in _singlelist)
                        {
                            SingleTest sm = new SingleTest();
                            sm.SingleId = s.Id;
                            sm.TestId = model.Id;
                            mydb.SingleTest.Add(sm);
                        }
                        foreach (var s in _gaplist)
                        {
                            GapTest gm = new GapTest();
                            gm.GapId = s.Id;
                            gm.TestId = model.Id;
                            mydb.GapTest.Add(gm);
                        }
                        n = mydb.SaveChanges();
                        
                    }
                    //如果添加成功则刷新试卷列表
                    if (n > 0)
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
        /// 读取试卷列表
        /// </summary>
        private void ReadData()
        {

            using (var mydb = new MyDBContext())
            {
                //读取所有试卷并贪婪加载所有习题
                TestList= mydb.TestPapers.Include(p => p.gapfills)
                    .ThenInclude(z => z.gap)
                    .Include(p => p.singles)
                    .ThenInclude(z => z.single).ToList();
            }
        }

        /// <summary>
        /// 获取题目类型难度等
        /// </summary>
        private void GetQuestType()
        {
            if(TypeList == null || TypeList.Count == 0)
            {
                TypeList.Add(" ");
                using (var mydb = new MyDBContext())
                {
                    var q = from c in mydb.SingleChoices
                            select c.Type;
                    var qq = from c in mydb.GapFillings
                             select c.Type;

                    var qqq = q.ToList();
                    qqq.AddRange(qq.ToList());
                    TypeList = qqq.Distinct().ToList();

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

                using (var mydb = new MyDBContext())
                {
                    var q = from c in mydb.SingleChoices
                            select c.Level;
                    var qq = from c in mydb.GapFillings
                             select c.Level;

                    var qqq = q.ToList();
                    qqq.AddRange(qq.ToList());
                    LevelList = qqq.Distinct().ToList();
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

                using (var mydb = new MyDBContext())
                {
                    var q = from c in mydb.SingleChoices
                            select c.Subject;
                    var qq = from c in mydb.GapFillings
                             select c.Subject;

                    var qqq = q.ToList();
                    qqq.AddRange(qq.ToList());
                    SubjectList = qqq.Distinct().ToList();
                }

            }
        }

        /// <summary>
        /// 获取题目数量
        /// </summary>
        private void GetQuestNum()
        {

            using (var mydb = new MyDBContext())
            {
                GapMax = (from c in mydb.GapFillings
                          where c.Subject == SubjectList[SubjectIndex] &&
                          c.Type == TypeList[TypeIndex] &&
                          c.Level == LevelList[LevelIndex]
                          select c).Count();
                SingleMax = (from c in mydb.SingleChoices
                          where c.Subject == SubjectList[SubjectIndex] &&
                          c.Type == TypeList[TypeIndex] &&
                          c.Level == LevelList[LevelIndex]
                          select c).Count();
            }

        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        public void TimerStop()
        {
            timer.Tick -= TimeChick;
            timer.Stop();
        }
        
        /// <summary>
        /// 设置定时器
        /// </summary>
        private void TestTime()
        {
            if (timer != null)
            {
                timer.Stop();
            }
            //新建定时器
            timer = new DispatcherTimer();
            //设定定时器总时间
            timespan = new TimeSpan(0, TestList[Test].Time, 0);
            //每秒触发事件
            timer.Interval = new TimeSpan(0, 0, 1);
            //绑定事件函数
            timer.Tick += TimeChick;
            timer.Start();
        }
        
        /// <summary>
        /// 定时器回调函数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="e"></param>
        private void TimeChick(object state,object e)
        {
            //构造要显示的字符串
            string str =timespan.Minutes.ToString() + ":" + timespan.Seconds.ToString();
            //将填空题和选择题答题页面都显示时间字符串
            ViewModelLocator.SinglePaper.Time = str;
            ViewModelLocator.GapPaper.Time = str;
            //如果时间到了，则终止定时器并自动跳转到成绩页面
            timespan = timespan.Subtract(new TimeSpan(0, 0, 1));
            if(timespan.TotalSeconds == 0.0)
            {
                timer.Tick -= TimeChick;
                timer.Stop();
                ViewModelLocator.TestResult.Init();
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                navigation.NavigateTo("Main");
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "MainFrame", "Result" }, "NavigateTo");
                return;
            }
        }

    }
}
