using NS.MBX_amarin.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class PagoServResumenViewModel : ViewModelBase
    {
        public PagoServResumenViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public override void OnNavigatingTo(NavigationParameters parametros)
        {
            RefNavParameters = parametros;

            string pageOrigen = parametros[Constantes.pageOrigen] as string;

            Recibo reciboIBS = RefNavParameters[Constantes.keyReciboIBS] as Recibo;
            DetalleRecibo detReciboIBS = RefNavParameters[Constantes.keyDetalleReciboIBS] as DetalleRecibo;
            Catalogo empresa = RefNavParameters[Constantes.keyEmpresa] as Catalogo;
            Servicio servicio = RefNavParameters[Constantes.keyServicio] as Servicio;
            string codigoServicio = RefNavParameters[Constantes.keyCodigoServicio] as string;
            Catalogo moneda = RefNavParameters[Constantes.keyMoneda] as Catalogo;

            Cuenta ctaOrigen = parametros[Constantes.keyCtaCargo] as Cuenta;
            string codOperacionGen = parametros[Constantes.keyCodOperacionGenerado] as string;
            DateTime fechaOperacion = (DateTime)parametros[Constantes.keyFechaOperacion];

            LblNombreCta1 = ctaOrigen.NombreCta;
            LblCodCta1 = ctaOrigen.CodigoCta;

            LblNomEmpresa = empresa.Nombre;
            LblNomServicio = servicio.Nombre;

            LblNomCliente = reciboIBS.NombreCliente;
            LblCodServicio = codigoServicio;

            LblCodOpe = "Código de Operación: " + codOperacionGen;
            LblFechaHora = "Fecha: " + fechaOperacion.ToString("dd/MM/yyyy") + "      Hora: " + fechaOperacion.ToString("HH:mm");
            LblMonedaMonto = moneda.Descripcion + " " + detReciboIBS.Monto;
            
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

        private string _lblMonedaMonto;
        public string LblMonedaMonto
        {
            get { return _lblMonedaMonto; }
            set { SetProperty(ref _lblMonedaMonto, value); }
        }

        private string _lblNomEmpresa;
        public string LblNomEmpresa
        {
            get { return _lblNomEmpresa; }
            set { SetProperty(ref _lblNomEmpresa, value); }
        }

        private string _lblNomServicio;
        public string LblNomServicio
        {
            get { return _lblNomServicio; }
            set { SetProperty(ref _lblNomServicio, value); }
        }

        private string _lblNomCliente;
        public string LblNomCliente
        {
            get { return _lblNomCliente; }
            set { SetProperty(ref _lblNomCliente, value); }
        }

        private string _lblCodServicio;
        public string LblCodServicio
        {
            get { return _lblCodServicio; }
            set { SetProperty(ref _lblCodServicio, value); }
        }
    }
}
