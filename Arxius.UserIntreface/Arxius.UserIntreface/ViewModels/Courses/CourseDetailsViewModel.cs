using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class CourseDetailsViewModel : INotifyPropertyChanged
    {      
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        private CoursesService cService;
        public CourseDetailsViewModel(INavigation navi,Course course)
        {
            _navigation = navi;
            cService = new CoursesService();
            GetCourseDetailsAsync(course);

            EnrollOrUnroll = new Command<EnrollmentClass>(ExecuteEnrollOrUnroll);
            ShowList = new Command<_Class>(ExecuteShowList);
        }
        private async void GetCourseDetailsAsync(Course course)
        {
            
           await cService.GetCourseWideDetails(course);
            
            _Course = course;
        }
        #region BindableProperties
        private Course _courseBF;
        public Course _Course
        {
            set
            {
                if (_courseBF != value)
                {
                    _courseBF = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Course")); 
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseName")); 
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseClasses"));
                    }
                }
            }
            get
            {
                return _courseBF;
            }
        }

        public string CourseName
        {
            set
            {
                _Course.Name = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CourseName"));
                }
            }
            get
            {
                if (_Course == null) return "";
                return _Course.Name;
            }
        }
        public string CourseEcts
        {
            get
            {
                if (_Course == null) return "";
                return "ECTS: "+_Course.Ects.ToString();
            }
        }
        public List<_Class> CourseClasses
        {
            set
            {
                _Course.Classes = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CourseClasses"));
                }
            }
            get
            {
                if (_Course == null) return new List<_Class> ();
                return _Course.Classes;
            }
        }

        #endregion
        public ICommand EnrollOrUnroll { private set; get; }
        async void ExecuteEnrollOrUnroll(EnrollmentClass c)
        {
          var x = await cService.EnrollOrUnroll(c);
        }

        public ICommand ShowList { private set; get; }
        async void ExecuteShowList(_Class c)
        {
            await _navigation.PushAsync(new StudentsListPage(_navigation, c));
        }


    }
}