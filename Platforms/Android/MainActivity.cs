using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Firebase.Core.Platforms.Android;

namespace TechnicianApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Plugin.Firebase v4 için güncel başlatma kodu:
            // 2. parametre olarak artık Bundle değil, 
            // bulunduğumuz Activity'i döndüren bir fonksiyon '() => this' veriyoruz.
            CrossFirebase.Initialize(this, () => this);
        }
    }
}