using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Leaf.Model;
using Leaf.SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Leaf.ViewModel
{
    class GapModel : ViewModelBase
    {

        //题目数量
        private int max = 0;
        //当前题目
        private int num = 0;
        //题目列表
        private List<GapFilling> GapList = new List<GapFilling>();
        //成绩单
        private List<bool> Result = new List<bool>();


        //题干
        private string _stem;
        public string Stem
        {
            get { return _stem; }
            set
            {
                Set(ref _stem, value);
            }
        }

        //答案
        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set { Set(ref _answer, value); }
        }


        //命令
        //继续
        public ICommand ContinueCommand { get; set; }
        private void Continue()
        {
            if (Result.Count <= max)
            {
                Result.Add(GetAnswer());
                num++;
            }
            if (num < max)
            {
                Init();
                Answer = "";
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<List<bool>>(Result, "GapEnd");
            }
        }

        public GapModel()
        {
            ResetTest();
            ContinueCommand = new RelayCommand(Continue);
            ResetCommand = new RelayCommand(ResetTest);
            max = GapList.Count;
        }



        //初始化
        private void Init()
        {
            if (num >= GapList.Count)
            {
                return;
            }
            Stem = GapList[num].Stems;
        }

        //测试
        public ICommand ResetCommand { get; set; }
        private void ResetTest()
        {
            max = 0;
            num = 0;
            Result.Clear();
            GapList.Clear();
            InsertTestData();   // TODO 测试用函数
            ReadTestData();     // TODO 测试用函数
            max = GapList.Count;
            Init();
        }


        /// <summary>
        /// 测试用读取
        /// </summary>
        private void ReadTestData()
        {
            if (GapList == null || GapList.Count == 0)
            {
                var db = new DbGapService();
                var newStr = new[] { "a", "1" };
                GapList = (List<GapFilling>)db.Query(newStr);
            }
        }

        /// <summary>
        /// 测试用插入
        /// </summary>
        private void InsertTestData()
        {
            var db = new DbGapService();
            if (db.QueryNum() > 10)
            {
                return;
            }
            var answers = new[] { 0, 2, 1, 0, 2, 1, 1, 1, 0, 2 };
            foreach (int t in answers)
            {
                var stem = "此习题所要填的内容为（  ）（答案为 "+t+")";
                var answer = t.ToString();
                var level = 1;
                var type = "a";
                var model = new GapFilling { Answer = answer, Level = level, Stems = stem, Type = type };
                var i = db.Insert(model);
                if (i > 0)
                {
                    Debug.WriteLine("yooooo, 加入成功了");
                }
            }
        }

        //获取答案
        private bool GetAnswer()
        {
            if (Answer == null || Answer == "" || Answer.Trim() == "")
                return false;
            if (Answer.ToUpper().Trim() == GapList[num].Answer.ToUpper())
                return true;
            return false;
        }

    }
}
