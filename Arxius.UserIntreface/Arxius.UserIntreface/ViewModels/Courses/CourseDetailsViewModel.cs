using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class CourseDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        private ICourseService cService;
        public CourseDetailsViewModel(INavigation navi, Course course)
        {
            _navigation = navi;
            cService = new CoursesService();
            GetCourseDetailsAsync(course);
            CanEnroll = false;
            EnrollOrUnroll = new Command<_Class>(ExecuteEnrollOrUnroll, (s) => CanEnroll);
            SaveCourseNotes = new Command(ExecuteSaveCourseNotes, () => CourseNotes.Length > 0);
            ShowList = new Command<_Class>(ExecuteShowList);
        }
        private async void GetCourseDetailsAsync(Course course)
        {

            _Course = await cService.GetCourseWideDetails(course);


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
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseEcts"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseClasses"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseNotes"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CanEnroll"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseHourSchema"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseKind"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseGroup"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseIsExam"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseIsForFirstYear"));

                        if (_courseBF != null && _courseBF.Classes.Count != 0)
                        {
                            CanEnroll = _courseBF.Classes.All(c => c.IsEnrollment);
                        }


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

        public string CourseNotes
        {
            set
            {
                if (_Course != null)
                {
                    _Course.Notes = value;
                    if (SaveCourseNotes != null)
                        ((Command)SaveCourseNotes).ChangeCanExecute();
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CourseNotes"));
                    }
                }
            }
            get
            {
                if (_Course == null || _Course.Notes == null) return "";
                return _Course.Notes;
            }
        }
        private bool canEnroll;

        public bool CanEnroll
        {
            get { return canEnroll; }
            set
            {
                if (canEnroll != value)
                {
                    canEnroll = value;
                    if (EnrollOrUnroll != null)
                        ((Command)EnrollOrUnroll).ChangeCanExecute();
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CanEnroll"));

                    }
                }
            }
        }
        public string CourseEcts
        {
            get
            {
                if (_Course == null) return "";
                return "ECTS: " + _Course.Ects.ToString();
            }
        }
        public FormattedString CourseHourSchema
        {
            get
            {
                if (_Course == null) return "";

                var span = new FormattedString();
                foreach (var item in _Course.HoursSchema)
                {
                    span.Spans.Add(new Span { Text = item.Key, FontSize = 17 });
                    span.Spans.Add(new Span { Text = " - ", FontSize = 17 });
                    span.Spans.Add(new Span { Text = item.Value + "\t", FontSize = 15 });
                }
                return span;
            }
        }
        public string CourseKind
        {
            get
            {
                if (_Course == null) return "";
                return _Course.Kind;
            }
        }
        public string CourseGroup
        {
            get
            {
                if (_Course == null) return "";
                return _Course.GroupOfEffects;
            }
        }
        public string CourseIsExam
        {
            get
            {
                if (_Course == null) return "";
                return string.Format("Egzamin: {0}", _Course.IsExam ? "Tak" : "Nie");
            }

        }
        public string CourseIsForFirstYear
        {
            get
            {
                if (_Course == null) return "";
                return string.Format("Przyjazny dla I roku: {0}", _Course.SugestedFor1stYear ? "Tak" : "Nie");
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
                if (_Course == null) return new List<_Class>();
                return _Course.Classes;
            }
        }

        #endregion
        public ICommand EnrollOrUnroll { private set; get; }
        async void ExecuteEnrollOrUnroll(_Class c)
        {
            var enrollmentTuple = await cService.EnrollOrUnroll(c);
            var newTuple = Tuple.Create(enrollmentTuple.Item1, c.ButtonEnrollText, enrollmentTuple.Item3);
            if (enrollmentTuple.Item1)
            {
                c.ButtonEnrollText = enrollmentTuple.Item2;
                c.IsSignedIn = !c.IsSignedIn;
                PropertyChanged(this, new PropertyChangedEventArgs("CourseClasses"));
            }
            MessagingCenter.Send(this, Properties.Resources.MsgEnrollment, newTuple);
        }

        public ICommand ShowList { private set; get; }
        async void ExecuteShowList(_Class c)
        {
            await _navigation.PushAsync(new StudentsListPage(_navigation, c));

        }
        public ICommand SaveCourseNotes { private set; get; }
        async void ExecuteSaveCourseNotes()
        {
            var fileService = DependencyService.Get<ISaveAndLoad>();
            try
            {
                await fileService.SaveTextAsync(string.Format("Notes{0}.txt", _Course.CourseID), CourseNotes);
            }
            catch
            {
                MessagingCenter.Send(this, Properties.Resources.MsgSave, false);
                return;
            }
            MessagingCenter.Send(this, Properties.Resources.MsgSave, true);

        }


    }
}