using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class NewsFeedPage : ContentPage
    {
        public NewsFeedPage(INavigation _navi)
        {
            InitializeComponent();
            BindingContext = new NewsFeedViewModel(_navi);
            NewsList.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };
        }
        
    }
}
