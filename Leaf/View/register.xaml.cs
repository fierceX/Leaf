using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Leaf.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class register : Page
    {
        public register()
        {
            this.InitializeComponent();
           
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<string>(this, "RegisterNo", MessageBox);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<string>(this, "RegisterYes", ToLogin);
        }

        private async void MessageBox(string msg)
        {
            await new MessageDialog(msg).ShowAsync();
        }
        private async void ToLogin(string msg)
        {
            var ret = await new MessageDialog(msg).ShowAsync();
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Login");
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister<string>(this, "RegisterYes", ToLogin);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister<string>(this, "RegisterNo", MessageBox);
        }
    }
}
