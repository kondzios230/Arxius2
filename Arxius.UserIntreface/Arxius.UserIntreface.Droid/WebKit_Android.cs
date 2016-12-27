using System;
using Xamarin.Forms;
using WorkingWithFiles.Droid;
using System.IO;
using System.Threading.Tasks;
using Arxius.Services.PCL.Interfaces_and_mocks;
using Android.Content;

[assembly: Dependency(typeof(WebKit_Android))]

namespace WorkingWithFiles.Droid
{
    public class WebKit_Android : IWebKit
    {
        public void OpenPage(string adress)
        {
            Uri uri = Uri.parse("http://www.example.com");
            Intent intent = new Intent(Intent.ACTION_VIEW, uri);
            startActivity(intent);
        }
        
    }
}