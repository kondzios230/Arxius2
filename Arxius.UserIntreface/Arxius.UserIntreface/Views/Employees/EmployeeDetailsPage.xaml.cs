using Arxius.Services.PCL.Entities;
using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class EmployeeDetailsPage : ContentPage
    {
        public EmployeeDetailsPage(INavigation _navi,Employee _news)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new EmployeeDetailsViewModel(_navi,_news);
          
        }
        
    }
}
