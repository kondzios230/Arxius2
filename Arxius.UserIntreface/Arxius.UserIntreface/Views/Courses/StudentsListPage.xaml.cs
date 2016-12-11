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
    public partial class StudentsListPage : ContentPage
    {
        public StudentsListPage(INavigation navi,_Class course)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            var vm = new StudentsListViewModel(navi, course);
            BindingContext = vm;
            Students.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };
        }
    

    }
}
