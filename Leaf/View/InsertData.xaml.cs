using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Leaf.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class InsertData : Page
    {
        public InsertData()
        {
            this.InitializeComponent();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<string[]>(this, "InsertYes", MessageBox);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<string>(this, "Exception", ExceptionMessageBox);
        }

        private async void MessageBox(string[] msg)
        {
            await new MessageDialog("成功插入\n" + msg[0] + " 道选择题\n" + msg[1] + " 道判断题\n" + msg[2] + " 图片\n" + "图片占用大小 " + msg[3]).ShowAsync();
        }

        private async void ExceptionMessageBox(string msg)
        {
            await new MessageDialog("引发异常：" + msg).ShowAsync();
        }
    }
}