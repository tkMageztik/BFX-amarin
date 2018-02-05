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
    public partial class CuentasView : ContentPage
    {
        public CuentasView()
        {
            //List<string> lstprueba = new List<string>();
            //lstprueba.Add("Cuenta Simple Soles");
            //lstprueba.Add("Cuenta Simple Dólares");
            //lstprueba.Add("Cuenta Ahorros Soles");
            //lstprueba.Add("Cuenta Ahorros Dólares");

            InitializeComponent();
            List<Cuenta> lstCtas = new List<Cuenta>
            {
                new Cuenta { NombreCta = "Cuenta Simple Soles", SaldoDisponible = 0.10M, Moneda = "S/" },
                new Cuenta { NombreCta = "Cuenta Simple Dólares", SaldoDisponible = 5.10M, Moneda = "$" },
                new Cuenta { NombreCta = "Cuenta Ahoros Soles", SaldoDisponible = 155.10M, Moneda = "S/" },
                new Cuenta { NombreCta = "Cuenta Ahorros Dólares", SaldoDisponible = 555.10M, Moneda = "$" }
            };

            lsvCtas.ItemsSource = lstCtas;

            lsvCtas.GestureRecognizers.Clear();
            lsvCtas.GestureRecognizers.Add(new TapGestureRecognizer());
        }

        private void LstvCtas_OnItemSelected(object sender, EventArgs args)
        {
            DisplayAlert("TEST", "TEST2", "TEST3");
            Navigation.PushAsync(new ConsultasView(""));
        }


    }
}