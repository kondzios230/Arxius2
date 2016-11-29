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
    public partial class ScheduleCourseDetailsPage : ContentPage
    {
        public ScheduleCourseDetailsPage(INavigation navi,Course course)
        {
            InitializeComponent();
            BindingContext = new ScheduleCourseDetailsViewModel(navi,course);
            Classes.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };

        }
        
    }
}
