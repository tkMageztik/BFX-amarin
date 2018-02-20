using NS.MBX_amarin.Model;
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
	public partial class SubOperacionesView : ContentPage
	{
		public SubOperacionesView (string idOperacion)
		{
			InitializeComponent ();

            List<SubOperacion> lstCtas = null;

            if (idOperacion == "1")
            {
                Title = "Pagos";

                lstCtas = new List<SubOperacion>
                {
                    new SubOperacion { Id= "1", Nombre = "Pago de servicios" },
                    new SubOperacion { Id= "2", Nombre = "Pago de alquiler" },
                    new SubOperacion { Id= "3", Nombre = "Pago de cuentas" }
                };
            }else if (idOperacion == "2")
            {
                Title = "Recargas";

                lstCtas = new List<SubOperacion>
                {
                    new SubOperacion { Id= "1", Nombre = "Recarga Claro" },
                    new SubOperacion { Id= "2", Nombre = "Recarga Movistar" },
                    new SubOperacion { Id= "3", Nombre = "Recarga Entel" }
                };
            }
            else if (idOperacion == "3")
            {
                Title = "Transferencias";

                lstCtas = new List<SubOperacion>
                {
                    new SubOperacion { Id= "1", Nombre = "A otras cuentas" },
                    new SubOperacion { Id= "2", Nombre = "Entre mis cuentas" }
                };
            }

            lsvCtas.ItemsSource = lstCtas;

            lsvCtas.GestureRecognizers.Clear();
            lsvCtas.GestureRecognizers.Add(new TapGestureRecognizer());
        }
	}
}