using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaf.Model;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Leaf.SQLite;
namespace Leaf.ViewModel
{
    class TestResultModel : ViewModelBase
    {

        private List<Score> score;
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
        public TestPaper TestPaperModel = new TestPaper();
        public List<bool> SingleResult = new List<bool>();
        public List<bool> GapResult = new List<bool>();

        public void Init()
        {
            int singright=0;
            int gapright = 0;
            foreach(var result in SingleResult)
            {
                if (result)
                    singright++;
            }
            foreach(var result in GapResult)
            {
                if (result)
                    gapright++;
            }
            SingleValue = singright*100/ TestPaperModel.SingleNum;
            GapValue = gapright*100 / TestPaperModel.GapNum;
            AllValue = (SingleValue + GapValue) / 2;
            SingleRight = "正确：" + singright.ToString();
            SingleWrong = "错误：" + (TestPaperModel.SingleNum - singright).ToString();
            GapWrong = "错误：" + (TestPaperModel.GapNum - gapright).ToString();
            GapRight = "正确：" + gapright.ToString();
            AllRight = "正确：" + (singright + gapright).ToString();
            AllWrong = "错误：" + (TestPaperModel.GapNum + TestPaperModel.SingleNum - singright - gapright).ToString();

            if(AllValue>=90.0)
            {
                Message = "非常棒！！！";
            }
            else if(AllValue>=75.0)
            {
                Message = "成绩不错，继续加油！！！";
            }else if(AllValue>=60.0)
            {
                Message = "革命尚未成功，同志仍需努力！！！";
            }
            else
            {
                Message = "还没及格，今天好好学习！！！";
            }
            WriteScore();

        }

        public void Clear()
        {
            //SingleValue = 0;
            //GapValue = 0;
            GapResult.Clear();
            SingleResult.Clear();
            TestPaperModel = null;
        }


        private void WriteScore()
        {
            Score _score = new Score();
            _score.Paper = TestPaperModel.Id;
            _score.score = AllValue;
            _score.time = DateTime.Now.ToString();
            if (ViewModelLocator.User.Score == null || ViewModelLocator.User.Score == "")
            {
                score = new List<Score>();  
            }
            else
            {
                score = JsonConvert.DeserializeObject<List<Score>>(ViewModelLocator.User.Score);
            }
            score.Add(_score);
            string json = JsonConvert.SerializeObject(score);
            ViewModelLocator.User.Score = json;
            var db = new DbUserService();
            if(db.Update(ViewModelLocator.User)>0)
            {
                var username = ViewModelLocator.User.Username;
                ViewModelLocator.User = (User)db.QueryObject(username);
                ViewModelLocator.UserInfo.Init();
            }
        }


        struct Score
        {
            public int Paper { get; set; }
            public double score { get; set; }
            public string time { get; set; }
        }

    }
}
