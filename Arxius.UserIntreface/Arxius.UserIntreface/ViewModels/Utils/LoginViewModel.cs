using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using Arxius.Services.PCL.Parsers;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class LoginViewModel : AbstractViewModel
    {
        private IUtilsService service;
        public LoginViewModel(INavigation navi)
        {
            Navigation = navi;
            Login = AuthDoNotSync.Login().Item1;
            Password = AuthDoNotSync.Login().Item2;
            ExecuteLogin = new Command(DoLogin);
            ShowPlan = new Command(async() => await Navigation.PushAsync(new WeekSchedulePage(Navigation, true)));
            service = new UtilsService();
        }

        static string _login;
        public string Login
        {
            set
            {
                if (_login != value)
                {
                    _login = value;

                    OnPropertyChanged("Login");
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

                    OnPropertyChanged("Password");
                }
            }
            get
            {
                return _password;
            }
        }

        public ICommand ExecuteLogin { private set; get; }
        public ICommand ShowPlan { private set; get; }
        async void DoLogin()
        {
            try
            {
                IsAIRunning = true;
                if (await service.Login(Login, Password))
                {
                    IsAIRunning = false;
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    MessagingCenter.Send(this, Properties.Resources.MsgLoginFailed);
                }
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            IsAIRunning = false;
        }
    }
}