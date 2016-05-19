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
    public sealed partial class SinglePapers : Page
    {
        public SinglePapers()
        {
            this.InitializeComponent();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<List<bool>>(this, "SingleEnd", MessageBox);
        }

        private async void MessageBox(List<bool> result)
        {
            var newStr = string.Join("||", result.ToArray());
            await new MessageDialog(newStr).ShowAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister<List<bool>>(this, "SingleEnd", MessageBox);
        }

    }
}
