using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
        private Color cellColor = Color.FromHex("#6cc9f7");
        private Color headersColor = Color.White;
        private Color todayColor = Color.FromHex("#cde6f2");
        private Color todayCellColor = Color.FromHex("#49c2ff");
        public WeekScheduleViewModel(INavigation navi, WeekSchedulePage page)
        {
            this.page = page;
            _navigation = navi;
            GetUserScheduleAsync();
        }
        private async void GetUserScheduleAsync()
        {
            var s = new CoursesService();
            Schedule = await s.GetUserPlanForCurrentSemester();
            AnalyzeSchedule(Schedule);
        }

        private void AnalyzeSchedule(List<Course>  _Schedule)
        {
            var lStart = new List<Lesson>();
            var lEnd = new List<Lesson>();
            foreach (var course in Schedule)
            {
                foreach (var c in course.Classes)
                    lStart.AddRange(c.Lessons);
            }
            lEnd.AddRange(lStart);
            lStart.Sort(new LessonStartTimeComparer());
            lEnd.Sort(new LessonStartTimeComparer());
            var startHour = lStart[0].StartTime.Hour;
            var endHour = lEnd[lEnd.Count-1].EndTime.Hour;
            var hourDifference = endHour- startHour;
            GenerateGrid(hourDifference, startHour,lStart);
            
        }
        private void GenerateGrid(int rows, int startHour, List<Lesson> lessons)
        {
            var grid = new Grid(){ ColumnSpacing = 1d, RowSpacing = 0.8d};
            var day = (int)DateTime.Now.DayOfWeek;
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) });//hourColumn
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });//dayRow
            grid.Children.Add(new Label { BackgroundColor = headersColor },0, 0);

            for (var i = 0; i < 5; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Absolute) });
                if(i+1==day)
                    grid.Children.Add(new Label { Text = ((DniTygodnia)((i+1)%7)).ToString(),BackgroundColor = todayColor,HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, i+1,0);
                else
                    grid.Children.Add(new Label { Text = ((DniTygodnia)((i + 1) % 7)).ToString(), BackgroundColor = headersColor, HorizontalTextAlignment = TextAlignment.Center }, i + 1, 0);

            }
           
            for (var i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Children.Add(new Label { Text = (startHour + i).ToString() + ":00", BackgroundColor = headersColor, HorizontalTextAlignment = TextAlignment.Center }, 0, i+1);
                if (day != 0 && day != 6)
                    grid.Children.Add(new Label { BackgroundColor = todayColor}, day, i + 1);
            }
           
            MapLessons(lessons, rows, startHour,grid);
            var stack = new StackLayout();
            stack.Children.Add(grid);
            var scroll = new ScrollView() { Content = stack, Orientation = ScrollOrientation.Both};
            page.Content = scroll;
            page.grid = grid;
        }
        private void MapLessons(List<Lesson> lessons, int hours, int begingHour,Grid grid)
        {
            
            var listOfStudyHours = new List<StudyHour>();
            foreach (var lesson in lessons)
            {
                for (var dT = lesson.StartTime.Hour; dT < lesson.EndTime.Hour; dT++)
                    listOfStudyHours.Add(new StudyHour() { Lesson = lesson, Hour = dT - begingHour + 1 });
            }           

            foreach (var hourGrouping in listOfStudyHours.GroupBy(c => new { c.Hour, c.Lesson.Day }))
            {
                var studyHourGrid = new Grid() { ColumnSpacing = 0.9d };
              
                int i = 0;
                foreach (var studyLesson in hourGrouping)
                {
                    studyHourGrid.Children.Add(GenerateLessonStack(studyLesson), i,0);
                    i++;
                }
                grid.Children.Add(studyHourGrid, (int)hourGrouping.Key.Day, hourGrouping.Key.Hour);                
            }

        }
        private StackLayout GenerateLessonStack(StudyHour studyLesson)
        {
            var day = DateTime.Now.DayOfWeek;
            var span = new FormattedString();
            span.Spans.Add(new Span { Text = ClassTypeEnums.ToFriendlyShortString(studyLesson.Lesson.Type) + "\t", FontSize = 10 });
            span.Spans.Add(new Span { Text = "s: " + studyLesson.Lesson.Classroom, FontSize = 8 });

            var lessonStack = new StackLayout() { Spacing = 1 };
            if (studyLesson.Lesson.Day == day)
                lessonStack.BackgroundColor = todayCellColor;
            else
                lessonStack.BackgroundColor = cellColor;
            lessonStack.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => ExecuteShowPlan(studyLesson.Lesson.Course)) });
            lessonStack.Children.Add(new Label() { Text = studyLesson.Lesson.Course.Name, FontSize = 12, FontAttributes = FontAttributes.Bold ,TextColor= Color.White});
            lessonStack.Children.Add(new Label() { FormattedText = span,VerticalTextAlignment=TextAlignment.End, TextColor = Color.White });
            return lessonStack;
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
      
        async void ExecuteShowPlan(Course course)
        {
          await _navigation.PushAsync(new CourseDetailsPage(_navigation, course));
        }
    }
}