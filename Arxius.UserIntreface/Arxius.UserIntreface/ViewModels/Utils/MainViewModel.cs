using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class MainViewModel :AbstractViewModel
    {
      
        public MainViewModel(INavigation navi,Page page)
        {
            _page = page;
            Navigation = navi;
            ShowPlan = new Command(ExecuteShowPlan);
           
            ShowUserProfile = new Command(ExecuteShowUserProfile);
            ShowNews = new Command(ExecuteShowNews);
            ShowEmployeeList = new Command(ExecuteShowEmployeeList);
            ShowCoursesList = new Command(ExecuteShowCoursesList);
        }

     

        public ICommand ShowPlan { private set; get; }
        async void ExecuteShowPlan()
        { 
            await Navigation.PushAsync(new WeekSchedulePage(Navigation));
        }


        public ICommand ShowUserProfile { private set; get; }
        async void ExecuteShowUserProfile()
        {
            await Navigation.PushAsync(new UserProfilePage(Navigation));
        }
        public ICommand ShowNews { private set; get; }
        async void ExecuteShowNews()
        {
            await Navigation.PushAsync(new NewsFeedPage(Navigation));
        }
        public ICommand ShowEmployeeList { private set; get; }
        async void ExecuteShowEmployeeList()
        {
            await Navigation.PushAsync(new EmployeeListPage(Navigation));
        }
        public ICommand ShowCoursesList { private set; get; }
        async void ExecuteShowCoursesList()
        {
            await Navigation.PushAsync(new CourseListPage(Navigation));
        }
    }
}