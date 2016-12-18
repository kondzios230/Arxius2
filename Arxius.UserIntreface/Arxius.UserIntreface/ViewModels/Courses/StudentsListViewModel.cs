﻿using Arxius.Services.PCL;
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
        
        public StudentsListViewModel(INavigation navi, _Class _class, Page page)
        {
            _page = page;
            Navigation = navi;
            cService = new CoursesService();
            GetStudentsListAsync(_class);
            Refresh = new Command(()=> GetStudentsListAsync(_class, true));
        }
        private async void GetStudentsListAsync(_Class _class, bool clear = false)
        {
            try
            {
                var tuple = await cService.GetStudentsList(_class,clear);
                Enrolled = tuple.Item1.ToString();
                Total = tuple.Item2.ToString();
                StudentsList = tuple.Item3;
                ClassField = _class;
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
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
            get {
                if (ClassField == null) return "";
                return ClassTypeEnums.ToFriendlyString(ClassField.ClassType);
            }
        }
        private string _enrolled;
        public string Enrolled
        {
            get { return "Zapisanych: " + _enrolled; }
            set
            {
                if (_enrolled != value)
                {
                    _enrolled = value;

                    OnPropertyChanged("Enrolled");

                }
            }
        }
        private string _total;
        public string Total
        {
            get { return "Max: " + _total; }
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
      
    }
}