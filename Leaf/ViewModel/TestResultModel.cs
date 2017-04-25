using GalaSoft.MvvmLight;
using Leaf.Model;
using System;
using System.Collections.Generic;

namespace Leaf.ViewModel
{
    internal class TestResultModel : ViewModelBase
    {
        private string _message;

        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
        }

        /// <summary>
        /// 选择题成绩值
        /// </summary>
        private double _singleValue;

        public double SingleValue
        {
            get { return _singleValue; }
            set { Set(ref _singleValue, value); }
        }

        /// <summary>
        /// 填空题成绩值
        /// </summary>
        private double _gapValue;

        public double GapValue
        {
            get { return _gapValue; }
            set { Set(ref _gapValue, value); }
        }

        /// <summary>
        /// 总成绩值
        /// </summary>
        private double _allValue;

        public double AllValue
        {
            get { return _allValue; }
            set { Set(ref _allValue, value); }
        }

        /// <summary>
        /// 选择题正确数
        /// </summary>
        private string _singleright;

        public string SingleRight
        {
            get { return _singleright; }
            set { Set(ref _singleright, value); }
        }

        /// <summary>
        /// 选择题错误数
        /// </summary>
        private string _singlewrong;

        public string SingleWrong
        {
            get { return _singlewrong; }
            set { Set(ref _singlewrong, value); }
        }

        /// <summary>
        /// 填空题正确数
        /// </summary>
        private string _gapright;

        public string GapRight
        {
            get { return _gapright; }
            set { Set(ref _gapright, value); }
        }

        /// <summary>
        /// 填空题错误数
        /// </summary>
        private string _gapwrong;

        public string GapWrong
        {
            get { return _gapwrong; }
            set { Set(ref _gapwrong, value); }
        }

        /// <summary>
        /// 总正确数
        /// </summary>
        private string _allright;

        public string AllRight
        {
            get { return _allright; }
            set { Set(ref _allright, value); }
        }

        /// <summary>
        /// 总错误数
        /// </summary>
        private string _allwrong;

        public string AllWrong
        {
            get { return _allwrong; }
            set { Set(ref _allwrong, value); }
        }

        /// <summary>
        /// 试卷
        /// </summary>
        public TestPaper TestPaperModel = new TestPaper();

        //单选题答案
        public List<bool> SingleResult = new List<bool>();

        //填空题答案
        public List<bool> GapResult = new List<bool>();

        //正确习题数量
        private int singleright;

        private int gapright;

        //初始化，也就是成绩结算
        public void Init()
        {
            singleright = 0;
            gapright = 0;
            //统计答对多少题
            foreach (var result in SingleResult)
            {
                if (result)
                    singleright++;
            }
            foreach (var result in GapResult)
            {
                if (result)
                    gapright++;
            }
            //统计平均分
            SingleValue = singleright * 100 / TestPaperModel.SingleNum;
            GapValue = gapright * 100 / TestPaperModel.GapNum;
            AllValue = (SingleValue + GapValue) / 2;
            SingleRight = "正确：" + singleright.ToString();
            SingleWrong = "错误：" + (TestPaperModel.SingleNum - singleright).ToString();
            GapWrong = "错误：" + (TestPaperModel.GapNum - gapright).ToString();
            GapRight = "正确：" + gapright.ToString();
            AllRight = "正确：" + (singleright + gapright).ToString();
            AllWrong = "错误：" + (TestPaperModel.GapNum + TestPaperModel.SingleNum - singleright - gapright).ToString();
            //写入数据库
            WriteScore();
            //显示结果消息
            if (AllValue >= 90.0)
            {
                Message = "非常棒！！！";
                return;
            }
            else if (AllValue >= 75.0)
            {
                Message = "成绩不错，继续加油！！！";
                return;
            }
            else if (AllValue >= 60.0)
            {
                Message = "革命尚未成功，同志仍需努力！！！";
                return;
            }
            else
            {
                Message = "还没及格，今天好好学习！！！";
                return;
            }
        }

        public void Clear()
        {
            GapResult.Clear();
            SingleResult.Clear();
            TestPaperModel = null;
        }

        /// <summary>
        /// 写入数据库
        /// </summary>
        private void WriteScore()
        {
            //建立用户和试卷级联表并写入数据库
            UserTest usertest = new UserTest();
            usertest.UserId = ViewModelLocator.User.Id;
            usertest.TestId = TestPaperModel.Id;
            usertest.singnum = singleright;
            usertest.gapnum = gapright;
            usertest.Time = DateTime.Now.ToString();
            usertest.Score = AllValue;
            using (var mydb = new MyDBContext())
            {
                mydb.UserTest.Add(usertest);
                mydb.SaveChanges();
            }
        }
    }
}