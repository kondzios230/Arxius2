using System;
using Arxius.Services.PCL;
using Xamarin.Forms;
namespace Arxius.Tests
{
    class Program
    {

        static void Main(string[] args)
        {
            MainAsync();
            Console.Read();
        }

        static async void MainAsync()
        {
            bool testing = true;
            var cService = new CoursesService();
            var uService = new UtilsService();
            #region Tests
            if (testing)
            {
                var tests = new Tests();
                Console.WriteLine("Authentication: " + await tests.IsAuthenticatedSuccessfully());

                Console.WriteLine("Current semester courses number correct (2): " + await tests.IsCorrectNumberOfCoursesInCurrentSemesterReturned());
                Console.WriteLine("All courses number correct (29): " + await tests.IsCorrectNumberOfAllTimeCourses());
                Console.WriteLine("Newsfeed download succesfull: " + await tests.IsNewsfeedDownloadSuccesfull());
                Console.WriteLine("UserPage download succesfull: " + await tests.IsUserPageDownloadSuccesfull());
                Console.WriteLine("Employelist counts 142 " + await tests.IsEmployeeListDownloaded());
            }
            #endregion
            else
            {
                await AuthDoNotSync.Login();
                uService.GetImportantDates();
            }
            Console.Read();
        }
    }
}
