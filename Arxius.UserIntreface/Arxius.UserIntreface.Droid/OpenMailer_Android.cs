using Xamarin.Forms;
using WorkingWithFiles.Droid;
using Arxius.Services.PCL.Interfaces;
using Android.Content;

[assembly: Dependency(typeof(OpenMailer_Android))]

namespace WorkingWithFiles.Droid
{
    public class OpenMailer_Android : IOpenMailer
    {

        public void SendMail(string adress)
        {
            var email = new Intent(Intent.ActionSend);
            email.PutExtra(Intent.ExtraEmail, new string[] { adress});
            email.SetType("message/rfc822");
            Forms.Context.StartActivity(email);
        }
    }
}