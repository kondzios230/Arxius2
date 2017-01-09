using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class NewsFeedViewModel : AbstractViewModel
    {
        public ICommand ShowNews { private set; get; }
        public ICommand PreviousPage { private set; get; }
        public ICommand NextPage { private set; get; }
        public NewsFeedViewModel(INavigation navi,Page page)
        {
            _page = page;
            Navigation = navi;
            PageNumber = 1;
            uService = new UtilsService();
            GetNewsPageAsync();
            ShowNews = new Command(async () => await Navigation.PushAsync(new NewsDetailsPage(Navigation, SelectedNews)));
            PreviousPage = new Command(async ()=>{ PageNumber--; await GetNewsPageAsync(PageNumber); },()=> PageNumber > 1);
            NextPage = new Command(async ()=>{ PageNumber++; await GetNewsPageAsync(PageNumber); },()=> PageNumber < 21);
            Refresh = new Command(async () => await GetNewsPageAsync(1, true));
        }
        private async Task<bool> GetNewsPageAsync(int i=1,bool clear = false)
        {
            try
            {
                IsAIRunning = true;
                NewsFeed = await uService.GetFeedPage(i,clear);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            IsAIRunning = false;
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
                    if (NextPage != null)
                        ((Command)NextPage).ChangeCanExecute();
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
        
    }
}