using Arxius.DataAccess.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using Arxius.Services.PCL.Parsers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arxius.Services.PCL
{
    public class UtilsService : IUtilsService
    {
        public async Task<List<News>> GetFeedPage(int pageNumber = 1, bool clean = false)
        {
            return await Cache.Get(new { a = "GetFeedPage", p = pageNumber }, async () =>
            {
                var s1 = string.Format(Properties.Resources.baseUri, "/news/?page={0}");
                var page = await HTMLUtils.GetPage(string.Format(s1, pageNumber));
                return UtilsParsers.GetFeedElementsContent(page);
            }, clean);
        }
        public async Task<UserPage> GetUserPage(bool clean = false)
        {
            return await Cache.Get("GetUserPage", async () =>
            {
                var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/users/"));
                return UtilsParsers.GetUserPage(page);
            }, clean);
        }
        public async Task<bool> Login(string login, string password)
        {
            try
            {
                await HTMLUtils.Login(Properties.Resources.baseUri, login, password);
                var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/users/"));
                return page.Contains("user_is_authenticated = true");
            }
            catch (ArxiusDataException e)
            {
                throw new ArxiusException(e);
            }
        }
        public async Task<List<Employee>> GetEmployees(bool clean = false)
        {
            return await Cache.Get("GetEmployees", async () =>
            {
                var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/users/employees/"));
                return UtilsParsers.GetEmployeesList(page);
            }, clean);
        }
        public void GetEmployeeDetails(string employeePage, bool clean = false)
        {
            var page = HTMLUtils.GetPage(employeePage);
        }
        public async void GetImportantDates(bool clean = false)
        {
            var page = (await HTMLUtils.GetPage(@"http://ii.uni.wroc.pl/dla-studenta/kalendarz")).Replace("\n", string.Empty);
            UtilsParsers.GetImportantDates(page);


        }
    }
}
