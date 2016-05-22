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
        public ICommand ToSingle { get; set; }

        private void Single()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Single");
        }

        public ICommand ToGap { get; set; }

        private void Gap()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Gap");
        }

        public ICommand ToJson { get; set; }

        private void json()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo("Insert");
        }

        public TestModel()
        {
            ToGap = new RelayCommand(Gap);
            ToJson = new RelayCommand(json);
            ToSingle = new RelayCommand(Single);
        }
    }
}
