﻿using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class CourseListViewModel : AbstractViewModel
    {
        
        public CourseListViewModel(INavigation navi, Page page)
        {
            cService = new CoursesService();
            _page = page;
            Navigation = navi;
            GetCoursesAsync();
            ShowCourse = new Command(ExecuteShowCourse);
            Refresh = new Command(ExecuteRefresh);
        }
        private async void GetCoursesAsync(bool clear =false)
        {
            try
            {
                IsAIRunning = true;
                AllCourses = await cService.GetAllCourses(clear);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            IsAIRunning = false;
        }
        private List<Course> _allCourses;
        public List<Course> AllCourses
        {
            set
            {
                if (_allCourses != value)
                {
                    _allCourses = value;


                    OnPropertyChanged("AllCourses");

                }
            }
            get
            {
                return _allCourses;
            }
        }
        private Course _selectedItem;
        public Course SelectedCourse
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;

                if (_selectedItem == null)
                    return;

                ShowCourse.Execute(_selectedItem);

                SelectedCourse = null;
            }
        }
        public ICommand ShowCourse { private set; get; }
        async void ExecuteShowCourse()
        {
            await Navigation.PushAsync(new CourseDetailsPage(Navigation, SelectedCourse));
        }
        public ICommand Refresh { private set; get; }
        void ExecuteRefresh()
        {
            GetCoursesAsync(true);
        }
    }
}