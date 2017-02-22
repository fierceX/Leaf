using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using Leaf.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Leaf.ViewModel
{
    class QuestionModel : ViewModelBase
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
            if(QuestionList==null || QuestionList.Count==0 || QuestionIndex < 0)
            {
                return;
            }
            //var gdb =new DbGapService();
            //var sdb =new DbSingleService();
            //var newstr = new[] {"*",QuestionList[QuestionIndex].Type,QuestionList[QuestionIndex].Level.ToString(),QuestionList[QuestionIndex].Subject,"5" };
            //ViewModelLocator.SinglePaper.SingleList =(List<SingleChoice>)sdb.QueryObject(newstr);
            //ViewModelLocator.GapPaper.GapList = (List<GapFilling>)gdb.QueryObject(newstr);
            using (var mydb = new MyDBContext())
            {
                var _singles = from c in mydb.SingleChoices
                        where c.Type == QuestionList[QuestionIndex].Type &&
                        c.Level == QuestionList[QuestionIndex].Level &&
                        c.Subject == QuestionList[QuestionIndex].Subject
                        select c;

                ViewModelLocator.SinglePaper.SingleList = _singles.OrderBy(s => Guid.NewGuid()).Take(5).ToList();
                
                var _gapfills = from c in mydb.GapFillings
                        where c.Type == QuestionList[QuestionIndex].Type &&
                        c.Level == QuestionList[QuestionIndex].Level &&
                        c.Subject == QuestionList[QuestionIndex].Subject
                        select c;

                ViewModelLocator.GapPaper.GapList = _gapfills.OrderBy(s => Guid.NewGuid()).Take(5).ToList();
            }
            ViewModelLocator.SinglePaper.Mode = 0;
            ViewModelLocator.GapPaper.Mode = 0;
            ViewModelLocator.SinglePaper.Init();
            ViewModelLocator.GapPaper.Init();
            //var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            //navigation.NavigateTo("Single");
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
            if (QuestionList != null || QuestionList.Count != 0)
                QuestionList.Clear();
            //var gdb = new DbGapService();
            //var sdb = new DbSingleService();
            //var newstr =new[] { "distinct singlechoice.type,singlechoice.Level,singlechoice.Subject", ",GapFilling" };
            //List<SingleChoice> _typelist = (List<SingleChoice>)sdb.Querysql(newstr);
            //List<QuestionView> _viewlist = new List<QuestionView>();
            using (var mydb = new MyDBContext())
            {
                var _singleTLS = from c in mydb.SingleChoices
                                 select new
                                 {
                                     Type = c.Type,
                                     Level = c.Level,
                                     Subject = c.Subject,
                                 };

                var _gapfillTLS = from c in mydb.GapFillings
                                  select new
                                  {
                                      Type = c.Type,
                                      Level = c.Level,
                                      Subject = c.Subject,
                                  };
                var TLSlist = _singleTLS.ToList();
                TLSlist.AddRange(_gapfillTLS.ToList());
                var _tlslist = TLSlist.Distinct().ToList();
                foreach (var c in _tlslist)
                {
                    QuestionView model = new QuestionView();
                    model.Level = c.Level;
                    model.Type = c.Type;
                    model.Subject = c.Subject;
                    var singlenum = (from e in mydb.SingleChoices
                                     where e.Type == c.Type &&
                                     e.Level == c.Level &&
                                     e.Subject == c.Subject
                                     select e).Count();
                    var gapfillnum = (from e in mydb.GapFillings
                                      where e.Type == c.Type &&
                                      e.Level == c.Level &&
                                      e.Subject == c.Subject
                                      select e).Count();
                    model.SingleNum = singlenum;
                    model.GapNum = gapfillnum;
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
                //for(int i=0;i<_typelist.Count;i++)
                //{
                //    //QuestionView model = new QuestionView();
                //    //model.Level = _typelist[i].Level;
                //    //model.Type = _typelist[i].Type;
                //    //model.Subject = _typelist[i].Subject;
                //    //var sql = new[] { _typelist[i].Type, _typelist[i].Level.ToString(),_typelist[i].Subject };
                //    //model.GapNum = (int)gdb.Query(sql);
                //    //model.SingleNum = (int)sdb.Query(sql);
                //    //switch(_typelist[i].Level)
                //    //{
                //    //    case 1:
                //    //        {
                //    //            model.Color = "#00FF00";
                //    //        }
                //    //        break;
                //    //    case 2:
                //    //        {
                //    //            model.Color = "#008B00";
                //    //        }break;
                //    //    case 3:
                //    //        {
                //    //            model.Color = "#CDCD00";
                //    //        }break;
                //    //    default:break;
                //    //}
                //    //QuestionList.Add(model);
                //}

            }
        }
    }
}
