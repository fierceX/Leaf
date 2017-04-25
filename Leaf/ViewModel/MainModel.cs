using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Leaf.ViewModel
{
    internal class MainModel : ViewModelBase
    {
        private string _hamburgTitle = "汉堡菜单";

        //汉堡菜单标题
        public string HamburgTitle
        {
            get { return _hamburgTitle; }
            set { Set(ref _hamburgTitle, value); }
        }

        //汉堡菜单状态
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

        //汉堡菜单内容
        private ObservableCollection<NavLink> _menuItems = new ObservableCollection<NavLink>()
            {
                new NavLink() { Icon = Symbol.Library, Text="题库列表"},
                new NavLink() { Icon = Symbol.AllApps,Text="试卷列表" },
                new NavLink() { Icon = Symbol.ImportAll, Text="导入题库"},
                new NavLink() { Icon = Symbol.ContactInfo,Text="个人信息"},
                new NavLink() { Icon = Symbol.Help,Text="帮助" },
                new NavLink() { Icon = Symbol.BlockContact,Text="注销" }
            };

        public ObservableCollection<NavLink> MenuItems
        {
            get { return _menuItems; }
            set { Set(ref _menuItems, value); }
        }

        //显示用户名
        public string Username
        {
            get { return ViewModelLocator.User.Username; }
        }

        //注销登陆
        public ICommand LogoffCommand { get; set; }

        private void Logoff()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<object>("确认注销吗");
        }

        //初始化
        public MainModel()
        {
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