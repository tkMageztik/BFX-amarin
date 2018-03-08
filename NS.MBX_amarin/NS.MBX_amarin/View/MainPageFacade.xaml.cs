using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPageFacade : ContentPage
	{
		public MainPageFacade ()
		{
			InitializeComponent ();
            App.Current.MainPage = new MainPage();
        }
	}
}