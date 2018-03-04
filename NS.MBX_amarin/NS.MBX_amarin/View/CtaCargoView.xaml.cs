using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CtaCargoView : ContentPage
    {
        public ObservableCollection<Cuenta> Items { get; set; }
        private string tipoTransf { get; set; }
        public bool origenMisCuentas;
        public string pageOrigen { get; set; }

        public CtaCargoView(string tipoTransf, bool origenMisCuentas, string pageOrigen)
        {
            InitializeComponent();
            this.tipoTransf = tipoTransf;
            this.origenMisCuentas = origenMisCuentas;
            this.pageOrigen = pageOrigen;

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
            if(pageOrigen == "PagoServicioEmpresaView")
            {
                Application.Current.Properties["ctaCargo"] = e.Item;
                await Navigation.PushAsync(new ConfPagoServicioEmpresaView(), false);
            }
            else
            {
                await Navigation.PushAsync(new CtaDestinoView(tipoTransf, e.Item as Cuenta, origenMisCuentas), false);
            }
            

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
