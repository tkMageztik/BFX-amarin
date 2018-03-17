using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using NS.MBX_amarin.View;
using NS.MBX_amarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.Views
{
    public partial class PagoServicioEmpresa : ContentPage
    {
        public PagoServicioEmpresa()
        {
            InitializeComponent();
            
        //    lblEmpresa.Text = Application.Current.Properties["stringEmpresa"] as string;
        //    lblServicio.Text = Application.Current.Properties["stringPicServicio"] as string;
        //    lblCodigo.Text = Application.Current.Properties["stringCodigo"] as string;

        //    TipoCambio tipo = ((PagoServicioEmpresaViewModel)BindingContext).ObtenerTipoCambioService().obtenerTipoCambio();
        //    lblTipoCambio.Text = "Tipo de cambio ref. Compra: " + tipo.CompraDolares + " Venta: " + tipo.VentaDolares;
        }

        //public async void EventoSiguiente(object sender, EventArgs args)
        //{
        //    int mont = 0;
        //    if (!int.TryParse(entMonto.Text, out mont))
        //    {
        //        await DisplayAlert("Mensaje", "Ingrese un monto válido", "OK");
        //        return;
        //    }
        //    else if (mont <= 0)
        //    {
        //        await DisplayAlert("Mensaje", "Ingrese un monto válido", "OK");
        //        return;
        //    }

        //    Application.Current.Properties["nombreEmpresa"] = lblEmpresa.Text;
        //    Application.Current.Properties["nombreServicio"] = lblServicio.Text;
        //    Application.Current.Properties["codigo"] = lblCodigo.Text;
        //    Application.Current.Properties["monto"] = entMonto.Text;

        //    Application.Current.Properties["strTipoTransf"] = "0";
        //    Application.Current.Properties["strOrigenMisCuentas"] = false;
        //    Application.Current.Properties["strPageOrigen"] = "PagoServicioEmpresaView";
        //    await ((PagoServicioEmpresaViewModel)BindingContext).NavegarCtaCargo();
        //    //await Navigation.PushAsync(new CtaCargoView(), false);
        //}
    }
}
