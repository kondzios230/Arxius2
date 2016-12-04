using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Interfaces_and_mocks
{
    public interface IUtilsService
    {
        Task<List<News>> GetFeedPage(int pageNumber = 1);
        Task<UserPage> GetUserPage();
        Task<bool> Login(string login, string password);
        Task<List<Employee>> GetEmployees();
        void GetEmployeeDetails(string employeePage);
    }
}
