using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class StudentsListViewModel : AbstractViewModel
    {
        private ICourseService cService;
        public StudentsListViewModel(INavigation navi, _Class _class,Page page)
        {
            _page = page;
            Navigation = navi;
            cService = new CoursesService();
            GetStudentsListAsync(_class);
            Refresh = new Command(ExecuteRefresh);
        }
        private async void GetStudentsListAsync(_Class _class)
        {
            var tuple = await cService.GetStudentsList(_class);
            Enrolled = tuple.Item1;
            Total = tuple.Item2;
            StudentsList = tuple.Item3;
            ClassField = _class;
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
                    
                }
            }
        }
        private int _enrolled;
        public int Enrolled
        {
            get { return _enrolled; }
            set
            {
                if (_enrolled != value)
                {
                    _enrolled = value;
                    
                        OnPropertyChanged("Enrolled");
                                       
                }
            }
        }
        private int _total;
        public int Total
        {
            get { return _total; }
            set
            {
                if (_total != value)
                {
                    _total = value;
                    
                        OnPropertyChanged("Total");
                    
                }
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
        public ICommand Refresh { private set; get; }
        async void ExecuteRefresh()
        {
            (_page as StudentsListPage).SetRefreshImage("refresh2.jpg");
            var tuple = await cService.GetStudentsList(_class,true);
            (_page as StudentsListPage).SetRefreshImage("refresh.jpg");
            Enrolled = tuple.Item1;
            Total = tuple.Item2;
            StudentsList = tuple.Item3;
            ClassField = _class;
        }


    }
    }