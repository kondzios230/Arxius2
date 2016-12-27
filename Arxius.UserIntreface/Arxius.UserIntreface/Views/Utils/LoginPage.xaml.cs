using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            Title = Properties.Resources.PageNameLogin;
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation,this);
            NavigationPage.SetHasNavigationBar(this, false);
            MessagingCenter.Subscribe<LoginViewModel>(this, Properties.Resources.MsgLoginFailed,
                async (sender) =>
                {
                    await this.DisplayAlert("",Properties.Resources.MessageLoginFailed, "OK");
                });
            MessagingCenter.Subscribe<LoginViewModel,string>(this, Properties.Resources.MsgNetworkError,
              async (sender,message) =>
              {
                  await this.DisplayAlert("Problem z siecią",message, "OK");
              });
        }
        
    }
}
