using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Leaf.Model;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Input;

namespace Leaf.ViewModel
{
    internal class LoginModle : ViewModelBase
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

        //登陆命令
        public ICommand LoginCommand { get; set; }

        //登陆
        private void Login()
        {
            //验证用户名和密码是否为空
            User model = new User();
            if (Username == null || Password == null || Username.Trim() == "" || Password.Trim() == "")
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("用户名或密码为空", "LoginNo");
            }
            else
            {
                //验证密码是否正确
                model.Username = Username;
                model.Password = Password;
                if (true == Verification.authenticate(model))
                {
                    Password = "";
                    var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                    navigation.NavigateTo("Main", model);
                    ViewModelLocator.UserInfo.Init();
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "MainFrame", "UserInfo" }, "NavigateTo");
                }
                else
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("用户名或密码错误", "LoginNo");
                }
            }
        }

        //注册命令
        public ICommand ToRegister { get; set; }

        //跳转到注册页面
        private void Register()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Register");
            ViewModelLocator.Register.Username = Username;
        }

        public ICommand ToTest { get; set; }

        private void test()
        {
            //var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            //navigation.NavigateTo("Test");
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(new[] { "MainFrame", "Test" }, "NavigateTo");
        }

        //初始化
        public LoginModle()
        {
            LoginCommand = new RelayCommand(Login);
            ToRegister = new RelayCommand(Register);
            ToTest = new RelayCommand(test);
        }
    }
}