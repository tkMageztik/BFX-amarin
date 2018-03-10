using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace NS.MBX_amarin.ViewModels
{
	public class RecargaBimViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        public RecargaBimViewModel(ITipoCambioService tipService, ICatalogoService catService, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            TipoCambioService = tipService;
            CatalogoService = catService;
        }

        private string _numBim;
        public string NumBim
        {
            get { return _numBim; }
            set { SetProperty(ref _numBim, value); }
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

        private DelegateCommand _tapInfoIC;
        public DelegateCommand TapInfoIC =>
            _tapInfoIC ?? (_tapInfoIC = new DelegateCommand(ExecuteTapInfoIC));

        async void ExecuteTapInfoIC()
        {
            await DialogService.DisplayAlertAsync(Constantes.MSJ_INFO, "Ingresa el número de tu billetera móvil u otra que desees recargar.", Constantes.MSJ_BOTON_ACEPTAR);
        }

        private DelegateCommand _accionSiguienteIC;
        public DelegateCommand AccionSiguienteIC =>
            _accionSiguienteIC ?? (_accionSiguienteIC = new DelegateCommand(ExecuteAccionSiguienteIC));

        async void ExecuteAccionSiguienteIC()
        {
            string msj = ValidarCampos();
            if (msj != "")
            {
                await DialogService.DisplayAlertAsync(Constantes.MSJ_VALIDACION, msj, Constantes.MSJ_BOTON_ACEPTAR);
            }
            else
            {
                var navParametros = new NavigationParameters();
                navParametros.Add("Monto", Monto);
                navParametros.Add("NumBim", NumBim);
                navParametros.Add(Constantes.pageOrigen, Constantes.pageRecargaBim);
                navParametros.Add("Moneda", CatalogoService.BuscarMonedaPorCodigo("PEN"));

                Application.Current.Properties["strTipoTransf"] = "0";
                Application.Current.Properties["strOrigenMisCuentas"] = false;
                Application.Current.Properties["strPageOrigen"] = Constantes.pageRecargaBim;
                await NavigationService.NavigateAsync(Constantes.pageCtaCargo, navParametros);
            }
        }

        public string ValidarCampos()
        {
            string msj = "";

            if (string.IsNullOrWhiteSpace(Monto))
            {
                msj = "Ingrese un monto válido";
            }

            return msj;
        }
    }
}
