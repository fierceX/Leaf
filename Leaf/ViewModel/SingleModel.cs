﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Leaf.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Leaf.SQLite;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Views;

namespace Leaf.ViewModel
{
    class SingleModel : ViewModelBase
    {
        /// <summary>
        /// 模式标志
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
        /// 正确答案标记
        /// </summary>
        private int answernum = 0;

        /// <summary>
        /// 题目列表
        /// </summary>
        public List<SingleChoice> SingleList = new List<SingleChoice>();

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
            get { return _stem;}
            set { Set(ref _stem, value); }
        }

        /// <summary>
        /// 选项内容
        /// </summary>
        private string _choicetext1; 
         public string ChoiceText1
        { 
             get { return _choicetext1; } 
             set { Set(ref _choicetext1, value);} 
        } 
 
 
         private string _choicetext2; 
         public string ChoiceText2
        { 
             get { return _choicetext2; } 
             set { Set(ref _choicetext2, value); } 
        } 
 
 
         private string _choicetext3; 
         public string ChoiceText3
        { 
             get { return _choicetext3; } 
             set { Set(ref _choicetext3, value); } 
        } 
 
 
         private string _choicetext4; 
         public string ChoiceText4
        { 
             get { return _choicetext4; } 
             set { Set(ref _choicetext4, value); } 
        }

        /// <summary>
        /// 选项状态
        /// </summary>
        private bool _choice1;
        public bool Choice1
        {
            get { return _choice1; }
            set { Set(ref _choice1, value); }
        }
        private bool _choice2;
        public bool Choice2
        {
            get { return _choice2; }
            set { Set(ref _choice2, value); }
        }
        private bool _choice3;
        public bool Choice3
        {
            get { return _choice3; }
            set { Set(ref _choice3, value); }
        }
        private bool _choice4;
        public bool Choice4
        {
            get { return _choice4; }
            set { Set(ref _choice4, value); }
        }

        /// <summary>
        /// 命令
        /// /////////
        /// 继续
        /// </summary>
        public ICommand ContinueCommand { get; set; } 
         private void Continue()
         {
             if (Result.Count <= max)
             {
                Result.Add(GetAnswer(answernum));
            }
            if (num < max)
             {
                Init();
                Choice1 = false;
                Choice2 = false;
                Choice3 = false;
                Choice4 = false;
             }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<List<bool>>(Result, "SingleEnd");
                //if(Mode == 1)
                //{
                    ViewModelLocator.TestResult.SingleResult = Result;
                Result.Clear();
                num = 0;
                answernum = 0;
                max = 0;
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                    navigation.NavigateTo("Gap");
               // }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            max = SingleList.Count;
            if (num >= SingleList.Count)
            {
                return;
            }
            int[] array = GetRandom(4);
            string[] choicearray = new string[4];
            choicearray[array[0]] = SingleList[num].Answer;
            choicearray[array[1]] = SingleList[num].Choices1;
            choicearray[array[2]] = SingleList[num].Choices2;
            choicearray[array[3]] = SingleList[num].Choices3;
            ChoiceText1 = choicearray[0];
            ChoiceText2 = choicearray[1];
            ChoiceText3 = choicearray[2];
            ChoiceText4 = choicearray[3];
            Stem = SingleList[num].Stems;
            answernum = array[0];
            num++;

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleModel()
        {
            Init();
            ContinueCommand = new RelayCommand(Continue);
            
        }

        /// <summary>
        /// 生成随机化数列
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        private int[] GetRandom(int total)
        {
            int[] array = new int[total];
            for (int i = 0; i < total; i++)
            { array[i] = i; }
            Random random = new Random();
            int temp2;
            int end = total;
            for (int i = 0; i < total; i++)
            {
                int temp = random.Next(end);
                temp2 = array[temp];
                array[temp] = array[end - 1];
                array[end - 1] = temp2;
                end--;
            }
            return array;
        }

        /// <summary>
        /// 获取答案
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private bool GetAnswer(int num)
        {
            bool[] choicebool = new bool[4];
            choicebool[0] = Choice1;
            choicebool[1] = Choice2;
            choicebool[2] = Choice3;
            choicebool[3] = Choice4;
            return choicebool[num];
        }
    }
}
