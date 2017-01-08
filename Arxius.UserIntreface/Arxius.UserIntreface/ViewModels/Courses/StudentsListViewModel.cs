using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class StudentsListViewModel : AbstractViewModel
    {

        public StudentsListViewModel(INavigation navi, _Class _class, Page page)
        {
            _page = page;
            Navigation = navi;
            cService = new CoursesService();
            GetStudentsListAsync(_class);
            Refresh = new Command(() => GetStudentsListAsync(_class, true));
        }
        private async void GetStudentsListAsync(_Class _class, bool clear = false)
        {
            try
            {
                IsAIRunning = true;
                var tuple = await cService.GetStudentsList(_class, clear);
                Enrolled = tuple.Item1.ToString();
                Total = tuple.Item2.ToString();
                StudentsList = tuple.Item3;
                ClassField = _class;
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            IsAIRunning = false;
        }
        #region BindableProperties
        private _Class _class;
        public _Class ClassField
        {
            get { return _class; }
            set
            {
                if (_class != value)
                {
                    _class = value;
                    OnPropertyChanged("ClassField");
                    OnPropertyChanged("ClassType");
                }
            }
        }
        public string ClassType
        {
            get
            {
                if (ClassField == null) return "";
                return ClassTypeEnums.ToFriendlyString(ClassField.ClassType);
            }
        }
        private string _enrolled;
        public FormattedString Enrolled
        {
            get
            {
                var span = new FormattedString();
                span.Spans.Add(new Span { Text = "Zapisanych: ", FontAttributes = FontAttributes.Bold });
                span.Spans.Add(new Span { Text = _enrolled });
                return span;
            }
            set
            {
                _enrolled = (string)value;
                OnPropertyChanged("Enrolled");
            }
        }
        private string _total;
        public FormattedString Total
        {
            get
            {
                var span = new FormattedString();
                span.Spans.Add(new Span { Text = "Max: ", FontAttributes = FontAttributes.Bold });
                span.Spans.Add(new Span { Text = _total });
                return span;
            }
            set
            {
                _total = (string)value;
                OnPropertyChanged("Total");
            }
        }
        private List<Student> _studentsList;
        public List<Student> StudentsList
        {
            get { return _studentsList; }
            set
            {
                if (_studentsList != value)
                {
                    _studentsList = value;

                    OnPropertyChanged("StudentsList");

                }
            }
        }


        #endregion

    }
}