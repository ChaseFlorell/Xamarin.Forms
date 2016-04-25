using System;

using Xamarin.Forms;

namespace Xamarin.Forms.Pages
{
	public interface ISaveAndLoad
	{
		void SaveText(string filename, string text);
		string LoadText(string filename);
	}
}


