using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Leaf.Model;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;

namespace Leaf.ViewModel
{
    class LoginModle:ViewModelBase
    {
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
        public ICommand LoginCommand { get; set; }
        private void Login()
        {
            User model = new User();
            if(Username == null || Password == null || Username.Trim() == "" || Password.Trim() == "")
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("用户名或密码为空", "LoginNo");
            }
            else
            {
                model.Username = Username;
                model.Password = Password;
                if (true == server.authenticate(model))
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


        public ICommand ToRegister { get; set; }
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


        public LoginModle()
        {
            LoginCommand = new RelayCommand(Login);
            ToRegister = new RelayCommand(Register);
            ToTest = new RelayCommand(test);
        }
    }
}
