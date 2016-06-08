using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaf.Model;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Command;

namespace Leaf.ViewModel
{
    class MainModel:ViewModelBase
    {
        private string _hamburgTitle = "汉堡菜单";

        public string HamburgTitle
        {
            get { return _hamburgTitle; }
            set { Set(ref _hamburgTitle, value); }
        }

        private Boolean _isPaneOpen = false;

        public Boolean IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private void HamburgButton()
        {
            IsPaneOpen = !IsPaneOpen;
        }
        public ICommand HamburgCommand { get; set; }


        private ObservableCollection<NavLink> _menuItems = new ObservableCollection<NavLink>()
            {
                new NavLink() { Icon = Symbol.Library, Text="题库列表"},
                new NavLink() { Icon = Symbol.AllApps,Text="试卷列表" },
                new NavLink() { Icon = Symbol.ImportAll, Text="本地插入"},
                new NavLink() { Icon = Symbol.Globe,Text="在线下载"},
                new NavLink() { Icon = Symbol.ContactInfo,Text="个人信息"},
                new NavLink() { Icon = Symbol.Help,Text="帮助" },
                new NavLink() { Icon = Symbol.BlockContact,Text="注销" }
            };

        public ObservableCollection<NavLink> MenuItems
        {
            get { return _menuItems; }
            set { Set(ref _menuItems, value); }
        }




        private string _username;
        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        //private User _user;
        //public User User
        //{
        //    get
        //    {
        //        return _user;
        //    }
        //    set
        //    {
        //        _user = value;
        //        Username = value.Username;
        //    }
        //}

        public ICommand LogoffCommand { get; set; }
        private void Logoff()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<object>("确认注销吗");
        }
        public MainModel()
        {
            Username = ViewModelLocator.User.Username;
            LogoffCommand = new RelayCommand(Logoff);
            HamburgCommand = new RelayCommand(HamburgButton);
        }
    }

    internal class NavLink : ViewModelBase
    {
        private String _text;
        public String Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }
        private Symbol _Icon;
        public Symbol Icon
        {
            get { return _Icon; }
            set { Set(ref _Icon, value); }
        }
    }
}
