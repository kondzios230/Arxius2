using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class DatesPage : ContentPage
    {
        public DatesPage(INavigation _navi)
        {
            Title = Properties.Resources.PageNameDates;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new DatesViewModel(_navi, this);
            GroupedView.ItemSelected += (sender, e) =>
            {
                var item = (((ListView)sender).SelectedItem as string);
                if (item != null)
                {
                    var splitIndex = item.IndexOf(':');
                    try
                    {
                        DisplayAlert(item.Substring(0,splitIndex), item.Substring(splitIndex+2), "OK");
                    }
                    catch { }

                }
                 ((ListView)sender).SelectedItem = null;
            };
        }

    }
}
