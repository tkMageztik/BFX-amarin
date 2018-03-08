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
	public partial class NavBar : ContentView
	{

		public NavBar ()
		{
			InitializeComponent ();
		}

        public async void NavegarMisCtas(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CuentasView(), false);
        }

        public async void NavegarOperaciones(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new OperacionesView(), false);
        }

        public void seleccionarBoton(string idBoton)
        {
            if(idBoton == "0")
            {
                btnMisCtas.BackgroundColor = Color.Yellow;
            }else if (idBoton == "1")
            {
                btnOpe.BackgroundColor = Color.Yellow;
            }
        }
    }

    
}