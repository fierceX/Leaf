using GalaSoft.MvvmLight;
using System.Windows.Input;
using Leaf.Model;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Windows.Security.Cryptography.Core;
using System.Text;
using SQLite;
using Leaf.SQLite;

namespace Leaf.ViewModel
{
    class RegisterModel: ViewModelBase
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

        public ICommand RegisterCommand { get; set; }
        private void Register()
        {
            if (Username == null || Password == null || Username.Trim() == "" || Password.Trim() == "")
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("请填写用户名或密码", "RegisterNo");
                return;
            }

            var model = new User();
            var md5 = new Md5();
            model.Username = Username;
            model.Password = md5.ToMd5(Password);
            var db = new DbUserService();
            if (db.QueryNum() == 0)
                model.Admin = 1;
            var i=db.Insert(model);
            if (i>0)
            {
                Username = "";
                Password = "";
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                navigation.NavigateTo("Login");
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("注册成功","RegisterYes");
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("注册失败","RegisterNo");
            }
        }

        public ICommand ToLogin { get; set; }
        private void Login()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Login");
            ViewModelLocator.Login.Username = Username;
        }

        public RegisterModel()
        {
            RegisterCommand = new RelayCommand(Register);
            ToLogin = new RelayCommand(Login);
        }
    }
}
