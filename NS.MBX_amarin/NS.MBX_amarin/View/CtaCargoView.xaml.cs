using NS.MBX_amarin.Model;
using System;
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
        private string origen { get; set; }
        public CtaCargoView(string origen)
        {
            InitializeComponent();
            this.origen = origen;

            Items = new ObservableCollection<Cuenta>
            {
                new Cuenta { NombreCta = "Cuenta Simple Soles", SaldoDisponible = 0.10M, Moneda = "S/" },
                new Cuenta { NombreCta = "Cuenta Simple Dólares", SaldoDisponible = 5.10M, Moneda = "$" },
                new Cuenta { NombreCta = "Cuenta Ahoros Soles", SaldoDisponible = 155.10M, Moneda = "S/" },
                new Cuenta { NombreCta = "Cuenta Ahorros Dólares", SaldoDisponible = 555.10M, Moneda = "$" }
            };

            Title = "Mis cuentas";
            //lsvCtas.ItemSelected += LstvCtas_OnItemSelected;

            MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            if (origen == "Transferencia Ctas mismo banco")
            {
                await Navigation.PushAsync(new CtaDestinoView(origen));
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
