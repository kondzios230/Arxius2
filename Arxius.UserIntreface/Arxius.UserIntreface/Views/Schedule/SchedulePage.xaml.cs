using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class SchedulePage : ContentPage
    {
        public SchedulePage(INavigation _navi)
        {
            InitializeComponent();
            BindingContext = new ScheduleViewModel(_navi);
            Courses.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };
           
        }
        
    }
}
