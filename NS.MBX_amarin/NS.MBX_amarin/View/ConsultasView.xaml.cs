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
    public partial class ConsultasView : ContentPage
    {
        public Cuenta cuenta;
        public ConsultasView(Cuenta cta)
        {
            InitializeComponent();
            Title = "Mis consultas";

            cuenta = cta;

            //lblNomCuenta.Text = cuenta.NombreCta;
            //lblSaldo.Text = cuenta.Moneda + " " + cuenta.SaldoDisponible.ToString();

            navBar.seleccionarBoton("0");
        }

        private async void BtnMovimientos_OnClicked(object sender, EventArgs args)
        {
            //Navigation.PushAsync(new MovimientosView());
            //ShowPopup();
            
             
            await Navigation.PushModalAsync(new PopUpOperacionesView(this), false);
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


        public void navegarSubOperaciones(string idOperacion)
        {
            Navigation.PushAsync(new SubOperacionesView(idOperacion, true), false);
        }


    }
}
