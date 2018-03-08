﻿using NS.MBX_amarin.Model;
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
	public partial class ConfPagoServicioEmpresaView : ContentPage
	{
		public ConfPagoServicioEmpresaView ()
		{
			InitializeComponent ();

            Cuenta cta = Application.Current.Properties["ctaCargo"] as Cuenta;
            lblCodCta.Text = cta.CodigoCta;
            lblNombreCta.Text = cta.NombreCta;

            lblEmpresa.Text = Application.Current.Properties["nombreEmpresa"] as string;
            lblServicio.Text = Application.Current.Properties["nombreServicio"] as string;
            lblCodigo.Text = Application.Current.Properties["codigo"] as string;

            lblMonedaMonto.Text = "Moneda y monto: S/ " + (string)Application.Current.Properties["monto"];
        }

        public async void EventoConfirmar(object sender, EventArgs args)
        {

            if (entClave.Text == null || entClave.Text == "" )
            {
                await DisplayAlert("Mensaje", "Ingrese una clave válida", "OK");
                return;
            }

            Cuenta cta = Application.Current.Properties["ctaCargo"] as Cuenta;
            string montoStr = Application.Current.Properties["monto"] as string;
            decimal monto = decimal.Parse(montoStr);

            string rpta = CuentaService.efectuarMovimiento(cta, monto, "PEN", false);

            if(rpta != "")
            {
                await DisplayAlert("Mensaje", rpta, "OK");
            }
            else
            {
                if (swtFrecuente.IsToggled)
                {
                    SubOperacion subope = Application.Current.Properties["suboperacionActual"] as SubOperacion;
                    subope.FechaOperacion = DateTime.Now;
                    subope.ServicioFrecuente = Application.Current.Properties["servicio"] as Servicio;

                    Catalogo empresa = Application.Current.Properties["empresa"] as Catalogo;
                    subope.NombreFrecuente = subope.Nombre + ": " + empresa.Nombre;
                    OperacionService.AgregarSuboperacionFrecuente(subope);
                }
                await DisplayAlert("Mensaje", Constantes.msjExito, "OK");
                await Navigation.PopToRootAsync();
            }

        }
    }
}