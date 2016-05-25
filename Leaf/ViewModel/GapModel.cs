using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Leaf.Model;
using Leaf.SQLite;
using Microsoft.Practices.ServiceLocation;
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

        /// <summary>
        /// 模式
        /// </summary>
        public int Mode;

        /// <summary>
        /// 题目数量
        /// </summary>
        private int max = 0;

        /// <summary>
        /// 当前题目
        /// </summary>
        private int num = 0;

        /// <summary>
        /// 题目列表
        /// </summary>
        public List<GapFilling> GapList = new List<GapFilling>();

        /// <summary>
        /// 成绩单
        /// </summary>
        private List<bool> Result = new List<bool>();


        /// <summary>
        /// 题干
        /// </summary>
        private string _stem;
        public string Stem
        {
            get { return _stem; }
            set
            {
                Set(ref _stem, value);
            }
        }

        /// <summary>
        /// 答案
        /// </summary>
        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set { Set(ref _answer, value); }
        }


        /// <summary>
        /// 命令
        /// /////
        /// 继续
        /// </summary>
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
                if (Mode == 1)
                {
                    ViewModelLocator.TestResult.GapResult = Result;
                    Result.Clear();
                    num = 0;
                    max = 0;
                    var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                    navigation.NavigateTo("Gap");
                }
                else
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<List<bool>>(Result, "GapEnd");
                    Result.Clear();
                    num = 0;
                    max = 0;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GapModel()
        {
            Init();
            ContinueCommand = new RelayCommand(Continue);
        }



        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            max = GapList.Count;
            if (num >= GapList.Count)
            {
                return;
            }
            Stem = GapList[num].Stems;
        }

        /// <summary>
        /// 获取答案
        /// </summary>
        /// <returns></returns>
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
