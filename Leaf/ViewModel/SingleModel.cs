using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Leaf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Leaf.ViewModel
{
    class SingleModel : ViewModelBase
    {
        //题目数量
        private int max = 0;
        //当前题目
        private int num = 0;
        //正确答案标记
        private int answernum = 0;
        //题目列表
        private List<SingleChoice> SingleList = new List<SingleChoice>();
        //随机数列
        private int[] array = new int[4];
        //成绩单
        private List<bool> Result = new List<bool>();


        //题干
        private string _stem;
        public string Stem
        {
            get { return _stem;}
            set { Set(ref _stem, value); }
        }


        //选项内容
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


        //选项状态
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

 
        //命令
        //继续
         public ICommand ContinueCommand { get; set; } 
         private void Continue()
         {
            Result.Add(GetAnswer(answernum));
            if (num < max)
            { Init(); }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<List<bool>>(Result, "SingleEnd");
            }
        } 
 
        //初始化
        private void Init()
        {
            array = GetRandom(4);
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

        //构造函数
        public SingleModel()
        { 
            ContinueCommand = new RelayCommand(Continue);
            max = SingleList.Count;
        }

        //生成随机化数列
        private  int[] GetRandom(int total)
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

        //获取答案
        private bool GetAnswer(int num)
        {
            bool[] choicebool = new bool[4];
            choicebool[array[0]] = Choice1;
            choicebool[array[1]] = Choice2;
            choicebool[array[2]] = Choice3;
            choicebool[array[3]] = Choice4;

            return choicebool[num];
        }
    }
}
