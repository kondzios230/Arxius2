//using Arxius.Services.PCL;
//using Arxius.Services.PCL.Entities;
//using Arxius.Services.PCL.Interfaces_and_mocks;
//using Arxius.Services.PCL.Parsers;
//using System.ComponentModel;
//using System.Windows.Input;
//using Xamarin.Forms;

//namespace Arxius.UserIntreface.ViewModels
//{
//    class LoginViewModel : AbstractViewModel
//    {
//        private IUtilsService service;
//        public LoginViewModel(INavigation navi)
//        {
//            Navigation = navi;
//            Login = AuthDoNotSync.Login().Item1;
//            Password = AuthDoNotSync.Login().Item2;
//            ExecuteLogin = new Command(DoLogin);
//            ShowPlan = new Command(async() => await Navigation.PushAsync(new WeekSchedulePage(Navigation, true)));
//            service = new UtilsService();
//        }

//        static string _login;
//        public string Login
//        {
//            set
//            {
//                if (_login != value)
//                {
//                    _login = value;

//                    OnPropertyChanged("Login");
//                }
//            }
//            get
//            {
//                return _login;
//            }
//        }

//        static string _password;
//        public string Password
//        {
//            set
//            {
//                if (_password != value)
//                {
//                    _password = value;

//                    OnPropertyChanged("Password");
//                }
//            }
//            get
//            {
//                return _password;
//            }
//        }

//        public ICommand ExecuteLogin { private set; get; }
//        public ICommand ShowPlan { private set; get; }
//        async void DoLogin()
//        {
//            try
//            {
//                IsAIRunning = true;
//                if (await service.Login(Login, Password))
//                {
//                    IsAIRunning = false;
//                    Application.Current.MainPage = new NavigationPage(new MainPage());
//                }
//                else
//                {
//                    MessagingCenter.Send(this, Properties.Resources.MsgLoginFailed);
//                }
//            }
//            catch (ArxiusException e)
//            {
//                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
//            }
//            IsAIRunning = false;
//        }
//    }
//}




using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using Arxius.Services.PCL.Parsers;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class LoginViewModel : AbstractViewModel
    {
        private IUtilsService service;
        public LoginViewModel(INavigation navi,Page page)
        {
            _page = page;
            uService = new UtilsService();
            Navigation = navi;
            Login = AuthDoNotSync.Login().Item1;
            Password = AuthDoNotSync.Login().Item2;
            ExecuteLogin = new Command(DoLogin);
            ShowPlan = new Command(
                //async () => await Navigation.PushAsync(new WeekSchedulePage(Navigation, true)));
                () =>
                {
                    Android.Webkit.CookieManager _cookieManager = Android.Webkit.CookieManager.Instance;
                    try
                    {
                        //var xa = new WebView();
                        var x = _cookieManager.GetCookie(@"https://zapisy.ii.uni.wroc.pl/");
                        var cookieValue = Regex.Match(x, @"csrftoken=(.*?);");
                        var csrfToken = cookieValue.Groups[1].ToString(); uService.Login(x);
                        Application.Current.MainPage = new NavigationPage(new MainPage());
                    }
                    catch (Exception e)
                    {

                    }

                }
                );
            //service = new UtilsService();
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
            var x = new WebViewPage(@"https://zapisy.ii.uni.wroc.pl/");
            this._page.Appearing += _page_Appearing;
            await Navigation.PushAsync(x);
            //try
            //{
            //    IsAIRunning = true;
            //    if (await service.Login(Login, Password))
            //    {
            //        IsAIRunning = false;
            //        Application.Current.MainPage = new NavigationPage(new MainPage());
            //    }
            //    else
            //    {
            //        MessagingCenter.Send(this, Properties.Resources.MsgLoginFailed);
            //    }
            //}
            //catch (ArxiusException e)
            //{
            //    MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            //}
            //IsAIRunning = false;
        }

        private void _page_Appearing(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}