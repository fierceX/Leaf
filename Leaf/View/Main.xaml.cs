using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<object>(this, true, LogoffMessage);

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

        private void MainListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
