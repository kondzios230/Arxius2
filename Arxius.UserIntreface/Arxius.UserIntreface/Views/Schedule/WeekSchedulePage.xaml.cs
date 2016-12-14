
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
        public WeekSchedulePage(INavigation _navi)
        {
            Title = Properties.Resources.PageNameSchedule;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new WeekScheduleViewModel(_navi,this);
            MessagingCenter.Subscribe<WeekScheduleViewModel>(this, Properties.Resources.MsgEmptySchedule,
             async (sender) =>
             {
                 await this.DisplayAlert("Plan zajęć", "Twój plan jest pusty", "OK");
             });
        }       
        
    }
}

