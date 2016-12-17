using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class UserProfileViewModel : AbstractViewModel
    {
        public UserProfileViewModel(INavigation navi,Page page)
        {
            _page = page;
            Navigation = navi;
            GetUserProfileAsync();
            ShowProfile = new Command(ExecuteShowProfile);
            Refresh = new Command(ExecuteRefresh);
        }
        private async void GetUserProfileAsync()
        {
            try
            {
                var s = new UtilsService();
                UserPage = await s.GetUserPage();
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
           

        }
        #region Bindable properties

        private UserPage _userPage;

        public UserPage UserPage
        {
            get { return _userPage; }
            set
            {
                if (_userPage != value)
                {
                    _userPage = value;
                    OnPropertyChanged("UserPage");
                    OnPropertyChanged("Ects");
                    OnPropertyChanged("LimitRemovalTime");
                    OnPropertyChanged("EndTime");
                    OnPropertyChanged("RegistrationTimes");
                }
            }
        }
        public int Ects
        {
            set
            {
                UserPage.Ects = value;
                OnPropertyChanged("Ects");
            }
            get
            {
                if (UserPage == null) return 0;
                return UserPage.Ects;
            }
        }
        public string LimitRemovalTime 
        {
            set
            {
                UserPage.LimitRemovalTime = value;
                OnPropertyChanged("LimitRemovalTime");
             }
            get
            {
                if (UserPage == null) return "";
                return UserPage.LimitRemovalTime;
            }
        }
        public string EndTime
        {
            set
            {
                UserPage.EndTime = value;
                OnPropertyChanged("EndTime");
            }
            get
            {
                if (UserPage == null) return "";
                return UserPage.EndTime;
            }
        }
        public List<RegistrationTimesCollection> RegistrationTimes
        {
            set
            {
                UserPage.RegistrationTimes = value;
                OnPropertyChanged("RegistrationTimes");
            }
            get
            {
                if (UserPage == null) return new List<RegistrationTimesCollection>();
                return UserPage.RegistrationTimes;
            }
        }

        #endregion
        public ICommand ShowProfile { private set; get; }
        async void ExecuteShowProfile()
        {
            await Navigation.PushAsync(new EctsPage(Navigation));
        }
        public ICommand Refresh { private set; get; }
        async void ExecuteRefresh()
        {
            try
            {
                (_page as UserProfilePage).SetRefreshImage("refresh2.jpg");
                var s = new UtilsService();
                UserPage = await s.GetUserPage(true);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            (_page as UserProfilePage).SetRefreshImage("refresh.jpg");
        }
    }
}