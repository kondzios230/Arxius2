using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class CourseListViewModel : AbstractViewModel
    {
        public ICommand ShowCourse { private set; get; }
        public CourseListViewModel(INavigation navi, Page page)
        {
            cService = new CoursesService();
            _page = page;
            Navigation = navi;
            GetCoursesAsync();
            ShowCourse = new Command(async ()=> await Navigation.PushAsync(new CourseDetailsPage(Navigation, SelectedCourse)));
            Refresh = new Command(()=>GetCoursesAsync(true));
        }
        private async void GetCoursesAsync(bool clear =false)
        {
            try
            {
                IsAIRunning = true;
                var courses = await cService.GetAllCourses(clear);
                var groupedCourses = courses.GroupBy(c => c.Semester);
                var allCourses = new List<CourseGroupedCollection>();
                foreach(var group in groupedCourses)
                {
                    var x = new CourseGroupedCollection(group.Key);
                    x.AddRange(group.ToList());
                    allCourses.Add(x);
                }
                AllCourses = allCourses;
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            IsAIRunning = false;
        }
        private List<CourseGroupedCollection> _allCourses;
        public List<CourseGroupedCollection> AllCourses
        {
            set
            {
                if (_allCourses != value)
                {
                    _allCourses = value;


                    OnPropertyChanged("AllCourses");

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

        
        
        
    }
}