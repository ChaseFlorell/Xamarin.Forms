using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System.IO;
​
namespace Xamarin.Forms.Pages.Azure
{
	public class AzureEasyTableSource : AzureSource
	{
		public static readonly BindableProperty TableNameProperty =
			BindableProperty.Create(nameof(TableName), typeof(string), typeof(AzureEasyTableSource), null);
​
		public string TableName
		{
			get
			{
				return (string)GetValue(TableNameProperty);
			}
			set
			{
				SetValue(TableNameProperty, value);
			}
		}
​
		public override async Task<JToken> GetJson()
		{
			var mobileServiceClient = new MobileServiceClient(Uri);
			var table = mobileServiceClient.GetTable(TableName);
			var jobj = await GetFromCache();
			if (jobj != null)
				return jobj;
			jobj = await table.ReadAsync(string.Empty);
			await WriteToCache(jobj);
			return jobj;
		}
​
		async Task<JToken> GetFromCache()
		{
			try
			{
				var file = await GetFileCacheStream();
				var writer = new StreamReader(file);
				return await writer.ReadToEndAsync();
			}
			catch (Exception)
			{
				return null;
			}
		}
​
		async Task WriteToCache(JToken token)
		{
			try
			{
​
				var file = await GetFileCacheStream();
				var writer = new StreamWriter(file);
				await writer.WriteAsync(token.ToString());
			}
			catch (Exception ex)
			{
​
			}
		}
​
		Task<System.IO.Stream> GetFileCacheStream()
		{
			var store = Device.PlatformServices.GetUserStoreForApplication();
			var file = store.OpenFileAsync("storedCode", FileMode.OpenOrCreate, FileAccess.ReadWrite);
			return file;
		}
​
	}
}