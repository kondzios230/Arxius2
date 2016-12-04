using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class UserProfileViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        public UserProfileViewModel(INavigation navi)
        {
            _navigation = navi;
            GetUserProfileAsync();
            ShowProfile = new Command(ExecuteShowProfile);
        }
        private async void GetUserProfileAsync()
        {
            var s = new UtilsService();
            UserPage = await s.GetUserPage();

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

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("UserPage"));
                        PropertyChanged(this, new PropertyChangedEventArgs("Ects"));
                        PropertyChanged(this, new PropertyChangedEventArgs("LimitRemovalTime"));
                        PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                        PropertyChanged(this, new PropertyChangedEventArgs("RegistrationTimes"));
                    }
                }
            }
        }
        public int Ects
        {
            set
            {
                UserPage.Ects = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Ects"));
                }
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

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LimitRemovalTime"));
                }
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

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                }
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

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RegistrationTimes"));
                }
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
            await _navigation.PushAsync(new ProfilePage(_navigation));
        }
    }
}