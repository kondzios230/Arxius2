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
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);
            MessagingCenter.Subscribe<LoginViewModel>(this, Properties.Resources.MsgLoginFailed,
                async (sender) =>
                {
                    await this.DisplayAlert("",Properties.Resources.MessageLoginFailed, "OK");
                });
        }
        
    }
}
