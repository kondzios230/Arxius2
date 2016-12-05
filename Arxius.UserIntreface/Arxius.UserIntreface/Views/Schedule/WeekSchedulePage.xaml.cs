
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
        private Grid grid;
        public WeekSchedulePage(INavigation _navi)
        {
            InitializeComponent();
            BindingContext = new WeekScheduleViewModel(_navi,this);
        }
        public void GenerateGrid(int rows,int beginingHour)
        {            
            var list = new RowDefinitionCollection();
            for (var i = 0; i < rows; i++)
            {
                list.Add(new RowDefinition { Height = GridLength.Auto });
            }
            grid = new Grid
            {
                RowDefinitions = list,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute)  },
                    new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) }
                }
            };
            for (var i = 0; i < rows; i++)
            {
                grid.Children.Add(new Label
                {
                    Text = (beginingHour+i).ToString()+":00",
                    HorizontalOptions = LayoutOptions.Center
                }, 0, i);
            }
            var stack = new StackLayout();
            stack.Children.Add(grid);
            var scroll = new ScrollView() { Content = stack,Orientation = ScrollOrientation.Horizontal };
            Content = scroll;
        }
        public void MapLessons(List<Lesson> lessons, int hours, int begingHour)
        {
            var listOfStudyHours = new List<StudyHour>();
            foreach (var lesson in lessons)
            {
                var d = (int)lesson.Day;
                var deltaStart = lesson.StartTime.Hour - begingHour;
                var deltaEnd = lesson.EndTime.Hour - begingHour;

                for (var dT = deltaStart; dT < deltaEnd; dT++)
                {
                    listOfStudyHours.Add(new StudyHour() { Lesson = lesson, Hour = dT });
                }

            }
            var lessonsGroups = listOfStudyHours.GroupBy(c => new { c.Hour, c.Lesson.Day });
            foreach(var hourGrouping in lessonsGroups)
            {
                var _stack = new StackLayout();
                foreach (var studyLesson in hourGrouping)
                    _stack.Children.Add(new Label() { Text = "AAA" });
                grid.Children.Add(_stack, (int)hourGrouping.Key.Day, hourGrouping.Key.Hour);
            }
         
        }
    }
}

