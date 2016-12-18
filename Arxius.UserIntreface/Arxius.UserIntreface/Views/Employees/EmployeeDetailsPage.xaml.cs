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
        public EmployeeDetailsPage(INavigation _navi,Employee _employee)
        {
            Title = _employee.Name;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new EmployeeDetailsViewModel(_navi, _employee,this);
            GroupedView.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };
        }
        
    }
}
