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
        public CourseDetailsPage(INavigation navi, Course course)
        {
            Title = course.Name;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new CourseDetailsViewModel(navi, course, this);
            Classes.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
            MessagingCenter.Subscribe<CourseDetailsViewModel, Tuple<bool,string>>(this, Properties.Resources.MsgEnrollment,
                async (sender, flag) =>
                {
                    string message = "Operacja zakończona powodzeniem";
                    if (!flag.Item1)
                        message = "Operacja nieudana";
                    await this.DisplayAlert(message, flag.Item2, "OK");

                });
            MessagingCenter.Subscribe<CourseDetailsViewModel, bool>(this, Properties.Resources.MsgSave,
                async (sender, flag) =>
                {
                    var title = "";
                    var message = "";
                    if (flag)
                    {
                        title = "Sukces";
                        message = "Zapis notatek udany";
                    }
                    else
                    {
                        title = "Błąd";
                        message = "Wystąpił błąd, nie udało się zapisać notatek";
                    }
                    await this.DisplayAlert(title, message, "OK");

                });
            MessagingCenter.Subscribe<CourseDetailsViewModel, string>(this, Properties.Resources.MsgNetworkError,
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
