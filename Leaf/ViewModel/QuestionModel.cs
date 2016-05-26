using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaf.Model;
using Leaf.SQLite;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Views;

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
            var gdb =new DbGapService();
            var sdb =new DbSingleService();
            var newstr = new[] {"*",QuestionList[QuestionIndex].Type,QuestionList[QuestionIndex].Level.ToString(),"5" };
            ViewModelLocator.SinglePaper.SingleList =(List<SingleChoice>)sdb.QueryObject(newstr);
            ViewModelLocator.GapPaper.GapList = (List<GapFilling>)gdb.QueryObject(newstr);
            ViewModelLocator.SinglePaper.Mode = 0;
            ViewModelLocator.GapPaper.Mode = 0;
            //ViewModelLocator.GapPaper.Init();
            //ViewModelLocator.SinglePaper.Init();
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Single");
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QuestionModel()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            ReadData();
            ToTest = new RelayCommand(test);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        private void ReadData()
        {
            var gdb = new DbGapService();
            var sdb = new DbSingleService();
            var newstr =new[] { "distinct singlechoice.type,singlechoice.Level", ",GapFilling" };
            List<SingleChoice> _typelist = (List<SingleChoice>)sdb.Querysql(newstr);
            for(int i=0;i<_typelist.Count;i++)
            {
                QuestionView model = new QuestionView();
                model.Level = _typelist[i].Level;
                model.Type = _typelist[i].Type;
                var sql = new[] { _typelist[i].Type, _typelist[i].Level.ToString() };
                model.GapNum = (int)gdb.Query(sql);
                model.SingleNum = (int)sdb.Query(sql);
                switch(_typelist[i].Level)
                {
                    case 1:
                        {
                            model.Color = "#00FF00";
                        }
                        break;
                    case 2:
                        {
                            model.Color = "#008B00";
                        }break;
                    case 3:
                        {
                            model.Color = "#CDCD00";
                        }break;
                    default:break;
                }
                QuestionList.Add(model);
            }

        }
    }
}
