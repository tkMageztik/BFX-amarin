using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConsultasView : ContentPage
	{
		public ConsultasView (string texto)
		{
			InitializeComponent ();
            lblTitulo.Text = texto;
		}

       
	}
}