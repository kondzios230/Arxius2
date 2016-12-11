using Arxius.Services.PCL.Entities;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class EmployeeDetailsViewModel : AbstractViewModel
    {
        public EmployeeDetailsViewModel(INavigation navi, Employee _news, Page page)
        {

            Name = _news.Name;
            Email = _news.Email;
            Url = _news.Url;
            _page = page;
            Navigation = navi;
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
                    OnPropertyChanged("Name");
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
                    OnPropertyChanged("Email");
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
                    OnPropertyChanged("Url");
                }
            }
        }

        #endregion


    }
}