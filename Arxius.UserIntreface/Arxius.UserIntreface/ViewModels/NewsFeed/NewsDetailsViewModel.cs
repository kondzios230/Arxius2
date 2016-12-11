using Arxius.Services.PCL.Entities;
using System.ComponentModel;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class NewsDetailsViewModel : AbstractViewModel
    {
        public NewsDetailsViewModel(INavigation navi, News _news, Page page)
        {
            Title = _news.Title;
            Author = _news.Author;
            Date = _news.Date;
            Content = _news.Content;

            _page = page;
            Navigation = navi;
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
                    OnPropertyChanged("Title");
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
                    OnPropertyChanged("Author");
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
                    OnPropertyChanged("Date");
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
                    OnPropertyChanged("Content");
                }
            }
        }

        #endregion


    }
}