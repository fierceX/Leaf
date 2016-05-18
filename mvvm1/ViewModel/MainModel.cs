using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mvvm1.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace mvvm1.ViewModel
{
    class MainModel:ViewModelBase
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }
        private User _user;
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                Username = value.Username;
            }
        }
        public ICommand LogoffCommand { get; set; }
        private void Logoff()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<object>("确认注销吗");
        }
        public MainModel()
        {
            LogoffCommand = new RelayCommand(Logoff);
        }
    }
}
