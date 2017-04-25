using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
            // 注册左上角回退按钮
            MainFrame.Navigated += OnNavigated;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = getVisibilityStatus(MainFrame);
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequseted;

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
                {"Login", typeof (Login)},
                {"Main", typeof (Main)},
                {"Register", typeof (register)},
                {"Single", typeof (SinglePapers)},
                {"Gap", typeof (GapPapers)},
                {"Insert", typeof (InsertData)},
                {"TestPaper", typeof (TestPaperManage)},
                {"Question", typeof (QuestionList)},
                {"Result", typeof (TestResult)},
                {"UserInfo", typeof (UserInfo) },
                {"Help", typeof (Help) }
            };

            FrameDictionary = new Dictionary<string, Frame>
            {
                 {"MainFrame", MainFrame},
                 {"RootFrame", Window.Current.Content as Frame}
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
                 ViewModelLocator.User.Clear();
                 NavigateTo("RootFrame", "Login");
             });
            UICommand no = new UICommand("返回", (o) =>
             {
             });
            mag.Commands.Add(yes);
            mag.Commands.Add(no);
            var re = await mag.ShowAsync();
            if (re == yes)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister<object>(this, LogoffMessage);
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = getVisibilityStatus((Frame)sender);
        }

        private AppViewBackButtonVisibility getVisibilityStatus(Frame sender)
        {
            return sender.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed; ;
        }

        private void OnBackRequseted(object sender, BackRequestedEventArgs e)
        {
            //            var rootFrame = Window.Current.Content as Frame;
            if (MainFrame != null && MainFrame.CanGoBack)
            {
                e.Handled = true;
                MainFrame.GoBack();
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
                        NavigateTo("MainFrame", "Question");
                        break;
                    }
                case 1:
                    {
                        NavigateTo("MainFrame", "TestPaper");
                        break;
                    }
                case 2:
                    {
                        NavigateTo("MainFrame", "Insert");
                        break;
                    }
                case 3:
                    {
                        NavigateTo("MainFrame", "UserInfo");
                        break;
                    }
                case 4:
                    {
                        NavigateTo("MainFrame", "Help");
                        break;
                    }
                case 5:
                    {
                        LogoffMessage("确定要注销吗？");
                        break;
                    }
                default:
                    break;
            }
        }
    }
}