using GalaSoft.MvvmLight;
using System.Windows.Input;
using mvvm1.Model;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Windows.Security.Cryptography.Core;
using System.Text;
using SQLite;

namespace mvvm1.ViewModel
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
            User model = new User();
            MD5 md5 = new MD5();
            model.Username = Username;
            model.Password = md5.MMD5(Password);
            DbService db = new DbService();
            int i=db.InserUser(model);
            if (i>0)
            {
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                navigation.NavigateTo("Login");
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("注册成功","RegisterYes");
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("注册失败","RegisterNo");
            }
        }

        public RegisterModel()
        {
            RegisterCommand = new RelayCommand(Register);
        }
    }
}
