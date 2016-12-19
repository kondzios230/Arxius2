using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Arxius.UserIntreface.Droid
{
    [Activity(Label = "Arxius", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            if (Convert.ToInt32(global::Android.OS.Build.VERSION.Sdk) > 20)
                base.SetTheme(global::Android.Resource.Style.ThemeMaterialLight);
            else
                base.SetTheme(global::Android.Resource.Style.ThemeHoloLight);
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        } 
    }
}

