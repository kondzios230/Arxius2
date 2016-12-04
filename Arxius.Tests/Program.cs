using System;
using Arxius.Services.PCL;

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
            bool testing = false;
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
                var x = await cService.GetAllCourses();
                var z = x.Find(c => c.Name == "Ekonomia międzynarodowa");
                 var zx = await cService.GetCourseWideDetails(z);
                //var aa = await cService.GetStudentsList(zx.Classes[0]);
                await cService.EnrollOrUnroll(zx.Classes[0]);
                await cService.EnrollOrUnroll(zx.Classes[0]);
                //  Console.Write(aa.Item1);
                //cService.Foo();
            }
            Console.Read();
        }
    }
}
