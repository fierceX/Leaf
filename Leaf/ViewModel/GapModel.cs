using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Leaf.Model;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using System.Windows.Input;

namespace Leaf.ViewModel
{
    class GapModel : ViewModelBase
    {

        private bool ContinueBool = true;

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
        /// 剩余时间
        /// </summary>
        private string _time;
        public string Time
        {
            get { return _time; }
            set { Set(ref _time, value); }
        }

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

        private string _answerright;
        public string RightAnswer
        {
            get { return _answerright; }
            set { Set(ref _answerright, value); }
        }
        /// <summary>
        /// 命令
        /// /////
        /// 继续
        /// </summary>
        public ICommand ContinueCommand { get; set; }
        private void Continue()
        {
            if (ContinueBool && Mode == 0)
            {
                RightAnswer = "正确答案是：" + GapList[num].Answer;
                ContinueBool = false;
                return;
            }
            else
            {
                num++;
                ContinueBool = true;
                RightAnswer = "";
            }
            if (Mode == 1)
                ViewModelLocator.TestResult.GapResult.Add(GetAnswer());
            if (num<max)
            {
                Stem = GapList[num].Stems;
                Answer = "";
            }
            else
            {
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                if (Mode == 1)
                {
                    ViewModelLocator.TestResult.Init();
                    num = 0;
                    max = 0;
                    ViewModelLocator.TestPaper.TimerStop();
                    navigation.NavigateTo("Main");
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "MainFrame", "Result" }, "NavigateTo");
                }
                else
                {
                    navigation.NavigateTo("Main");
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "MainFrame", "Question" }, "NavigateTo");
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
            num = 0;
            if (Mode != 1)
                Time = "";
            if (num >= max)
                return;
            Stem = GapList[num].Stems;
            Answer = "";
        }

        

        /// <summary>
        /// 获取答案
        /// </summary>
        /// <returns></returns>
        private bool GetAnswer()
        {
            if (Answer == null || Answer == "" || Answer.Trim() == "")
                return false;
            if (Answer.ToUpper().Trim() == GapList[num-1].Answer.ToUpper())
                return true;
            return false;
        }

    }
}
