using Arxius.Services.PCL;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public static class AuthDoNotSync
    {
        public static System.Tuple<string, string> Login()
        {
            return new System.Tuple<string, string>("user_1195", "pass");
        }
    }
}
