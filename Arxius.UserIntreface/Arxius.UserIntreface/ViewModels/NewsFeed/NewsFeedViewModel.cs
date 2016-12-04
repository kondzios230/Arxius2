using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class NewsFeedViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        public NewsFeedViewModel(INavigation navi)
        {
            _navigation = navi;
            GetNewsPageAsync();
            ShowNews = new Command(ExecuteShowNews);
        }
        private async void GetNewsPageAsync()
        {
            var s = new UtilsService();
            NewsFeed = await s.GetFeedPage();

        }
        #region Bindable properties

        private List<News> _newsFeed;

        public List<News> NewsFeed
        {
            get { return _newsFeed; }
            set
            {
                if (_newsFeed != value)
                {
                    _newsFeed = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("NewsFeed"));
                      
                    }
                }
            }
        }
        private News _selectedNews;
        public News SelectedNews
        {
            get
            {
                return _selectedNews;
            }
            set
            {
                _selectedNews = value;

                if (_selectedNews == null)
                    return;

                ShowNews.Execute(SelectedNews);

                SelectedNews = null;
            }
        }

        #endregion
        public ICommand ShowNews { private set; get; }
        async void ExecuteShowNews()
        {
            await _navigation.PushAsync(new NewsDetailsPage(_navigation, SelectedNews));
        }
    }
}