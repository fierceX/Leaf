using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Leaf.Model;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Linq;
using System.Windows.Input;

namespace Leaf.ViewModel
{
    internal class RegisterModel : ViewModelBase
    {
        //用户名和密码
        private string _username;

        private string _password;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                Set(ref _username, value);
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                Set(ref _password, value);
            }
        }

        //命令，注册
        public ICommand RegisterCommand { get; set; }

        private void Register()
        {
            //判断用户名和密码是否为空
            if (Username == null || Password == null || Username.Trim() == "" || Password.Trim() == "")
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("请填写用户名或密码", "RegisterNo");
                return;
            }

            //加密用户密码
            var model = new User();
            var md5 = new Md5();
            model.Username = Username;
            model.Password = md5.ToMd5(Password);
            model.BuildTime = DateTime.Now.ToString();

            int i = 0;
            //添加用户
            using (var mydb = new MyDBContext())
            {
                if (mydb.Users.Count() <= 0)
                    model.Admin = 1;
                mydb.Users.Add(model);
                i = mydb.SaveChanges();
            }
            //成功则跳转到登陆页面
            if (i > 0)
            {
                Username = "";
                Password = "";
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                navigation.NavigateTo("Login");
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("注册成功", "RegisterYes");
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("注册失败", "RegisterNo");
            }
        }

        //命令，返回到登陆页面
        public ICommand ToLogin { get; set; }

        private void Login()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Login");
            ViewModelLocator.Login.Username = Username;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public RegisterModel()
        {
            RegisterCommand = new RelayCommand(Register);
            ToLogin = new RelayCommand(Login);
        }
    }
}