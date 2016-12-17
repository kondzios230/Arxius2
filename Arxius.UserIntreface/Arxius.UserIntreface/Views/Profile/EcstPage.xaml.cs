using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class EctsPage : ContentPage
    {
        public EctsPage(INavigation _navi)
        {
            Title = Properties.Resources.PageNameEcts;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new EctsViewModel(_navi,this);
            MessagingCenter.Subscribe<EctsViewModel, string>(this, Properties.Resources.MsgNetworkError,
        async (sender, message) =>
        {
            await this.DisplayAlert("Problem z siecią", message, "OK");
        });

        }
        public void SetRefreshImage(string imagePath)
        {
            RefreshButton.Source = imagePath;
        }


    }
}
