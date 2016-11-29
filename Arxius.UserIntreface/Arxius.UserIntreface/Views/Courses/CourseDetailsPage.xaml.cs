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
    public partial class CourseDetailsPage : ContentPage
    {
        public CourseDetailsPage(INavigation navi,Course course)
        {
            InitializeComponent();
            var vm = new CourseDetailsViewModel(navi, course);
            BindingContext = vm;
            Classes.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };
        }
    

    }
}
