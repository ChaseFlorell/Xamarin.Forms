using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace Xamarin.Forms.Pages.Azure
{
	public class AzureEasyTableSource : AzureSource
	{
		public static readonly BindableProperty TableNameProperty =
			BindableProperty.Create(nameof(TableName), typeof(string), typeof(AzureEasyTableSource), null);

		public string TableName
		{
			get { return (string)GetValue(TableNameProperty); }
			set { SetValue(TableNameProperty, value); }
		}

		public override async Task<JToken> GetJson()
		{
			var mobileServiceClient = new MobileServiceClient(Uri);
			IMobileServiceTable table = mobileServiceClient.GetTable(TableName);
			JToken jobj = await GetFromCache();
			if (jobj != null)
				return jobj;
			jobj = await table.ReadAsync(string.Empty);
			await WriteToCache(jobj);
			return jobj;
		}

		Task<Stream> GetFileCacheStream()
		{
			IIsolatedStorageFile store = Device.PlatformServices.GetUserStoreForApplication();
			Task<Stream> file = store.OpenFileAsync("storedCode", FileMode.OpenOrCreate, FileAccess.ReadWrite);
			return file;
		}

		async Task<JToken> GetFromCache()
		{
			try
			{
				Stream file = await GetFileCacheStream();
				var writer = new StreamReader(file);
				return await writer.ReadToEndAsync();
			}
			catch (Exception)
			{
				return null;
			}
		}

		async Task WriteToCache(JToken token)
		{
			try
			{
				Stream file = await GetFileCacheStream();
				var writer = new StreamWriter(file);
				await writer.WriteAsync(token.ToString());
			}
			catch (Exception ex)
			{
			}
		}
	}
}