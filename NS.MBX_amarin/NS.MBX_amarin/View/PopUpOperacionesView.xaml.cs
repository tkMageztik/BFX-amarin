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
    public partial class PopUpOperacionesView : ContentPage
    {
        public PopUpOperacionesView()
        {
            InitializeComponent();
        }

        private async void BtnSeleccionaCtaCargo_OnClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new SeleccionaCtaCargo("Transferencia Ctas mismo banco"));
            //ShowPopup();

            //var page = new NavigationPage(new SeleccionaCtaCargo("Transferencia Ctas mismo banco"));
            //await Navigation.PopToRootAsync();

            
            await Navigation.PushAsync(new CtaCargoView("Transferencia Ctas mismo banco"));
        }
    }
}