using NS.MBX_amarin.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.Views
{
    public partial class PopUpOperaciones : ContentPage
    {
        Consultas consultasView;
        public PopUpOperaciones()
        {
            InitializeComponent();

            Consultas conView = Application.Current.Properties["popConsultasPage"] as Consultas;
            consultasView = conView;
        }

        private async void BtnSeleccionaCtaCargo_OnClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new SeleccionaCtaCargo("Transferencia Ctas mismo banco"));
            //ShowPopup();

            //var page = new NavigationPage(new SeleccionaCtaCargo("Transferencia Ctas mismo banco"));
            //await Navigation.PopToRootAsync();

            //await Navigation.PushAsync(new CtaCargoView("Transferencia Ctas mismo banco"));
            consultasView.navegarSubOperaciones("3");
            await Navigation.PopModalAsync();

        }
    }
}
