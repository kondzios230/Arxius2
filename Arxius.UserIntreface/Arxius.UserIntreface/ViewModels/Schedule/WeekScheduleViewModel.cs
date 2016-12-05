using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using static Arxius.Services.PCL.Entities.Lesson;

namespace Arxius.UserIntreface.ViewModels
{
    class WeekScheduleViewModel : INotifyPropertyChanged
    {
      
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        private WeekSchedulePage page;
        public WeekScheduleViewModel(INavigation navi, WeekSchedulePage page)
        {
            this.page = page;
            _navigation = navi;
            GetUserScheduleAsync();
            ShowCourse = new Command(ExecuteShowPlan);
        }
        private async void GetUserScheduleAsync()
        {
            var s = new CoursesService();
            Schedule = await s.GetUserPlanForCurrentSemester();
            Foo(Schedule);
        }

        private void Foo(List<Course>  _Schedule)
        {
            var lStart = new List<Lesson>();
            var lEnd = new List<Lesson>();
            foreach (var course in Schedule)
            {
                foreach (var c in course.Classes)
                {
                    lStart.AddRange(c.Lessons);
                    lEnd.AddRange(c.Lessons);
                }
            }
            lStart.Sort(new LessonStartTimeComparer());
            lEnd.Sort(new LessonStartTimeComparer());
            var firstTime = lStart[0].StartTime;
            var lastTime = lEnd[lEnd.Count-1].EndTime;
            var hours = lastTime.Hour - firstTime.Hour;
            page.GenerateGrid(hours, firstTime.Hour);
            page.MapLessons(lStart, hours, firstTime.Hour);
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
      
        public ICommand ShowCourse { private set; get; }
        async void ExecuteShowPlan()
        {
        //    await _navigation.PushAsync(new ScheduleCourseDetailsPage(_navigation, SelectedCourse));
        }
    }
}