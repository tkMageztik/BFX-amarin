using NS.MBX_amarin.Model;
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
    public partial class Cuentas : ContentPage
    {
        public Cuentas()
        {
            //List<string> lstprueba = new List<string>();
            //lstprueba.Add("Cuenta Simple Soles");
            //lstprueba.Add("Cuenta Simple Dólares");
            //lstprueba.Add("Cuenta Ahorros Soles");
            //lstprueba.Add("Cuenta Ahorros Dólares");

            InitializeComponent();
            Title = "Mis cuentas";
            //lsvCtas.ItemSelected += LstvCtas_OnItemSelected;
            //List<Cuenta> lstCtas = new List<Cuenta>
            //{
            //    new Cuenta { NombreCta = "Cuenta Simple Soles", SaldoDisponible = 0.10M, Moneda = "S/" },
            //    new Cuenta { NombreCta = "Cuenta Simple Dólares", SaldoDisponible = 5.10M, Moneda = "$" },
            //    new Cuenta { NombreCta = "Cuenta Ahoros Soles", SaldoDisponible = 155.10M, Moneda = "S/" },
            //    new Cuenta { NombreCta = "Cuenta Ahorros Dólares", SaldoDisponible = 555.10M, Moneda = "$" }
            //};

            lsvCtas.ItemsSource = Application.Current.Properties["listaCuentas"] as ObservableCollection<Cuenta>;

            //lsvCtas.GestureRecognizers.Clear();
            //lsvCtas.GestureRecognizers.Add(new TapGestureRecognizer());
            //navBar.seleccionarBoton("0");
        }

        //private void LstvCtas_OnItemSelected(object sender, EventArgs args)
        //{
        //    //DisplayAlert("TEST", "TEST2", "TEST3");

        //    Navigation.PushAsync(new ConsultasView(""), false);

        //    //lsvCtas.ItemSelected -= LstvCtas_OnItemSelected;
        //    //((ListView)sender).SelectedItem = null;
        //    //lsvCtas.ItemSelected += LstvCtas_OnItemSelected;
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            lsvCtas.ItemsSource = null;

            Device.BeginInvokeOnMainThread(() =>
            {
                lsvCtas.ItemsSource = Application.Current.Properties["listaCuentas"] as ObservableCollection<Cuenta>;

            });

        }

        async void LsvCtas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            Application.Current.Properties["objCuenta"] = e.Item;
            await ((CuentasViewModel)BindingContext).Navegar("Consultas");
            //await Navigation.PushAsync(new ConsultasView(e.Item as Cuenta), false);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
