using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Leaf.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class QuestionStore : Page
    {
        public QuestionStore()
        {
            this.InitializeComponent();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<int[]>(this, "DownloadYes", MessageBox);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<string>(this, "StoreError", ErrorMessage);
        }
        private async void MessageBox(int[] msg)
        {
            await new MessageDialog("成功下载\n" + msg[0].ToString() + " 道选择题\n" + msg[1].ToString() + " 道填空题").ShowAsync();
        }

        private async void ErrorMessage(string msg)
        {
            await new MessageDialog(msg).ShowAsync();
        }
    }
}
