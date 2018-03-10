using NS.MBX_amarin.Model;
using NS.MBX_amarin.ViewModels;
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
    public partial class CtaDestino : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }
        private string origen { get; set; }
        public string tipoTransf;
        public bool origenMisCuentas;
        public Cuenta cuentaOrigen;

        public CtaDestino()
        {

            InitializeComponent();
            Title = "Cuenta Destino";
            
            this.tipoTransf = Application.Current.Properties["destTipoTransf"] as string;
            this.origenMisCuentas = (bool)Application.Current.Properties["destorigenMisCuentas"];
            this.cuentaOrigen = Application.Current.Properties["destCtaOrigen"] as Cuenta;

            //otras cuentas finan
            if (tipoTransf == "0")
            {
                layoutMisCuentas.IsVisible = false;
                layoutOtrasCuentasFinan.IsVisible = true;
                layoutOtrosBancos.IsVisible = false;

                lblCtaOri.Text = "Desde: " + cuentaOrigen.NombreCta;

                Dictionary<string, string> nameToCta = new Dictionary<string, string>
                {
                    { "Cuenta Simple Soles", "" },
                    { "Cuenta Simple Dólares", "" },
                    { "Cuenta Ahorros Soles", "" },
                    { "Cuenta Ahorros Dólares", "" }
                };

                foreach (string cuentaName in nameToCta.Keys)
                {
                    picTipoCuenta.Items.Add(cuentaName);
                }

                //otros bancos
            }
            else if (tipoTransf == "1")
            {
                layoutMisCuentas.IsVisible = false;
                layoutOtrasCuentasFinan.IsVisible = false;
                layoutOtrosBancos.IsVisible = true;

                Dictionary<string, string> nameToCta = new Dictionary<string, string>
                {
                    { "Soles", "" },
                    { "Dólares", "" }
                };

                foreach (string cuentaName in nameToCta.Keys)
                {
                    picMoneda.Items.Add(cuentaName);
                }

            }
            else if (tipoTransf == "2")
            {
                layoutMisCuentas.IsVisible = true;
                layoutOtrasCuentasFinan.IsVisible = false;
                layoutOtrosBancos.IsVisible = false;

                lsvCtas.ItemsSource = Application.Current.Properties["listaCuentas"] as ObservableCollection<Cuenta>;

                ObservableCollection<Cuenta> listaCuentas = Application.Current.Properties["listaCuentas"] as ObservableCollection<Cuenta>;
                ObservableCollection<Cuenta> listaNueva = new ObservableCollection<Cuenta>();

                foreach (Cuenta cta in listaCuentas)
                {
                    if (cta.idCta != cuentaOrigen.idCta)
                    {
                        listaNueva.Add(cta);
                    }
                }

                lsvCtas.ItemsSource = listaNueva;
            }
            Items = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
            };
            //navBar.seleccionarBoton("1");
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

            Application.Current.Properties["trxCuentaOrigen"] = cuentaOrigen;
            Application.Current.Properties["trxCuentaDestino"] = e.Item;
            Application.Current.Properties["trxOrigenMisCuentas"] = origenMisCuentas;
            Application.Current.Properties["trxTipoTransf"] = tipoTransf;
            await ((CtaDestinoViewModel)BindingContext).Navegar("Transferencia");
            //await Navigation.PushAsync(new TransferenciaView(cuentaOrigen, e.Item as Cuenta, origenMisCuentas, tipoTransf), false);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void BtnSgt_OnClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new SeleccionaCtaCargo("Transferencia Ctas mismo banco"));
            //ShowPopup();

            Cuenta cuentaDestino = new Cuenta();
            cuentaDestino.NombreCta = "otra cuenta Financiero";
            cuentaDestino.idMoneda = "PEN";
            cuentaDestino.Moneda = "S.";

            Application.Current.Properties["trxCuentaOrigen"] = cuentaOrigen;
            Application.Current.Properties["trxCuentaDestino"] = cuentaDestino;
            Application.Current.Properties["trxOrigenMisCuentas"] = origenMisCuentas;
            Application.Current.Properties["trxTipoTransf"] = tipoTransf;
            await ((CtaDestinoViewModel)BindingContext).Navegar("Transferencia");

            //await Navigation.PushAsync(new TransferenciaView(cuentaOrigen, cuentaDestino, origenMisCuentas, tipoTransf), false);


        }

        private async void BtnTransfOtroBanco_OnClicked(object sender, EventArgs args)
        {

            Cuenta cuentaDestino = new Cuenta();
            cuentaDestino.NombreCta = "otros bancos";
            cuentaDestino.idMoneda = "PEN";
            cuentaDestino.Moneda = "S.";

            Application.Current.Properties["trxCuentaOrigen"] = cuentaOrigen;
            Application.Current.Properties["trxCuentaDestino"] = cuentaDestino;
            Application.Current.Properties["trxOrigenMisCuentas"] = origenMisCuentas;
            Application.Current.Properties["trxTipoTransf"] = tipoTransf;
            await ((CtaDestinoViewModel)BindingContext).Navegar("Transferencia");

           // await Navigation.PushAsync(new TransferenciaView(cuentaOrigen, cuentaDestino, origenMisCuentas, tipoTransf), false);

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
