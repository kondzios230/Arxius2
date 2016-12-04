using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class NewsFeedViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        
        private IUtilsService uService;
        public NewsFeedViewModel(INavigation navi)
        {
            _navigation = navi;
            PageNumber = 1;
            uService = new UtilsService();
            GetNewsPageAsync();
            ShowNews = new Command(ExecuteShowNews);
            PreviousPage = new Command(ExecutePreviousPage,()=> PageNumber > 1);
            NextPage = new Command(ExecuteNextPage);
        }
        private async Task<bool> GetNewsPageAsync(int i=1)
        {             
            NewsFeed = await uService.GetFeedPage(i);
            return true;
        }
        #region Bindable properties

        private int pageNumber;

        public int PageNumber
        {
            get { return pageNumber; }
            set
            {
                if (pageNumber != value)
                {
                    pageNumber = value;
                    if(PreviousPage!=null)
                        ((Command)PreviousPage).ChangeCanExecute();
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PageNumber"));

                    }
                }
            }
        }
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
        public ICommand PreviousPage { private set; get; }
        async void ExecutePreviousPage()
        {
            PageNumber--;
            await GetNewsPageAsync(PageNumber);
        }
        public ICommand NextPage { private set; get; }
        async void ExecuteNextPage()
        {
            PageNumber++;
            await GetNewsPageAsync(PageNumber);
        }
    }
}