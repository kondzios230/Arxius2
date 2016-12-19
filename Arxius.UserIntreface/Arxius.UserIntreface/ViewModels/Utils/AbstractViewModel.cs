using Arxius.Services.PCL.Interfaces_and_mocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    public abstract class AbstractViewModel : INotifyPropertyChanged
    {
        protected ICourseService cService;
        protected IUtilsService uService;
        public ICommand Refresh { protected set; get; }
        public event PropertyChangedEventHandler PropertyChanged;
        private string _breadCrumb;
        protected Page _page;
        public string BreadCrumb
        {
            set
            {
                if (_breadCrumb != value)
                {
                    _breadCrumb = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("BreadCrumb"));
                    }
                }
            }
            get
            {
                return _breadCrumb;
            }
        }
        private INavigation _navigation;
        public INavigation Navigation
        {
            set
            {
                _navigation = value;
                var s = new List<string>();
                foreach (var x in value.NavigationStack)
                {
                    s.Add(x.Title);
                }
                if (_page != null && (s.Count==0 || s[s.Count - 1] != _page.Title))
                    s.Add(_page.Title);
                BreadCrumb = string.Join(" > ", s.Skip(Math.Max(0, s.Count() - 2)));
            }
            get
            {
                return _navigation;
            }
        }
        private bool isAIRunning;
        public bool IsAIRunning
        {
            get { return isAIRunning; }
            set { isAIRunning = value; OnPropertyChanged("IsAIRunning"); }
        }
        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}