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
    class CourseDetailsViewModel : AbstractViewModel
    {
        private Course emptyCourse;
        public ICommand EnrollOrUnroll { private set; get; }
        public ICommand ShowList { private set; get; }
        public ICommand SaveCourseNotes { private set; get; }
        private bool wasClicked;
        public CourseDetailsViewModel(INavigation navi, Course course, Page page)
        {
            _page = page;
            emptyCourse = course;
            Navigation = navi;
            CanEnroll = false;
            cService = new CoursesService();
            GetCourseDetailsAsync();

            EnrollOrUnroll = new Command<_Class>(ExecuteEnrollOrUnroll, (s) => CanEnroll);
            SaveCourseNotes = new Command(ExecuteSaveCourseNotes, () => CourseNotes.Length > 0);
            ShowList = new Command<_Class>(async (c) => await Navigation.PushAsync(new StudentsListPage(Navigation, c)));
            Refresh = new Command(() => GetCourseDetailsAsync(true));
        }
        private async void GetCourseDetailsAsync(bool clear = false)
        {
            try
            {
                _Course = null;
                IsAIRunning = true;
                _Course = await cService.GetCourseWideDetails(emptyCourse, clear);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
                await Navigation.PopAsync();
            }
            IsAIRunning = false;

        }
        #region BindableProperties
        private Course _courseBF;
        public Course _Course
        {
            set
            {
                _courseBF = value;
                OnPropertyChanged("Course");
                OnPropertyChanged("CourseNotes");
                OnPropertyChanged("CourseName");
                OnPropertyChanged("CourseEcts");
                OnPropertyChanged("CourseClasses");
                OnPropertyChanged("CanEnroll");
                OnPropertyChanged("CourseKind");
                OnPropertyChanged("CourseHourSchema");
                OnPropertyChanged("CourseGroup");
                OnPropertyChanged("CourseIsExam");
                OnPropertyChanged("CourseIsForFirstYear");

                if (_courseBF != null && _courseBF.Classes.Count != 0)
                {
                    CanEnroll = _courseBF.Classes.All(c => c.IsEnrollment);
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


                OnPropertyChanged("CourseName");

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

                    OnPropertyChanged("CourseNotes");

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

                    OnPropertyChanged("CanEnroll");

                }
            }
        }
      
        public FormattedString CourseEcts
        {
            get
            {
                if (_Course == null) return "";
                var span = new FormattedString();
                span.Spans.Add(new Span { Text = "ECTS: ", FontAttributes = FontAttributes.Bold });
                span.Spans.Add(new Span { Text = _Course.Ects.ToString()});
                return span;
            }
        }
        private bool _isHoursVisible;
        public bool IsHoursVisible
        {
            get { return _isHoursVisible; }
            set
            {
                if (_isHoursVisible != value)
                {
                    _isHoursVisible = value;
                    OnPropertyChanged("IsHoursVisible");
                }
            }
        }
        public FormattedString CourseHourSchema
        {
            get
            {
                IsHoursVisible = !(_Course == null || _Course.HoursSchema.Count == 0);
                if (_Course == null) return "";
                
                var span = new FormattedString();
                foreach (var item in _Course.HoursSchema)
                {
                    span.Spans.Add(new Span { Text = item.Key, FontAttributes = FontAttributes.Bold });
                    span.Spans.Add(new Span { Text = ". - ", FontAttributes = FontAttributes.Bold });
                    span.Spans.Add(new Span { Text = item.Value + "h\t" });
                }
                return span;
            }
        }
        private bool _isKindVis;
        public bool IsKindVisible
        {
            get { return _isKindVis; }
            set
            {
                if (_isKindVis != value)
                {
                    _isKindVis = value;
                    OnPropertyChanged("IsKindVisible");
                }
            }
        }
        public FormattedString CourseKind
        {
            get
            {
                IsKindVisible = !(_Course == null || _Course.Kind == null || _Course.Kind.Length == 0);
                if (_Course == null || _Course.Kind == null || _Course.Kind.Length==0) return "";
                var span = new FormattedString();
                span.Spans.Add(new Span { Text = "R: ", FontAttributes = FontAttributes.Bold });
                span.Spans.Add(new Span { Text = _Course.Kind });
                return span;
            }
        }
        private bool _isGroupVis;
        public bool IsGroupVisible
        {
            get { return _isGroupVis; }
            set
            {
                if (_isGroupVis != value)
                {
                    _isGroupVis = value;                   
                    OnPropertyChanged("IsGroupVisible");
                }
            }
        }
        public FormattedString CourseGroup
        {
            get
            {
                IsGroupVisible = !(_Course == null || _Course.GroupOfEffects == null || _Course.GroupOfEffects.Length == 0);
                if (_Course == null || _Course.GroupOfEffects == null || _Course.GroupOfEffects.Length == 0) return "";
                var span = new FormattedString();
                span.Spans.Add(new Span { Text = "G: ", FontAttributes = FontAttributes.Bold });
                span.Spans.Add(new Span { Text = _Course.GroupOfEffects });
                return span;
            }
        }
        public FormattedString CourseIsExam
        {
            get
            {
                if (_Course == null) return "";
                var span = new FormattedString();
                span.Spans.Add(new Span { Text = "Egzamin: ", FontAttributes = FontAttributes.Bold });
                span.Spans.Add(new Span { Text = _Course.IsExam ? "Tak" : "Nie" });
                return span;
            }

        }
        public FormattedString CourseIsForFirstYear
        {
            get
            {
                if (_Course == null) return "";
                var span = new FormattedString();
                span.Spans.Add(new Span { Text = "Przyjazny dla I roku: ", FontAttributes = FontAttributes.Bold });
                span.Spans.Add(new Span { Text = _Course.SugestedFor1stYear ? "Tak" : "Nie" });
                return span;
            }
        }
        public List<_Class> CourseClasses
        {
            set
            {
                _Course.Classes = value;


                OnPropertyChanged("CourseClasses");

            }
            get
            {
                if (_Course == null) return new List<_Class>();
                return _Course.Classes;
            }
        }

        #endregion

        async void ExecuteEnrollOrUnroll(_Class c)
        {
            if (!wasClicked)
            {
                wasClicked = true;
                try
                {

                    var enrollmentTuple = await cService.EnrollOrUnroll(c);
                    if (enrollmentTuple.Item1)
                    {
                        GetCourseDetailsAsync(true);
                        Cache.Clear("GetUserPlanForCurrentSemester");
                        if (BreadCrumb.Contains(Properties.Resources.PageNameSchedule))
                            enrollmentTuple.Item3.Add("Odśwież plan zajęć");
                    }
                    MessagingCenter.Send(this, Properties.Resources.MsgEnrollment, Tuple.Create(enrollmentTuple.Item1, "", enrollmentTuple.Item3));
                }
                catch (ArxiusException e)
                {
                    MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
                }
                wasClicked = false;
            }

        }
        async void ExecuteSaveCourseNotes()
        {
            var fileService = DependencyService.Get<ISaveAndLoad>();
            try
            {
                await fileService.SaveTextAsync(string.Format("Notes{0}.txt", _Course.Name), CourseNotes);
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