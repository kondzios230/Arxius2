using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class CourseListPage : ContentPage
    {
        public CourseListPage(INavigation _navi)
        {
            Title = Properties.Resources.PageNameCoursesList;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new CourseListViewModel(_navi,this);
            Courses.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };
            MessagingCenter.Subscribe<CourseListViewModel, string>(this, Properties.Resources.MsgNetworkError,
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
