using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class WebViewPage : ContentPage
    {
        public WebViewPage(string adress)
        {
            var browser = new WebView();

            browser.Source =adress;

            Content = browser;
        }
        
    }
}
