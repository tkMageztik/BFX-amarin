using NS.MBX_amarin.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class PagoTCResumenViewModel : ViewModelBase
    {
        public PagoTCResumenViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public override void OnNavigatingTo(NavigationParameters parametros)
        {
            RefNavParameters = parametros;

            Operacion operacion = parametros[Constantes.keyOperacion] as Operacion;
            SubOperacion suboperacion = parametros[Constantes.keySubOperacion] as SubOperacion;

            string pageOrigen = parametros[Constantes.pageOrigen] as string;
            string codOperacionGen = parametros[Constantes.keyCodOperacionGenerado] as string;
            Cuenta ctaOrigen = parametros[Constantes.keyCtaCargo] as Cuenta;
            Tarjeta tcDestino = parametros[Constantes.keyTCDestino] as Tarjeta;
            DateTime fechaOperacion = (DateTime)parametros[Constantes.keyFechaOperacion];
            Catalogo origenTarjeta = parametros[Constantes.keyOrigenTarjeta] as Catalogo;

            Catalogo moneda = parametros["Moneda"] as Catalogo;
            string monto = parametros["Monto"] as string;

            LblCodOpe = "Código de Operación: " + codOperacionGen;
            LblFechaHora = "Fecha: " + fechaOperacion.ToString("dd/MM/yyyy") + "      Hora: " + fechaOperacion.ToString("HH:mm");
            LblMonedaMonto = moneda.Descripcion + " " + monto;

            LblNombreCta1 = ctaOrigen.NombreCta;
            LblCodCta1 = ctaOrigen.CodigoCta;

            //parametros
            if (origenTarjeta.Codigo == "0")
            {
                Catalogo tipoPropTarjeta = parametros[Constantes.keyTipoPropTarjeta] as Catalogo;

                if (tipoPropTarjeta.Codigo == "0")
                {
                    LblNombreCta2 = tcDestino.MarcaTarjeta;
                    LblCodCta2 = tcDestino.NroTarjeta;
                }
                else
                {
                    LblNombreCta2 = tcDestino.NombreCliente;
                    LblCodCta2 = tcDestino.NroTarjeta;
                }
            }
            else
            {
                LblNombreCta2 = tcDestino.MarcaTarjeta;
                LblCodCta2 = tcDestino.NroTarjeta;
            }
            

        }

        private string _lblCodOpe;
        public string LblCodOpe
        {
            get { return _lblCodOpe; }
            set { SetProperty(ref _lblCodOpe, value); }
        }

        private string _lblFechaHora;
        public string LblFechaHora
        {
            get { return _lblFechaHora; }
            set { SetProperty(ref _lblFechaHora, value); }
        }

        private string _lblNombreCta1;
        public string LblNombreCta1
        {
            get { return _lblNombreCta1; }
            set { SetProperty(ref _lblNombreCta1, value); }
        }

        private string _lblCodCta1;
        public string LblCodCta1
        {
            get { return _lblCodCta1; }
            set { SetProperty(ref _lblCodCta1, value); }
        }

        private string _lblNombreCta2;
        public string LblNombreCta2
        {
            get { return _lblNombreCta2; }
            set { SetProperty(ref _lblNombreCta2, value); }
        }

        private string _lblCodCta2;
        public string LblCodCta2
        {
            get { return _lblCodCta2; }
            set { SetProperty(ref _lblCodCta2, value); }
        }

        private string _lblMonedaMonto;
        public string LblMonedaMonto
        {
            get { return _lblMonedaMonto; }
            set { SetProperty(ref _lblMonedaMonto, value); }
        }
    }
}
