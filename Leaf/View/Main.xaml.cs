using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Popups;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Views;
using Leaf.ViewModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Leaf.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Main : Page
    {
        public Main()
        {
            this.InitializeComponent();
            this.InitData();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<object>(this, true, LogoffMessage);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<string[]>(this, "NavigateTo", NavigateTo);
        }

        public Dictionary<string, Frame> FrameDictionary;
        public Dictionary<string, Type> PageDictionary;
        public void InitData()
        {
            if (PageDictionary != null) return;
            Debug.WriteLine("进行了初始化,之后需要单独拿出", "information");
            PageDictionary = new Dictionary<string, Type>
            {
                {"login", typeof (Login)},
                {"Main", typeof (Main)},
                {"Register", typeof (register)},
                {"Single", typeof (SinglePapers)},
                {"Gap", typeof (GapPapers)},
                {"Test", typeof (View.test)},
                {"Insert", typeof (InsertData)},
                {"TestPaper", typeof (TestPaperManage)}
            };

            FrameDictionary = new Dictionary<string, Frame>
            {
                 {"MainFrame", MainFrame},
            };
        }

        public void NavigateTo(string[] data)
        {
            var frameName = data[0];
            var pageName = data[1];
            NavigateTo(frameName, pageName);
        }

        public void NavigateTo(string frameName, string pageName)
        {
            FrameDictionary[frameName].Navigate(PageDictionary[pageName]);
        }
        public async void LogoffMessage(object msg)
        {
            MessageDialog mag = new MessageDialog(msg as string);
            UICommand yes = new UICommand("确定", (o) =>
             {
                 var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                 navigation.GoBack();
             });
            UICommand no = new UICommand("返回", (o) =>
             {
             });
            mag.Commands.Add(yes);
            mag.Commands.Add(no);
            var re = await mag.ShowAsync();
            if(re == yes)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister<object>(this, LogoffMessage);
            }
        }

        private void MainListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("yahouooooooooooooooooooooo");
            var listView = (ListView)sender;
            switch (listView.SelectedIndex)
            {
                case 0:
                {
                       NavigateTo("MainFrame","login");
                        break;
                    }
                case 1:
                    {
                        NavigateTo("MainFrame", "Register");
                        break;
                    }
                case 2:
                    {
                        NavigateTo("MainFrame", "Test");
                        break;
                    }
                case 3:
                    {
                        NavigateTo("MainFrame", "login");
                        break;
                    }

                default:
                    break;
            }
        }
    }
}
