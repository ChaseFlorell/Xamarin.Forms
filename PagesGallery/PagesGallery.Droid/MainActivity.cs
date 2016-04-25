using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using PagesGallery.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Pages;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace PagesGallery.Droid
{
	[Activity(Label = "PagesGallery", Theme = "@style/MyTheme", Icon = "@drawable/icon", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			ToolbarResource = Resource.Layout.Toolbar;
			TabLayoutResource = Resource.Layout.Tabbar;

			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			base.OnCreate(bundle);

			Forms.Init(this, bundle);
			LoadApplication(new App());
		}

	}

	public class SaveAndLoad : ISaveAndLoad
	{
		public void SaveText(string filename, string text)
		{
			var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(documentsPath, filename);
			System.IO.File.WriteAllText(filePath, text);
		}
		public string LoadText(string filename)
		{
			var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(documentsPath, filename);
			return System.IO.File.ReadAllText(filePath);
		}
	}
}