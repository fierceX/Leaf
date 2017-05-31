using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Leaf.Model;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Leaf.ViewModel
{
    internal class GapModel : ViewModelBase
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
        private bool _answer;

        public bool Answer
        {
            get { return _answer; }
            set { Set(ref _answer, value); }
        }

        private bool _answerno;

        public bool AnswerNo
        {
            get { return _answerno; }
            set { Set(ref _answerno, value); }
        }

        private string _answerright;

        public string RightAnswer
        {
            get { return _answerright; }
            set { Set(ref _answerright, value); }
        }

        /// <summary>
        /// 题目配图
        /// </summary>
        private BitmapImage _img;

        public BitmapImage Img
        {
            get { return _img; }
            set { Set(ref _img, value); }
        }

        /// <summary>
        /// 命令
        /// /////
        /// 继续
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        private void Continue()
        {
            if (GapList.Count > 0)
            {
                //判断是不是练习模式，如果是则显示答案
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
                //如果是测试模式，则获取答案
                if (Mode == 1)
                    ViewModelLocator.TestResult.GapResult.Add(GetAnswer());
                //如果还有题目，则显示下一题并清空答案
                if (num < max)
                {
                    LoadQuestionAsync();
                }
                //否则就跳转回去
                else
                {
                    Stem = "没有判断题，点击跳转至下一页面！";
                    Answer = false;
                    AnswerNo = false;
                    Img = new BitmapImage();
                    var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                    //如果是测试模式，则跳转到成绩页面
                    if (Mode == 1)
                    {
                        ViewModelLocator.TestResult.Init();
                        num = 0;
                        max = 0;
                        ViewModelLocator.TestPaper.TimerStop();
                        navigation.NavigateTo("Main");
                        GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "MainFrame", "Result" }, "NavigateTo");
                    }
                    //否则跳回主页
                    else
                    {
                        navigation.NavigateTo("Main");
                        GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "MainFrame", "Question" }, "NavigateTo");
                        num = 0;
                        max = 0;
                    }
                }
            }
            else
            {
                num = 0;
                max = 0;
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                if (Mode == 1)
                {
                    ViewModelLocator.TestResult.Init();
                    ViewModelLocator.TestPaper.TimerStop();
                    navigation.NavigateTo("Main");
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "MainFrame", "Result" }, "NavigateTo");
                }
                else
                {
                    navigation.NavigateTo("Main");
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "MainFrame", "Question" }, "NavigateTo");
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
            if (_img == null)
                _img = new BitmapImage();
            if (Mode != 1)
                Time = "";

            LoadQuestionAsync();
        }

        private async System.Threading.Tasks.Task LoadQuestionAsync()
        {
            if (num >= max)
            {
                Stem = "没有判断题，点击跳转至下一页面！";
                Answer = false;
                AnswerNo = false;
                Img = new BitmapImage();
                return;
            }
            Stem = GapList[num].Stems;
            Answer = false;
            AnswerNo = false;
            Img = new BitmapImage();

            if (GapList[num].ImgPath != "")
            {
                try
                {
                    StorageFile f = await StorageFile.GetFileFromPathAsync(GapList[num].ImgPath);
                    IRandomAccessStream _s = await f.OpenAsync(FileAccessMode.Read);
                    Img.SetSource(_s);
                }
                catch (Exception e)
                {
                }
            }
        }

        /// <summary>
        /// 获取答案
        /// </summary>
        /// <returns></returns>
        private bool GetAnswer()
        {
            return Answer == GapList[num - 1].Answer;
        }
    }
}