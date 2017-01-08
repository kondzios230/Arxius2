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
        public ICommand ShowProfile { private set; get; }       
        public UserProfileViewModel(INavigation navi,Page page)
        {
            uService = new UtilsService();
            _page = page;
            Navigation = navi;
            GetUserProfileAsync();
            ShowProfile = new Command(async () => await Navigation.PushAsync(new EctsPage(Navigation)));
            Refresh = new Command(() => GetUserProfileAsync(true));
        }
        private async void GetUserProfileAsync(bool clear = false)
        {
            try
            {
                IsAIRunning = true;
                UserPage = await uService.GetUserPage(clear);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            IsAIRunning = false;
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
        public List<CourseGroupedCollection> RegistrationTimes
        {
            set
            {
                UserPage.RegistrationTimes = value;
                OnPropertyChanged("RegistrationTimes");
            }
            get
            {
                if (UserPage == null) return new List<CourseGroupedCollection>();
                return UserPage.RegistrationTimes;
            }
        }

        #endregion
    
        
    }
}