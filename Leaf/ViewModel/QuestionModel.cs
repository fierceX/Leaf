using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Leaf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Leaf.ViewModel
{
    internal class QuestionModel : ViewModelBase
    {
        /// <summary>
        /// 题目列表
        /// </summary>
        private List<QuestionView> _questionlist = new List<QuestionView>();

        public List<QuestionView> QuestionList
        {
            get { return _questionlist; }
            set { Set(ref _questionlist, value); }
        }

        /// <summary>
        /// 选中题目索引
        /// </summary>
        private int _questionIndex;

        public int QuestionIndex
        {
            get { return _questionIndex; }
            set { Set(ref _questionIndex, value); }
        }

        /// <summary>
        /// 命令
        /// 开始练习
        /// </summary>
        public ICommand ToTest { get; set; }

        private void test()
        {
            //判断习题列表是否为空
            if (QuestionList == null || QuestionList.Count == 0 || QuestionIndex < 0)
            {
                return;
            }

            //获取所选习题库内的习题
            using (var mydb = new MyDBContext())
            {
                //检索所有符合条件的单选题
                var _singles = from c in mydb.SingleChoices
                               where c.Type == QuestionList[QuestionIndex].Type &&
                               c.Level == QuestionList[QuestionIndex].Level &&
                               c.Subject == QuestionList[QuestionIndex].Subject
                               select c;
                //随机选择5个
                ViewModelLocator.SinglePaper.SingleList = _singles.OrderBy(s => Guid.NewGuid()).Take(5).ToList();

                //检索所有符合条件的填空题
                var _gapfills = from c in mydb.GapFillings
                                where c.Type == QuestionList[QuestionIndex].Type &&
                                c.Level == QuestionList[QuestionIndex].Level &&
                                c.Subject == QuestionList[QuestionIndex].Subject
                                select c;
                //随机选择5个
                ViewModelLocator.GapPaper.GapList = _gapfills.OrderBy(s => Guid.NewGuid()).Take(5).ToList();
            }
            //设置相关状态并初始化
            ViewModelLocator.SinglePaper.Mode = 0;
            ViewModelLocator.GapPaper.Mode = 0;
            ViewModelLocator.SinglePaper.Init();
            ViewModelLocator.GapPaper.Init();
            //跳转到答题页面
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "RootFrame", "Single" }, "NavigateTo");
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QuestionModel()
        {
            Init();
            ToTest = new RelayCommand(test);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            ReadData();
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        private void ReadData()
        {
            //查询当前习题列表是否为空，不为空则清空
            if (QuestionList != null || QuestionList.Count != 0)
                QuestionList.Clear();

            //检索习题
            using (var mydb = new MyDBContext())
            {
                //检索所有单选题的类型，难度，主题
                var _singleTLS = from c in mydb.SingleChoices
                                 select new
                                 {
                                     Type = c.Type,
                                     Level = c.Level,
                                     Subject = c.Subject,
                                 };

                //检索所有填空题的类型，难度，主题
                var _gapfillTLS = from c in mydb.GapFillings
                                  select new
                                  {
                                      Type = c.Type,
                                      Level = c.Level,
                                      Subject = c.Subject,
                                  };

                //两个合在一起并去重
                var TLSlist = _singleTLS.ToList();
                TLSlist.AddRange(_gapfillTLS.ToList());
                var _tlslist = TLSlist.Distinct().ToList();

                //遍历所有难度类型主题检索相关习题量
                foreach (var c in _tlslist)
                {
                    QuestionView model = new QuestionView();
                    model.Level = c.Level;
                    model.Type = c.Type;
                    model.Subject = c.Subject;

                    //单选题数量
                    var singlenum = (from e in mydb.SingleChoices
                                     where e.Type == c.Type &&
                                     e.Level == c.Level &&
                                     e.Subject == c.Subject
                                     select e).Count();

                    //填空题数量
                    var gapfillnum = (from e in mydb.GapFillings
                                      where e.Type == c.Type &&
                                      e.Level == c.Level &&
                                      e.Subject == c.Subject
                                      select e).Count();
                    model.SingleNum = singlenum;
                    model.GapNum = gapfillnum;

                    //根据不同难度设置不同颜色
                    switch (c.Level)
                    {
                        case 1:
                            {
                                model.Color = "#00FF00";
                            }
                            break;

                        case 2:
                            {
                                model.Color = "#008B00";
                            }
                            break;

                        case 3:
                            {
                                model.Color = "#CDCD00";
                            }
                            break;

                        default: break;
                    }
                    QuestionList.Add(model);
                }
            }
        }
    }
}