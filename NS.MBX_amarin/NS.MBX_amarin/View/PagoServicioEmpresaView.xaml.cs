using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
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
	public partial class PagoServicioEmpresaView : ContentPage
	{
		public PagoServicioEmpresaView (string empresa, string servicio, string codigo)
		{
			InitializeComponent ();

            lblEmpresa.Text = empresa;
            lblServicio.Text = servicio;
            lblCodigo.Text = codigo;

            TipoCambio tipo = TipoCambioService.obtenerTipoCambio();
            lblTipoCambio.Text = "Tipo de cambio ref. Compra: " + tipo.CompraDolares + " Venta: " + tipo.VentaDolares;
		}

        public async void EventoSiguiente(object sender, EventArgs args)
        {
            int mont = 0;
            if (!int.TryParse(entMonto.Text, out mont))
            {
                await DisplayAlert("Mensaje", "Ingrese un monto válido", "OK");
                return;
            }else if(mont <= 0)
            {
                await DisplayAlert("Mensaje", "Ingrese un monto válido", "OK");
                return;
            }

            Application.Current.Properties["nombreEmpresa"] = lblEmpresa.Text;
            Application.Current.Properties["nombreServicio"] = lblServicio.Text;
            Application.Current.Properties["codigo"] = lblCodigo.Text;
            Application.Current.Properties["monto"] = entMonto.Text;
            await Navigation.PushAsync(new CtaCargoView("0", false, "PagoServicioEmpresaView"), false);
        }
    }
}