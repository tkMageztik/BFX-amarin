using NS.MBX_amarin.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class TransfResumenViewModel : ViewModelBase
    {
        public TransfResumenViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public override void OnNavigatingTo(NavigationParameters parametros)
        {
            RefNavParameters = parametros;

            Operacion operacion = parametros["Operacion"] as Operacion;
            SubOperacion suboperacion = parametros["SubOperacion"] as SubOperacion;

            string pageOrigen = parametros[Constantes.pageOrigen] as string;
            string codOperacionGen = parametros["CodOperacionGenerado"] as string;
            Cuenta ctaOrigen = parametros["CtaCargo"] as Cuenta;
            Cuenta ctaDestino = parametros["CtaDestino"] as Cuenta;
            DateTime fechaOperacion = (DateTime)parametros["FechaOperacion"];

            Catalogo moneda = parametros["Moneda"] as Catalogo;
            string monto = parametros["Monto"] as string;

            LblCodOpe = "Código de Operación: " + codOperacionGen;
            LblFechaHora = "Fecha: " + fechaOperacion.ToString("dd/MM/yyyy") + "      Hora: " + fechaOperacion.ToString("HH:mm");
            LblMonedaMonto = moneda.Descripcion + " " + monto;

            LblNombreCta1 = ctaOrigen.NombreCta;
            LblCodCta1 = ctaOrigen.CodigoCta;

            if(operacion.Id == "3")
            {
                if (suboperacion.Id == "2")//cta propia
                {
                    LblNombreCta2 = ctaDestino.NombreCta;
                    LblCodCta2 = ctaDestino.CodigoCta;
                }
                else if (suboperacion.Id == "0" || suboperacion.Id == "1")
                {
                    LblNombreCta2 = ctaDestino.NombreTitular;
                    LblCodCta2 = ctaDestino.CodigoCta;
                }
            }
            
        }

        public override async void OnNavigatedFrom(NavigationParameters parametros)
        {
            await NavigationService.GoBackToRootAsync();
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
