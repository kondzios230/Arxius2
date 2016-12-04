using Arxius.DataAccess.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using Arxius.Services.PCL.Parsers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arxius.Services.PCL
{
    public  class UtilsService :IUtilsService
    {
        public async Task<List<News>> GetFeedPage(int pageNumber=1)
        {
            var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/news/?page={1}/"));
            return UtilsParsers.GetFeedElementsContent(page);
        }

        public async Task<UserPage> GetUserPage()
        {
            var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/users/"));
            return UtilsParsers.GetUserPage(page);
        }

        public async Task<bool> Login(string login, string password)
        {
            return await HTMLUtils.Login(Properties.Resources.baseUri,login, password);
        }
        public async Task<List<Employee>> GetEmployees()
        {
            var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/users/employees/"));
            return UtilsParsers.GetEmployeesList(page);
        }
        public void GetEmployeeDetails(string employeePage)
        {
            var page = HTMLUtils.GetPage(employeePage);
        }
    }
}
