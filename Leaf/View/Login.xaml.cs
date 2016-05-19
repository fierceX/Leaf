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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Leaf.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<string>(this, "LoginNo", MessageBox);
        }

        private async void MessageBox(string msg)
        {
            await new MessageDialog(msg).ShowAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister<string>(this, "LoginNo", MessageBox);
        }
    }
}
