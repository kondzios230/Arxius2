using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Interfaces_and_mocks
{
    public interface IUtilsService
    {
        Task<List<News>> GetFeedPage(int pageNumber = 1, bool clean = false);
        Task<UserPage> GetUserPage(bool clean = false);
        Task<bool> Login(string login, string password);
        void Login(string cookie);
        Task<List<Employee>> GetEmployees(bool clean = false);
        Task<Employee> GetEmployeeDetails(Employee employee, bool clean = false);
        Task<List<StringGroup>> GetImportantDates(bool clean = false);
        Task<bool> IsLoggedIn();
    }
}
