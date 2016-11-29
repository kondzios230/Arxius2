using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class ScheduleViewModel : INotifyPropertyChanged
    {
      
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        public ScheduleViewModel(INavigation navi)
        {
            _navigation = navi;
            GetUserScheduleAsync();
            ShowCourse = new Command(ExecuteShowPlan);
        }
        private async void GetUserScheduleAsync()
        {
            var s = new CoursesService();
            Schedule = await s.GetUserPlanForCurrentSemester();
        }
        private List<Course> _schedule;
        public List<Course> Schedule
        {
            set
            {
                if (_schedule != value)
                {
                    _schedule = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Schedule"));
                    }
                }
            }
            get
            {
                return _schedule;
            }
        }
        private Course _selectedItem;
        public Course SelectedCourse
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;

                if (_selectedItem == null)
                    return;

                ShowCourse.Execute(_selectedItem);

                SelectedCourse = null;
            }
        }
        public ICommand ShowCourse { private set; get; }
        async void ExecuteShowPlan()
        {
            await _navigation.PushAsync(new ScheduleCourseDetailsPage(_navigation, SelectedCourse));
        }
    }
}