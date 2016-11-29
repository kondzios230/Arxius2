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
    class StudentsListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        private CoursesService cService;
        public StudentsListViewModel(INavigation navi, _Class _class)
        {
            _navigation = navi;
            cService = new CoursesService();
            GetStudentsListAsync(_class);
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

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ClassField"));
                    }
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

                    if (PropertyChanged != null)
                    {
                        
                        PropertyChanged(this, new PropertyChangedEventArgs("Enrolled"));
                                            }
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

                    if (PropertyChanged != null)
                    {

                        PropertyChanged(this, new PropertyChangedEventArgs("Total"));
                    }
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

                    if (PropertyChanged != null)
                    {

                        PropertyChanged(this, new PropertyChangedEventArgs("StudentsList"));
                    }
                }
            }
        }


        #endregion



    }
    }