using Arxius.Services.PCL.Entities;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class NewsDetailsViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        public NewsDetailsViewModel(INavigation navi, News _news)
        {
            Title = _news.Title;
            Author = _news.Author;
            Date= _news.Date;
            Content = _news.Content;

            _navigation = navi;
        }

        #region Bindable properties
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                    }
                }
            }
        }
        private string _author;
        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    _author = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Author"));
                    }
                }
            }
        }
        private string _date;
        public string Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Date"));
                    }
                }
            }
        }
        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Content"));
                    }
                }
            }
        }

        #endregion


    }
}