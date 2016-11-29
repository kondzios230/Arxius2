using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class UserProfilePage : ContentPage
    {
        public UserProfilePage(INavigation _navi)
        {
            InitializeComponent();
            BindingContext = new UserProfileViewModel(_navi);
            GroupedView.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };
        }
        
    }
}
