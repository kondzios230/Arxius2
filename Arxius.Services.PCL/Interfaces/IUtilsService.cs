using Arxius.CrossLayer.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Interfaces
{
    public interface IUtilsService
    {
        Task<List<News>> GetFeedPage(int pageNumber = 1, bool clean = false);
        Task<UserPage> GetUserPage(bool clean = false);
        void Login(string cookie);
        Task<List<Employee>> GetEmployees(bool clean = false);
        Task<Employee> GetEmployeeDetails(Employee employee, bool clean = false);
        Task<List<GenericGroupedCollection<string, string>>> GetImportantDates(bool clean = false);
        Task<bool> IsLoggedIn();
    }
}
