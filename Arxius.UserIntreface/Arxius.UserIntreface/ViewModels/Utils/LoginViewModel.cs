using Arxius.Services.PCL;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class LoginViewModel : AbstractViewModel
    {
        string adress = @"https://zapisy.ii.uni.wroc.pl/";
        public LoginViewModel(INavigation navi, Page page)
        {
            _page = page;
            uService = new UtilsService();
            Navigation = navi;
            CanClick = true;
            ExecuteLogin = new Command(DoLogin,()=>CanClick);
            ShowPlan = new Command(async () => await Navigation.PushAsync(new WeekSchedulePage(Navigation, true)), () => CanClick);
        }
        private bool canClick;

        public bool CanClick
        {
            get { return canClick; }
            set { canClick = value;
                if (ExecuteLogin != null)
                    ((Command)ExecuteLogin).ChangeCanExecute();
                if (ShowPlan != null)
                    ((Command)ShowPlan).ChangeCanExecute();
            }
        }

        public ICommand ExecuteLogin { private set; get; }
        public ICommand ShowPlan { private set; get; }
        async void DoLogin()
        {
            MessagingCenter.Send(this, Properties.Resources.MsgHowToLogin);
            _page.Appearing += _page_Appearing;
            await Navigation.PushAsync(new WebViewPage(adress));
        }

        private async void _page_Appearing(object sender, EventArgs e)
        {
            IsAIRunning = true;
            CanClick = false;
            _page.Appearing -= _page_Appearing;
            var cookieManager = Android.Webkit.CookieManager.Instance;
            uService.Login(cookieManager.GetCookie(adress));
            if (await uService.IsLoggedIn())
                Application.Current.MainPage = new NavigationPage(new MainPage());
            else
                MessagingCenter.Send(this, Properties.Resources.MsgLoginFailed);
            IsAIRunning = false;
            CanClick = true;
        }
    }
}