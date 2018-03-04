using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ObservableCollection<SubOperacion> lstCtas = OperacionService.ListarSubOperaciones(idOperacion);
            idOpe = idOperacion;
            

            lsvCtas.ItemsSource = lstCtas;

            lsvCtas.GestureRecognizers.Clear();
            lsvCtas.GestureRecognizers.Add(new TapGestureRecognizer());

            //navBar.seleccionarBoton("1");
        }

        async void LsvCtas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            SubOperacion subope = e.Item as SubOperacion;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            Application.Current.Properties["suboperacionActual"] = subope;
            if (idOpe == "1")
            {
                //pago de servicios
                if(subope.Id == "0")
                {
                    await Navigation.PushAsync(new EmpresaView(), false);
                }
            }else if(idOpe == "3" )
            {
                await Navigation.PushAsync(new CtaCargoView(subope.Id, origenMisCuentas, ""), false);
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
