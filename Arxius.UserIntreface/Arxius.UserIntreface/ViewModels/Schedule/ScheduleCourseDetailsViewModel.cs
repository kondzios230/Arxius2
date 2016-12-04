using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class ScheduleCourseDetailsViewModel : INotifyPropertyChanged
    {      
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;

        public ScheduleCourseDetailsViewModel(INavigation navi,Course course)
        {
            var cs = new CoursesService();
         //   cs.GetCourseDetails(course);
            _navigation = navi;
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
                return _Course.Name;
            }
        }
        public string CourseEcts
        {
            get
            {
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
                return _Course.Classes;
            }
        }
        
        #endregion


    }
}