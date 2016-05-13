using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mvvm1.ViewModel
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

    }
}
