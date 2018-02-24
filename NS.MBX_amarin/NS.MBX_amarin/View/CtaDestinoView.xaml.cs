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
    public partial class CtaDestinoView : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }
        private string origen { get; set; }
        public string tipoTransf;
        public bool origenMisCuentas;
        public Cuenta cuentaOrigen;

        public CtaDestinoView(string tipoTransf, Cuenta cuentaOrigen, bool origenMisCuentas)
        {
            
            InitializeComponent();
            Title = "Cuenta Destino";
            this.tipoTransf = tipoTransf;
            this.origenMisCuentas = origenMisCuentas;
            this.cuentaOrigen = cuentaOrigen;

            //otras cuentas
            if (tipoTransf == "1")
            {
                layoutMisCuentas.IsVisible = false;
                layoutOtrasCuentas.IsVisible = true;
                //entre mis cuentas
            }else if (tipoTransf == "2")
            {
                layoutMisCuentas.IsVisible = true;
                layoutOtrasCuentas.IsVisible = false;
                lsvCtas.ItemsSource = Application.Current.Properties["listaCuentas"] as List<Cuenta>;
            }
                Items = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
            };
            navBar.seleccionarBoton("1");
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        async void LsvCtas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            await Navigation.PushAsync(new TransferenciaView(cuentaOrigen, e.Item as Cuenta, origenMisCuentas, tipoTransf), false);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void BtnSgt_OnClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new SeleccionaCtaCargo("Transferencia Ctas mismo banco"));
            //ShowPopup();

            await DisplayAlert("Transferencia Exitosa", "Transferido correctamente", "OK");


            //var page = new NavigationPage(new CuentasView());

            //await Navigation.PushAsync(page);

            RemoveBeforeDestination(typeof(CuentasView));

            //await Navigation.PushAsync(new CuentasView());
            //await Navigation.PushAsync(page);
            await Navigation.PopAsync();

        }

        public void RemoveBeforeDestination(Type DestinationPage)
        {
            int LeastFoundIndex = 0;
            int PagesToRemove = 0;

            for (int index = Navigation.NavigationStack.Count - 2; index > 0; index--)
            {
                if (Navigation.NavigationStack[index].GetType().Equals(DestinationPage))
                {
                    break;
                }
                else
                {
                    LeastFoundIndex = index;
                    PagesToRemove++;
                }
            }

            for (int index = 0; index < PagesToRemove; index++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[LeastFoundIndex]);
            }

        }
    }
}
