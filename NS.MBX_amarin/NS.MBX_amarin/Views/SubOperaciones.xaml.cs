using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using NS.MBX_amarin.View;
using NS.MBX_amarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.Views
{
    public partial class SubOperaciones : ContentPage
    {
        public string idOpe;
        public bool origenMisCuentas;

        public SubOperaciones()
        {
            InitializeComponent();
            string idOperacion = Application.Current.Properties["opeId"] as string;
            bool origenMisCuentas = (bool)Application.Current.Properties["origenMisCuentas"];

            this.origenMisCuentas = origenMisCuentas;
            ObservableCollection<SubOperacion> lstCtas = ((SubOperacionesViewModel)BindingContext).ObtenerOperacionService().ListarSubOperaciones(idOperacion);
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
                if (subope.Id == "0")
                {
                    await ((SubOperacionesViewModel)BindingContext).NavegarEmpresa();
                    //await Navigation.PushAsync(new EmpresaView(), false);
                }
                else if (subope.Id == "1") //pago a institucion o empresa
                {
                    await ((SubOperacionesViewModel)BindingContext).NavegarBuscadorEmpresa();
                    //await Navigation.PushAsync(new BuscadorEmpresaView(), false);
                }
                else if (subope.Id == "2") //pago de tarjetas
                {
                    await ((SubOperacionesViewModel)BindingContext).NavegarTipoTarjeta();
                }
            }
            else if (idOpe == "2")//recargas
            {
                //recarga de celular
                if (subope.Id == "0")
                {
                    await ((SubOperacionesViewModel)BindingContext).Navegar("RecargaCelular");
                    //await Navigation.PushAsync(new EmpresaView(), false);
                }else if (subope.Id == "1")
                {
                    await ((SubOperacionesViewModel)BindingContext).Navegar("RecargaBim");
                    //await Navigation.PushAsync(new EmpresaView(), false);
                }
            }

            else if (idOpe == "3")
            {
                Application.Current.Properties["strTipoTransf"] = subope.Id;
                Application.Current.Properties["strOrigenMisCuentas"] = origenMisCuentas;
                Application.Current.Properties["strPageOrigen"] = "";
                await ((SubOperacionesViewModel)BindingContext).Navegar("CtaCargo");
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
