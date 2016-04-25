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
			if (jobj != null && !string.IsNullOrEmpty(jobj.ToString()))
				return jobj;
			jobj = await table.ReadAsync(string.Empty);
			await WriteToCache(jobj);
			return jobj;
		}

		Task<Stream> GetFileCacheStream()
		{
			IIsolatedStorageFile store = Device.PlatformServices.GetUserStoreForApplication();
			Task<Stream> file = store.OpenFileAsync(Path.Combine("storedCode" + TableName + ".json"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
			return file;
		}

		async Task<JToken> GetFromCache()
		{
			try
			{
				string result;
				using (Stream file = await GetFileCacheStream())
				using (var writer = new StreamReader(file))
				{
					result = await writer.ReadToEndAsync();
				}
				return string.IsNullOrEmpty(result) ? null : JToken.Parse(result);
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		async Task WriteToCache(JToken token)
		{
			try
			{
				using (var file = await GetFileCacheStream())
				using (var writer = new StreamWriter(file))
				{
					await writer.WriteAsync(token.ToString());
					writer.Flush();
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}
	}
}