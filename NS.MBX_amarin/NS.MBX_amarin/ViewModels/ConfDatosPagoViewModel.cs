using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class ConfDatosPagoViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }

        public Cuenta Cuenta { get; set; }
        public Catalogo Moneda { get; set; }
        public string Monto { get; set; }

        public ConfDatosPagoViewModel(ICuentaService cuentaService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            CatalogoService = catalogoService;
            CuentaService = cuentaService;
        }

        public override void OnNavigatedTo(NavigationParameters parametros)
        {
            string pageOrigen = parametros[Constantes.pageOrigen] as string;

            if(pageOrigen == Constantes.pageDatosPagoTarjeta)
            {
                string codTipoTarjeta = parametros["CodTipoTarjeta"] as string;
                LblDatos1 = CatalogoService.ObtenerTipoTarjetaCredito(codTipoTarjeta).Descripcion;
                LblDatos2 = parametros["NumTarjeta"] as string;
            }
            else if(pageOrigen == Constantes.pageRecargaCelular)
            {
                LblDatos1 = parametros["NomOperador"] as string;
                LblDatos2 = parametros["NumCelular"] as string;
            }
            else if (pageOrigen == Constantes.pageRecargaBim)
            {
                LblDatos1 = "Recarga de Billetera Móvil";
                LblDatos2 = parametros["NumBim"] as string;
            }

            Cuenta = parametros["CtaCargo"] as Cuenta;
            LblNombreCta = Cuenta.NombreCta;
            LblCodCta = Cuenta.CodigoCta;

            Moneda = parametros["Moneda"] as Catalogo;
            Monto = parametros["Monto"] as string;

            LblMonedaMonto = "Moneda y monto: " + Moneda.Descripcion + " " + Monto;
        }

        private string _lblNombreCta;
        public string LblNombreCta
        {
            get { return _lblNombreCta; }
            set { SetProperty(ref _lblNombreCta, value); }
        }

        private string _lblCodCta;
        public string LblCodCta
        {
            get { return _lblCodCta; }
            set { SetProperty(ref _lblCodCta, value); }
        }

        private string _lblDatos1;
        public string LblDatos1
        {
            get { return _lblDatos1; }
            set { SetProperty(ref _lblDatos1, value); }
        }

        private string _lblDatos2;
        public string LblDatos2
        {
            get { return _lblDatos2; }
            set { SetProperty(ref _lblDatos2, value); }
        }

        private string _lblMonedaMonto;
        public string LblMonedaMonto
        {
            get { return _lblMonedaMonto; }
            set { SetProperty(ref _lblMonedaMonto, value); }
        }

        private string _claveSms;
        public string ClaveSms
        {
            get { return _claveSms; }
            set { SetProperty(ref _claveSms, value); }
        }

        private bool _isOperacionFrecuente;
        public bool IsOperacionFrecuente
        {
            get { return _isOperacionFrecuente; }
            set { SetProperty(ref _isOperacionFrecuente, value); }
        }

        private DelegateCommand _accionConfirmarIC;
        public DelegateCommand AccionConfirmarIC =>
            _accionConfirmarIC ?? (_accionConfirmarIC = new DelegateCommand(ExecuteAccionConfirmarIC));

        async void ExecuteAccionConfirmarIC()
        {
            string msj = ValidarCampos();
            if (msj != "")
            {
                await DialogService.DisplayAlertAsync("Validación", msj, "OK");
                return;
            }

            decimal montoDec = decimal.Parse(Monto);

            string rptaTrx = CuentaService.efectuarMovimiento(Cuenta, montoDec, Moneda.Codigo, false);

            if (rptaTrx != "")
            {
                await DialogService.DisplayAlertAsync(Constantes.MSJ_INFO, rptaTrx, Constantes.MSJ_BOTON_OK);
            }
            else
            {
                if (IsOperacionFrecuente)
                {
                    //TODO
                }
                await DialogService.DisplayAlertAsync(Constantes.MSJ_INFO, Constantes.MSJ_EXITO, Constantes.MSJ_BOTON_OK);
                await NavigationService.GoBackToRootAsync();
            }

            //parametros.Add("Monto", Monto);
            //parametros.Add("NumTarjeta", NumTarjeta);
            //parametros.Add("Moneda", CatalogoService.BuscarMonedaPorNombre(NomMoneda));

            //await NavigationService.NavigateAsync(Constantes.pageConfDatosPago, parametros);

        }

        public string ValidarCampos()
        {
            string msj = "";

            if (ClaveSms == null || ClaveSms == "")
            {
                msj = "Ingrese un monto válido";
            }

            return msj;
        }

    }
}
