using Arxius.Services.PCL.Entities;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class EmployeeDetailsViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        public EmployeeDetailsViewModel(INavigation navi, Employee _news)
        {
            Name = _news.Name;
            Email = _news.Email;
            Url = _news.Url;

            _navigation = navi;
        }

        #region Bindable properties
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }
        }
        private string _eMail;
        public string Email
        {
            get { return _eMail; }
            set
            {
                if (_eMail != value)
                {
                    _eMail = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                    }
                }
            }
        }
        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                if (_url != value)
                {
                    _url = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Url"));
                    }
                }
            }
        }

        #endregion


    }
}