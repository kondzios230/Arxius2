using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class MainViewModel
    {
      
        private INavigation _navigation;
        public MainViewModel(INavigation navi)
        {
            _navigation = navi;
            ShowPlan = new Command(ExecuteShowPlan);
           
            ShowUserProfile = new Command(ExecuteShowUserProfile);
            ShowNews = new Command(ExecuteShowNews);
            ShowEmployeeList = new Command(ExecuteShowEmployeeList);
            ShowCoursesList = new Command(ExecuteShowCoursesList);
        }

     

        public ICommand ShowPlan { private set; get; }
        async void ExecuteShowPlan()
        {
            await _navigation.PushAsync(new SchedulePage(_navigation));
        }

        
        public ICommand ShowUserProfile { private set; get; }
        async void ExecuteShowUserProfile()
        {
            await _navigation.PushAsync(new UserProfilePage(_navigation));
        }
        public ICommand ShowNews { private set; get; }
        async void ExecuteShowNews()
        {
            await _navigation.PushAsync(new NewsFeedPage(_navigation));
        }
        public ICommand ShowEmployeeList { private set; get; }
        async void ExecuteShowEmployeeList()
        {
            await _navigation.PushAsync(new EmployeeListPage(_navigation));
        }
        public ICommand ShowCoursesList { private set; get; }
        async void ExecuteShowCoursesList()
        {
            await _navigation.PushAsync(new CourseListPage(_navigation));
        }
    }
}