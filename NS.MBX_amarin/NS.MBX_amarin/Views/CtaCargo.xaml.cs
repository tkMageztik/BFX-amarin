using NS.MBX_amarin.Model;
using NS.MBX_amarin.View;
using NS.MBX_amarin.ViewModels;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.Views
{
    public partial class CtaCargo : ContentPage
    {
        public ObservableCollection<Cuenta> Items { get; set; }
        private string tipoTransf { get; set; }
        public bool origenMisCuentas;
        public string pageOrigen { get; set; }

        public CtaCargo()
        {
            InitializeComponent();
            
            this.tipoTransf = Application.Current.Properties["strTipoTransf"] as string;
            this.origenMisCuentas = (bool)Application.Current.Properties["strOrigenMisCuentas"];
            this.pageOrigen = Application.Current.Properties["strPageOrigen"] as string;

            //Items = new ObservableCollection<Cuenta>
            //{
            //    new Cuenta { NombreCta = "Cuenta Simple Soles", SaldoDisponible = 0.10M, Moneda = "S/" },
            //    new Cuenta { NombreCta = "Cuenta Simple Dólares", SaldoDisponible = 5.10M, Moneda = "$" },
            //    new Cuenta { NombreCta = "Cuenta Ahoros Soles", SaldoDisponible = 155.10M, Moneda = "S/" },
            //    new Cuenta { NombreCta = "Cuenta Ahorros Dólares", SaldoDisponible = 555.10M, Moneda = "$" }
            //};

            Title = "Cuenta Cargo";
            //lsvCtas.ItemSelected += LstvCtas_OnItemSelected;

            lsvCtas.ItemsSource = Application.Current.Properties["listaCuentas"] as ObservableCollection<Cuenta>;
            //navBar.seleccionarBoton("1");
        }

        //async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item == null)
        //        return;

        //    //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

        //    //if (origen == "Transferencia Ctas mismo banco")
        //    //{
        //    //    await Navigation.PushAsync(new CtaDestinoView(origen));
        //    //}

        //    await Navigation.PushAsync(new CtaDestinoView(tipoTransf, e.Item as Cuenta, origenMisCuentas));

        //    //Deselect Item
        //    ((ListView)sender).SelectedItem = null;
        //}

        async void LsvCtas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            if (pageOrigen == "PagoServicioEmpresaView")
            {
                Application.Current.Properties["ctaCargo"] = e.Item;
                NavigationParameters parametros = ((CtaCargoViewModel)BindingContext).ObtenerNavParametros();
                parametros.Add("CtaCargo", e.Item);
                await ((CtaCargoViewModel)BindingContext).Navegar(Constantes.pageConfPagoServicioEmpresa, parametros);
                //await Navigation.PushAsync(new ConfPagoServicioEmpresaView(), false);
            }
            else if (pageOrigen == Constantes.pageTipoTarjeta)
            {
                Application.Current.Properties["ctaCargo"] = e.Item;
                var navParameters = ((CtaCargoViewModel)BindingContext).ObtenerNavParametros();
                navParameters.Add("CtaCargo", e.Item);
                navParameters.Add("CodTipoTarjeta", Application.Current.Properties["CodTipoTarjeta"]);
                await ((CtaCargoViewModel)BindingContext).Navegar(Constantes.pageDatosPagoTarjeta, navParameters);
            }
            else if (pageOrigen == Constantes.pageRecargaCelular)
            {
                Application.Current.Properties["ctaCargo"] = e.Item;
                var navParameters = ((CtaCargoViewModel)BindingContext).ObtenerNavParametros();
                navParameters.Add("CtaCargo", e.Item);
                await ((CtaCargoViewModel)BindingContext).Navegar(Constantes.pageConfDatosPago, navParameters);
                //await Navigation.PushAsync(new ConfPagoServicioEmpresaView(), false);
            }
            else if (pageOrigen == Constantes.pageRecargaBim)
            {
                Application.Current.Properties["ctaCargo"] = e.Item;
                var navParameters = ((CtaCargoViewModel)BindingContext).ObtenerNavParametros();
                navParameters.Add("CtaCargo", e.Item);
                await ((CtaCargoViewModel)BindingContext).Navegar(Constantes.pageConfDatosPago, navParameters);
                //await Navigation.PushAsync(new ConfPagoServicioEmpresaView(), false);
            }
            else if (pageOrigen == Constantes.pageOperaciones)//operacion frecuente pago de tc
            {
                Application.Current.Properties["ctaCargo"] = e.Item;
                var navParameters = ((CtaCargoViewModel)BindingContext).ObtenerNavParametros();
                navParameters.Add("CtaCargo", e.Item);
                await ((CtaCargoViewModel)BindingContext).Navegar(Constantes.pageDatosPagoTarjeta, navParameters);
                //await Navigation.PushAsync(new ConfPagoServicioEmpresaView(), false);
            }
            else
            {
                Application.Current.Properties["destTipoTransf"] = tipoTransf;
                Application.Current.Properties["destCtaOrigen"] = e.Item;
                Application.Current.Properties["destorigenMisCuentas"] = origenMisCuentas;
                await ((CtaCargoViewModel)BindingContext).Navegar("CtaDestino");
                //await Navigation.PushAsync(new CtaDestinoView(tipoTransf, e.Item as Cuenta, origenMisCuentas), false);
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
