using NS.MBX_amarin.Model;
using NS.MBX_amarin.ViewModels;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.Views
{
    public partial class Consultas : ContentPage
    {
        public Cuenta cuenta;

        public Consultas()
        {
            InitializeComponent();

            Cuenta cta = Application.Current.Properties["objCuenta"] as Cuenta;
            Title = "Mis consultas";

            cuenta = cta;

            //lblNomCuenta.Text = cuenta.NombreCta;
            //lblSaldo.Text = cuenta.Moneda + " " + cuenta.SaldoDisponible.ToString();

            //navBar.seleccionarBoton("0");
        }

        private async void BtnMovimientos_OnClicked(object sender, EventArgs args)
        {
            //Navigation.PushAsync(new MovimientosView());
            //ShowPopup();
            Application.Current.Properties["popConsultasPage"] = this;
            await ((ConsultasViewModel)BindingContext).NavegarModal("PopUpOperaciones");
            //await Navigation.PushModalAsync(new PopUpOperacionesView(this), false);
        }

        private async void ShowPopup()
        {
            //Create `ContentPage` with padding and transparent background
            ContentPage loginPage = new ContentPage
            {
                BackgroundColor = Color.FromHex("#D9000000"),
                Padding = new Thickness(20, 20, 20, 20)
            };

            // Create Children

            //Create desired layout to be a content of your popup page.
            var contentLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children =
            {
                // Add children

            }
            };
            //var contentLayout = new AbsoluteLayout
            //{
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    Children =
            //  {
            //        new StackLayout{
            //            VerticalOptions = LayoutOptions.Center,
            //            AbsoluteLayout.SetLayoutBounds(null, new Rectangle(2,2,2,2)),
            //            AbsoluteLayout.SetLayoutFlags(null,AbsoluteLayoutFlags.All)

            //        }
            //  }

            //};


            //set popup page content:
            loginPage.Content = contentLayout;

            //Show Popup
            await Navigation.PushModalAsync(loginPage, false);
        }


        public async void navegarSubOperaciones(string idOperacion)
        {
            Application.Current.Properties["opeId"] = idOperacion;
            Application.Current.Properties["origenMisCuentas"] = true;

            NavigationParameters navParameters = new NavigationParameters();
            navParameters.Add("Operacion", ((ConsultasViewModel)BindingContext).GetOperacionService().BuscarOperacion(idOperacion));
            navParameters.Add("IdOperacion", idOperacion);

            await ((ConsultasViewModel)BindingContext).Navegar("SubOperaciones", navParameters);
            //Navigation.PushAsync(new SubOperacionesView(idOperacion, true), false);
        }
    }
}
