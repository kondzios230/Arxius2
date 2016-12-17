using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
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
    class WeekScheduleViewModel : AbstractViewModel
    {
        private WeekSchedulePage page;
        private Color cellColor = Color.FromHex("#72d0ff");
        private Color headersColor = Color.White;
        private Color todayColor = Color.FromHex("#bfe9ff");
        private Color todayCellColor = Color.FromHex("#35bbff");
        private Color clickedCellColor = Color.FromHex("#00aaff");
        private ICourseService cService;
        public WeekScheduleViewModel(INavigation navi, WeekSchedulePage page)
        {
            cService = new CoursesService();
            _page = this.page = page;
            page.Content = new StackLayout() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            (page.Content as StackLayout).Children.Add(new ActivityIndicator() { Color = todayCellColor, IsVisible = true, IsRunning = true });
            Navigation = navi;
            GetUserScheduleAsync();
        }
        private async void GetUserScheduleAsync()
        {
            try
            {               
                Schedule = await cService.GetUserPlanForCurrentSemester();
                AnalyzeSchedule(Schedule);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
                await Navigation.PopAsync();
            }
        }

        private void AnalyzeSchedule(List<Course> _Schedule)
        {
            var lStart = new List<Lesson>();
            var lEnd = new List<Lesson>();
            foreach (var course in Schedule)
            {
                foreach (var c in course.Classes.Where(c => c.IsSignedIn))
                    lStart.AddRange(c.Lessons);
            }
            if (lStart.Count == 0)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgEmptySchedule);
                Navigation.PopAsync();
                Cache.Clear("GetUserPlanForCurrentSemester");
            }
            else
            {
                lEnd.AddRange(lStart);
                lStart.Sort(new LessonStartTimeComparer());
                lEnd.Sort(new LessonStartTimeComparer());
                var startHour = lStart[0].StartTime.Hour;
                var endHour = lEnd[lEnd.Count - 1].EndTime.Hour;
                var hourDifference = endHour - startHour;
                GenerateGrid(hourDifference, startHour, lStart);
            }
        }
        private void GenerateGrid(int rows, int startHour, List<Lesson> lessons)
        {
            var grid = new Grid() { ColumnSpacing = 1d, RowSpacing = 0.8d };
            var day = (int)DateTime.Now.DayOfWeek;
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) });//hourColumn
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });//dayRow
            grid.Children.Add(new Label { BackgroundColor = headersColor }, 0, 0);

            for (var i = 0; i < 5; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Absolute) });
                if (i + 1 == day)
                    grid.Children.Add(new Label { Text = ((DniTygodnia)((i + 1) % 7)).ToString(), BackgroundColor = todayColor, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, i + 1, 0);
                else
                    grid.Children.Add(new Label { Text = ((DniTygodnia)((i + 1) % 7)).ToString(), BackgroundColor = headersColor, HorizontalTextAlignment = TextAlignment.Center }, i + 1, 0);

            }

            for (var i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Children.Add(new Label { Text = (startHour + i).ToString() + ":00", BackgroundColor = headersColor, HorizontalTextAlignment = TextAlignment.Center }, 0, i + 1);
                if (day != 0 && day != 6)
                    grid.Children.Add(new Label { BackgroundColor = todayColor }, day, i + 1);
            }

            MapLessons(lessons, rows, startHour, grid);
            var scroll = new ScrollView() { Content = grid, Orientation = ScrollOrientation.Both };
            var stack = new StackLayout() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };

            var headerGrid = new Grid() { HorizontalOptions = LayoutOptions.EndAndExpand, Padding = new Thickness(2) };
            var image = new Image() { Source = "refresh2.png" };
            image.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => ExecuteForceRefresh()) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(25, GridUnitType.Absolute) });
            headerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) });
            headerGrid.Children.Add(new Label() { Text = BreadCrumb, FontAttributes = FontAttributes.Italic, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), Margin = 10, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalTextAlignment = TextAlignment.Center }, 0, 0);
            headerGrid.Children.Add(image, 1, 0);

            stack.Children.Add(headerGrid);
            stack.Children.Add(scroll);
            page.Content = stack;
            page.grid = grid;
        }
        private void MapLessons(List<Lesson> lessons, int hours, int begingHour, Grid grid)
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
                    studyHourGrid.Children.Add(GenerateLessonStack(studyLesson), i, 0);
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
            lessonStack.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => ExecuteShowPlan(studyLesson.Lesson.Course, lessonStack)) });
            lessonStack.Children.Add(new Label() { Text = studyLesson.Lesson.Course.Name, FontSize = 12, FontAttributes = FontAttributes.Bold, TextColor = Color.White });
            lessonStack.Children.Add(new Label() { FormattedText = span, VerticalTextAlignment = TextAlignment.End, TextColor = Color.White });
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

                    OnPropertyChanged("Schedule");
                }
            }
            get
            {
                return _schedule;
            }
        }

        async void ExecuteShowPlan(Course course, StackLayout l)
        {
            var prevColor = l.BackgroundColor;
            l.BackgroundColor = clickedCellColor;
            await Navigation.PushAsync(new CourseDetailsPage(Navigation, course));
            l.BackgroundColor = prevColor;
        }

        async void ExecuteForceRefresh()
        {
            var ai = new ActivityIndicator() { Color = todayCellColor, IsVisible = true, IsRunning = true };
            try
            {
                ((page.Content as StackLayout).Children[0] as Grid).Children.Add(ai);
                Schedule = await cService.GetUserPlanForCurrentSemester(true);
                AnalyzeSchedule(Schedule);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            ai.IsRunning = false;
        }
    }
}