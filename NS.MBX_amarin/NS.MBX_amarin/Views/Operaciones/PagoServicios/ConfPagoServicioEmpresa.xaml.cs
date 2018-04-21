using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
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
    public partial class ConfPagoServicioEmpresa : ContentPage
    {
        public ConfPagoServicioEmpresa()
        {
            InitializeComponent();

            //Cuenta cta = Application.Current.Properties["ctaCargo"] as Cuenta;
            //lblCodCta.Text = cta.CodigoCta;
            //lblNombreCta.Text = cta.NombreCta;

            //lblEmpresa.Text = Application.Current.Properties["nombreEmpresa"] as string;
            //lblServicio.Text = Application.Current.Properties["nombreServicio"] as string;
            //lblCodigo.Text = Application.Current.Properties["codigo"] as string;

            //lblMonedaMonto.Text = "Moneda y monto: S/ " + (string)Application.Current.Properties["monto"];
        }

        //public async void EventoConfirmar(object sender, EventArgs args)
        //{

        //    if (entClave.Text == null || entClave.Text == "")
        //    {
        //        await DisplayAlert("Mensaje", "Ingrese una clave válida", "OK");
        //        return;
        //    }

        //    Cuenta cta = Application.Current.Properties["ctaCargo"] as Cuenta;
        //    string montoStr = Application.Current.Properties["monto"] as string;
        //    decimal monto = decimal.Parse(montoStr);

        //    string rpta = ((ConfPagoServicioEmpresaViewModel)BindingContext).ObtenerCuentaService().efectuarMovimiento(cta, monto, "PEN", false);

        //    if (rpta != "")
        //    {
        //        await DisplayAlert(Constantes.MSJ_INFO, rpta, Constantes.MSJ_BOTON_OK);
        //    }
        //    else
        //    {
        //        if (swtFrecuente.IsToggled)
        //        {
        //            SubOperacion subope = Application.Current.Properties["suboperacionActual"] as SubOperacion;
        //            Catalogo empresa = Application.Current.Properties["empresa"] as Catalogo;
        //            OperacionFrecuente opeFrec = new OperacionFrecuente
        //            {
        //                FechaOperacion = DateTime.Now,
        //                SubOperacion = subope,
        //                Operacion = ((ConfPagoServicioEmpresaViewModel)BindingContext).ObtenerOperacionService().BuscarOperacion(subope.IdOperacion),
        //                Servicio = Application.Current.Properties["servicio"] as Servicio,
        //                NombreFrecuente = subope.Nombre + ": " + empresa.Nombre
        //            };

        //            ((ConfPagoServicioEmpresaViewModel)BindingContext).ObtenerOperacionService().AgregarOperacionFrecuente(opeFrec);
        //            ((ConfPagoServicioEmpresaViewModel)BindingContext).PublicarEventoOpeFrecuente();
        //        }
        //        await DisplayAlert("Mensaje", Constantes.msjExito, "OK");
        //        await ((ConfPagoServicioEmpresaViewModel)BindingContext).RetornarInicio();
        //    }

        //}
    }
}
