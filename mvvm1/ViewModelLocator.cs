﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using mvvm1.ViewModel;
using mvvm1.View;
using GalaSoft.MvvmLight.Views;

namespace mvvm1
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<LoginModle>();
            SimpleIoc.Default.Register<MainModel>();
            SimpleIoc.Default.Register<RegisterModel>();

            var navigationService = this.InitNavigationService();
            SimpleIoc.Default.Register(() => navigationService);

        }
        public INavigationService InitNavigationService()
        {
            NavigationService navigationService = new NavigationService();
            navigationService.Configure("Login", typeof(Login));
            navigationService.Configure("Main", typeof(Main));
            navigationService.Configure("Register", typeof(register));
            return navigationService;
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

    }
}
