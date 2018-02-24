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
        public string idOpe;
        public bool origenMisCuentas;

		public SubOperacionesView (string idOperacion, bool origenMisCuentas)
		{
			InitializeComponent ();
            this.origenMisCuentas = origenMisCuentas;
            List<SubOperacion> lstCtas = null;

            idOpe = idOperacion;

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
                    new SubOperacion { Id= "0", Nombre = "A otras cuenta Financiero" },
                    new SubOperacion { Id= "1", Nombre = "A otro banco" },
                    new SubOperacion { Id= "2", Nombre = "A cuenta propia" }
                };
            }

            lsvCtas.ItemsSource = lstCtas;

            lsvCtas.GestureRecognizers.Clear();
            lsvCtas.GestureRecognizers.Add(new TapGestureRecognizer());

            navBar.seleccionarBoton("1");
        }

        async void LsvCtas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            SubOperacion subope = e.Item as SubOperacion;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            if(idOpe == "3" )
            {
                await Navigation.PushAsync(new CtaCargoView(subope.Id, origenMisCuentas), false);
            }
            else
            {
                await Navigation.PushAsync(new SubOperacionesView(subope.Id, false), false);
            }

            

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
