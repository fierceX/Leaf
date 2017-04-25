using GalaSoft.MvvmLight;
using Leaf.Model;
using System.Collections.Generic;
using System.Linq;

namespace Leaf.ViewModel
{
    internal class UserInfoModel : ViewModelBase
    {
        /// <summary>
        /// 折线图字符串
        /// </summary>
        private string _points;

        public string Points
        {
            get { return _points; }
            set { Set(ref _points, value); }
        }

        //用户名
        private string _username;

        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        //是否是管理员
        private string _admin;

        public string Admain
        {
            get { return _admin; }
            set { Set(ref _admin, value); }
        }

        //注册时间
        private string _buildtime;

        public string BuildTime
        {
            get { return _buildtime; }
            set { Set(ref _buildtime, value); }
        }

        //初始化
        public void Init()
        {
            DrawPoint();
            ReadData();
        }

        //构造函数
        public UserInfoModel()
        {
            Init();
        }

        //读取用户信息
        private void ReadData()
        {
            Username = "用户名：" + ViewModelLocator.User.Username;
            if (ViewModelLocator.User.Admin == 1)
                Admain = "管理员账户";
            else
                Admain = "普通账户";
            BuildTime = "注册时间：" + ViewModelLocator.User.BuildTime;
        }

        //画成绩折线图
        private void DrawPoint()
        {
            //获取成绩列表
            List<double> scorelist = new List<double>();
            using (var mydb = new MyDBContext())
            {
                var q = from c in mydb.UserTest
                        where c.UserId == ViewModelLocator.User.Id
                        select c.Score;
                scorelist = q.ToList();
            }
            //如果有成绩，开始画
            if (scorelist.Count <= 0)
                return;
            string point = "";
            for (int i = 0; i < scorelist.Count(); i++)
            {
                point += (i * 20).ToString() + "," + (100 - scorelist[i]).ToString() + ",";
            }
            point = point.Substring(0, point.Length - 1);
            Points = point;
        }
    }
}