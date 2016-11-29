
using Arxius.Services.PCL;
using System.Threading.Tasks;

namespace Arxius.Tests
{
    public static class AuthDoNotSync
    {
        public static async Task<bool> Login()
        {
            var x = new UtilsService();
            //return await x.Login("user_1195", "pass");
            return await x.Login("user_1000", "pass");
        }
    }
}
