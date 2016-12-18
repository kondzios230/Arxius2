using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class MainViewModel :AbstractViewModel
    {
        public ICommand ShowPlan { private set; get; }
        public ICommand ShowUserProfile { private set; get; }
        public ICommand ShowNews { private set; get; }
        public ICommand ShowEmployeeList { private set; get; }
        public ICommand ShowCoursesList { private set; get; }
        public ICommand ShowDates { private set; get; }
        public MainViewModel(INavigation navi,Page page)
        {
            _page = page;
            Navigation = navi;
            ShowPlan = new Command(async () => await Navigation.PushAsync(new WeekSchedulePage(Navigation)));           
            ShowUserProfile = new Command(async () => await Navigation.PushAsync(new UserProfilePage(Navigation)));
            ShowNews = new Command(async () => await Navigation.PushAsync(new NewsFeedPage(Navigation)));
            ShowEmployeeList = new Command(async () => await Navigation.PushAsync(new EmployeeListPage(Navigation)));
            ShowCoursesList = new Command(async () => await Navigation.PushAsync(new CourseListPage(Navigation)));
            ShowDates = new Command(async () => await Navigation.PushAsync(new DatesPage(Navigation)));
        }

     

      
    }
}