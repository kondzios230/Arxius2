using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class CourseListViewModel : INotifyPropertyChanged
    {
      
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        public CourseListViewModel(INavigation navi)
        {
            _navigation = navi;
            GetCoursesAsync();
            ShowCourse = new Command(ExecuteShowCourse);
        }
        private async void GetCoursesAsync()
        {
            var s = new CoursesService();
            AllCourses = await s.GetAllCourses();
        }
        private List<Course> _allCourses;
        public List<Course> AllCourses
        {
            set
            {
                if (_allCourses != value)
                {
                    _allCourses = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AllCourses"));
                    }
                }
            }
            get
            {
                return _allCourses;
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
        async void ExecuteShowCourse()
        {
            await _navigation.PushAsync(new CourseDetailsPage(_navigation, SelectedCourse));
        }
    }
}