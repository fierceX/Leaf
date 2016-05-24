using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Leaf.ViewModel;
using Leaf.View;
using GalaSoft.MvvmLight.Views;

namespace Leaf
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<LoginModle>();
            SimpleIoc.Default.Register<MainModel>();
            SimpleIoc.Default.Register<RegisterModel>();
            SimpleIoc.Default.Register<SingleModel>();
            SimpleIoc.Default.Register<GapModel>();
            SimpleIoc.Default.Register<TestModel>();
            SimpleIoc.Default.Register<InsertModel>();
            SimpleIoc.Default.Register<TestPaperModel>();

            var navigationService = this.InitNavigationService();
            SimpleIoc.Default.Register(() => navigationService);

        }
        public INavigationService InitNavigationService()
        {
            NavigationService navigationService = new NavigationService();
            navigationService.Configure("Login", typeof(Login));
            navigationService.Configure("Main", typeof(Main));
            navigationService.Configure("Register", typeof(register));
            navigationService.Configure("Single", typeof(SinglePapers));
            navigationService.Configure("Gap", typeof(GapPapers));
            navigationService.Configure("Test", typeof(View.test));
            navigationService.Configure("Insert", typeof(InsertData));
            navigationService.Configure("TestPaper", typeof(TestPaperManage));
            return navigationService;
        }

        public static void Navigate(string pageName, string frameName)
        {
            if (pageName == null || pageName.Trim() == "" || frameName == null || frameName.Trim() =="")
            {
                Debug.WriteLine("ViewModelLocator.Navigate err : pageName isNull or frameName isNull", "error");
                return;
            }


            Navigate(pageName);
        }

        public static void Navigate(string pageName)
        {
            if (pageName == null || pageName.Trim() == "")
            {
                Debug.WriteLine("ViewModelLocator.Navigate err : pageName isNull", "error");
                return;
            }

            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo(pageName);
        }

        public static void Navigate(string pageName, object data)
        {
            if (pageName == null || pageName.Trim() == "" || data == null)
            {
                Debug.WriteLine("ViewModelLocator.Navigate err : pageName isNull or data isNull", "error");
                return;
            }
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            navigation.NavigateTo(pageName, data);
        }

        private static LoginModle _login;
        public static LoginModle Login
        {
            get
            {
                if(_login == null)
                {
                    _login = ServiceLocator.Current.GetInstance<LoginModle>();
                }
                return _login;
            }
        }
        private static MainModel _main;
        public static MainModel Main
        {
            get
            {
                if (_main == null)
                    _main = ServiceLocator.Current.GetInstance<MainModel>();
                return _main;
            }
        }
        private static RegisterModel _register;
        public static RegisterModel Register
        {
            get
            {
                if (_register == null)
                    _register = ServiceLocator.Current.GetInstance<RegisterModel>();
                return _register;
            }
        }

        private static SingleModel _singlepaper;
        public static SingleModel SinglePaper
        {
            get
            {
                if (_singlepaper == null)
                    _singlepaper = ServiceLocator.Current.GetInstance<SingleModel>();
                return _singlepaper;
            }
        }

        private static GapModel _gappaper;
        public static GapModel GapPaper
        {
            get
            {
                if (_gappaper == null)
                    _gappaper = ServiceLocator.Current.GetInstance<GapModel>();
                return _gappaper;
            }
        }

        private static TestModel _test;
        public static TestModel Test
        {
            get
            {
                if (_test == null)
                    _test = ServiceLocator.Current.GetInstance<TestModel>();
                return _test;
            }
        }

        private static InsertModel _insterdata;
        public static InsertModel InsterData
        {
            get
            {
                if (_insterdata == null)
                    _insterdata = ServiceLocator.Current.GetInstance<InsertModel>();
                return _insterdata;
            }
        }


        private static TestPaperModel _testpaper;
        public static TestPaperModel TestPaper
        {
            get
            {
                if (_testpaper == null)
                    _testpaper = ServiceLocator.Current.GetInstance<TestPaperModel>();
                return _testpaper;
            }
        }

    }
}
