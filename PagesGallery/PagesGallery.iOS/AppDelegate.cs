﻿using System;
using System.IO;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Pages;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(PagesGallery.iOS.AppDelegate.SaveAndLoad))]
namespace PagesGallery.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Forms.Init();
			LoadApplication(new App());
			Xamarin.Forms.Pages.Azure.AzureDataSource.Init();
			return base.FinishedLaunching(app, options);
		}


		public class SaveAndLoad : ISaveAndLoad
		{
			public void SaveText(string filename, string text)
			{
				var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var filePath = Path.Combine(documentsPath, filename);
				System.IO.File.WriteAllText(filePath, text);
			}
			public string LoadText(string filename)
			{
				var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var filePath = Path.Combine(documentsPath, filename);
				try
				{
					return System.IO.File.ReadAllText(filePath);
				}
				catch (Exception ex)
				{
					return "";
				}

			}
		}
	}
}