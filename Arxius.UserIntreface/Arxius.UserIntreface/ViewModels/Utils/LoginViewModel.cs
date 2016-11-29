using Arxius.Services.PCL;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        public LoginViewModel(INavigation navi)
        {
            _navigation = navi;
            Login = AuthDoNotSync.Login().Item1;
            Password= AuthDoNotSync.Login().Item2;
            ExecuteLogin = new Command(DoLogin);
        }

        static string  _login;
        public string Login
        {
            set
            {
                if (_login != value)
                {
                    _login = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,new PropertyChangedEventArgs("Login"));
                    }
                }
            }
            get
            {
                return _login;
            }
        }

        static string _password;
        public string Password
        {
            set
            {
                if (_password != value)
                {
                    _password = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                    }
                }
            }
            get
            {
                return _password;
            }
        }

        static string _success;
        public string Success
        {
            set
            {
                if (_success != value)
                {
                    _success = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Success"));
                    }
                }
            }
            get
            {
                return _success;
            }
        }

        public ICommand ExecuteLogin { private set; get; }
        async void DoLogin()
        {
            var us = new UtilsService();
            if (await us.Login(Login, Password))
            {
                await _navigation.PushModalAsync(new NavigationPage(new MainPage()));
            }
        }
    }
}