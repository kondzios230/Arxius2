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
    class NewsFeedViewModel : AbstractViewModel
    {
        private IUtilsService uService;
        public NewsFeedViewModel(INavigation navi,Page page)
        {
            _page = page;
            Navigation = navi;
            PageNumber = 1;
            uService = new UtilsService();
            GetNewsPageAsync();
            ShowNews = new Command(ExecuteShowNews);
            PreviousPage = new Command(ExecutePreviousPage,()=> PageNumber > 1);
            Refresh = new Command(ExecuteRefresh);
            NextPage = new Command(ExecuteNextPage);
        }
        private async Task<bool> GetNewsPageAsync(int i=1)
        {
            try
            {
                NewsFeed = await uService.GetFeedPage(i);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
           
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
                    OnPropertyChanged("PageNumber");
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
                    OnPropertyChanged("NewsFeed");
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
            await Navigation.PushAsync(new NewsDetailsPage(Navigation, SelectedNews));
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
        public ICommand Refresh { private set; get; }
        async void ExecuteRefresh()
        {
            try
            {
                (_page as NewsFeedPage).SetRefreshImage("refresh2.jpg");
                NewsFeed = await uService.GetFeedPage(1, true);
                PageNumber = 1;
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }            
            (_page as NewsFeedPage).SetRefreshImage("refresh.jpg");            
        }
    }
}