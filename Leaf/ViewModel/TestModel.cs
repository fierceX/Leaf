using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Leaf.ViewModel
{
    class TestModel : ViewModelBase
    {

        public ICommand ToJson { get; set; }

        private void json()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Insert");
        }

        public ICommand ToTest { get; set; }

        private void test()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("TestPaper");
        }

        public ICommand ToQuestion { get; set; }
        private void question()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Question");
        }

        public TestModel()
        {
            ToJson = new RelayCommand(json);
            ToTest = new RelayCommand(test);
            ToQuestion = new RelayCommand(question);
        }
    }
}
