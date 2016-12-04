using Arxius.DataAccess;
using Arxius.DataAccess.PCL;
using Arxius.Services;
using Arxius.Services.PCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Tests
{
    public class Tests
    {
        public async Task<bool> IsAuthenticatedSuccessfully()
        {
            return await AuthDoNotSync.Login();
        }
        public async Task<bool> IsCorrectNumberOfCoursesInCurrentSemesterReturned()
        {
            var sService = new CoursesService();
            var x = await sService.GetUserPlanForCurrentSemester();
            return (x.Count == 2);
        }
        public async Task<bool> IsCorrectNumberOfAllTimeCourses()
        {
            var sService = new CoursesService();
            var x = await sService.GetAllUserCourses();
            return (x.Count == 29);
        }
        public async Task<bool> IsNewsfeedDownloadSuccesfull()
        {
            var uService = new UtilsService();
            var x = await uService.GetFeedPage(2);
            return (!x.Any(n=>n.Content==null));
        }
        public async Task<bool> IsEmployeeListDownloaded()
        {
            var uService = new UtilsService();
            var x = await uService.GetEmployees();
            return (x.Count ==142);
        }
        public async Task<bool> IsUserPageDownloadSuccesfull()
        {
            var uService = new UtilsService();
            var x = await uService.GetUserPage();
            int ct = 1;
            foreach(var i in x.RegistrationTimes)
            {
                if (ct == 1 && i.Count != 13)
                    return false;
                if (ct == 2 && i.Count != 42)
                    return false;
                ct++;
            }
            return true;
        }
    }
}
