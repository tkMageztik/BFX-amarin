using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class DatosPagoTarjetaViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }
        public NavigationParameters parametros { get; set; }

        public DatosPagoTarjetaViewModel(ITipoCambioService tipoCambioService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            CatalogoService = catalogoService;
            TipoCambioService = tipoCambioService;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            string codTipoTarjeta = parameters["CodTipoTarjeta"] as string;
            parametros = parameters;

            if(codTipoTarjeta == "2")
            {
                LblTipoTarjeta = "Número de tarjeta Diners";
            }
            else
            {
                LblTipoTarjeta = "Número de tarjeta de crédito";
            }

            ListaMonedas = CatalogoService.ListarMonedasString();
            LblTipoCambio = TipoCambioService.ObtenerDescTipoCambio();
            Monto = null;
        }

        private string _lblTipoTarjeta;
        public string LblTipoTarjeta
        {
            get { return _lblTipoTarjeta; }
            set { SetProperty(ref _lblTipoTarjeta, value); }
        }

        private string _numTarjeta;
        public string NumTarjeta
        {
            get { return _numTarjeta; }
            set { SetProperty(ref _numTarjeta, value); }
        }

        private ObservableCollection<string> _listaMonedas;
        public ObservableCollection<string> ListaMonedas
        {
            get { return _listaMonedas; }
            set { SetProperty(ref _listaMonedas, value); }
        }

        private string _monto;
        public string Monto
        {
            get { return _monto; }
            set { SetProperty(ref _monto, value); }
        }

        private string _lblTipoCambio;
        public string LblTipoCambio
        {
            get { return _lblTipoCambio; }
            set { SetProperty(ref _lblTipoCambio, value); }
        }

        private DelegateCommand _accionSiguienteIC;
        public DelegateCommand AccionSiguienteIC =>
            _accionSiguienteIC ?? (_accionSiguienteIC = new DelegateCommand(ExecuteAccionSiguienteIC));

        private string _nomMoneda;
        public string NomMoneda
        {
            get { return _nomMoneda; }
            set { SetProperty(ref _nomMoneda, value); }
        }

        async void ExecuteAccionSiguienteIC()
        {
            string msj = ValidarCampos();
            if (msj != "")
            {
                await DialogService.DisplayAlertAsync("Validación", msj, "OK");
            }
            else
            {
                parametros.Add("Monto", Monto);
                parametros.Add("NumTarjeta", NumTarjeta);
                parametros.Add("Moneda", CatalogoService.BuscarMonedaPorNombre(NomMoneda));
                parametros.Add(Constantes.pageOrigen, Constantes.pageDatosPagoTarjeta);

                await NavigationService.NavigateAsync(Constantes.pageConfDatosPago, parametros);
            }
        }

        public string ValidarCampos()
        {
            string msj = "";

            if(Monto == null || Monto == "")
            {
                msj = "Ingrese un monto válido";
            }

            return msj;
        }

    }
}
