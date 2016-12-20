
using Arxius.Services.PCL.Entities;
using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class WeekSchedulePage : ContentPage
    {
        public Grid grid;
        public ScrollView scroll;
        public WeekSchedulePage(INavigation _navi,bool isOffline=false)
        {
            Title = isOffline ? Properties.Resources.PageNameOfflineSchedule:Properties.Resources.PageNameSchedule;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new WeekScheduleViewModel(_navi,this,isOffline);
            MessagingCenter.Subscribe<WeekScheduleViewModel>(this, Properties.Resources.MsgEmptySchedule,
             async (sender) =>
             {
                 await this.DisplayAlert("Plan zajęć", "Twój plan jest pusty", "OK");
             });
            MessagingCenter.Subscribe<WeekScheduleViewModel, string>(this, Properties.Resources.MsgNetworkError,
           async (sender, message) =>
           {
               await this.DisplayAlert("Problem z siecią", message, "OK");
           });
            MessagingCenter.Subscribe<WeekScheduleViewModel, string>(this, Properties.Resources.MsgFileError,
           async (sender, message) =>
           {
               await this.DisplayAlert("Problem z plikiem", message, "OK");
           });
        }       
        
    }
}

