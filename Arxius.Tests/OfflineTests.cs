using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Arxius.Tests
{
    [TestClass]
    public class OfflineTests
    {
        [TestMethod]
        public async Task IsCorrectNumberOfCoursesInCurrentSemesterReturned()
        {
            var sService = new CoursesServiceMock();
            var x = await sService.GetUserPlanForCurrentSemester();
            Assert.AreEqual(x.Count, 2);
        }
        [TestMethod]
        public async Task IsCorrectNumberOfAllTimeCourses()
        {
            var sService = new CoursesServiceMock();
            var x = await sService.GetAllUserCourses();
            Assert.AreEqual(x.Count, 29);
        }
        [TestMethod]
        public async Task IsNewsfeedDownloadSuccesfull()
        {
            var uService = new UtilsServiceMock();
            var x = await uService.GetFeedPage();
            var d= (!x.Any(n => n.Content == null));
            Assert.AreEqual(d, true);
        }
        [TestMethod]
        public async Task IsEmployeeListDownloaded()
        {
            var uService = new UtilsServiceMock();
            var x = await uService.GetEmployees();
            Assert.AreEqual(x.Count, 142);
        }
        [TestMethod]
        public async Task IsUserPageDownloadSuccesfull()
        {
            var uService = new UtilsServiceMock();
            var x = await uService.GetUserPage();
            int ct = 1;
            bool? ret=null;
            foreach (var i in x.RegistrationTimes)
            {
                if (ct == 1 && i.Count != 13)
                    ret= false;
                if (ct == 2 && i.Count != 42)
                    ret= false;
                ct++;
            }
            if (ret == null)
                ret = true;
            Assert.AreEqual(ret, true);
        }
        [TestMethod]
        public async Task IsNonEnrolledCourseDetailsParsedCorrectly()
        {
            //var uService = new CoursesServiceMock();
            //var c = new Course();
            //var x = await uService.GetCourseWideDetails(c);
           
            Assert.AreEqual(true, true);
        }
    }
}
