using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    public abstract class AbstractViewModel : INotifyPropertyChanged
    {
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
                BreadCrumb = string.Join(" > ", s);
            }
            get
            {
                return _navigation;
            }
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