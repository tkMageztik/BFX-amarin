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
	public class RecargaCelularViewModel : ViewModelBase
    {
        private ICatalogoService CatalogoService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        public RecargaCelularViewModel(ITipoCambioService tipService, ICatalogoService catService, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            CatalogoService = catService;
            TipoCambioService = tipService;

            ListaOperadores = CatalogoService.ListarOperadoresMovilesString();
            LbTipoCambio = TipoCambioService.ObtenerDescTipoCambio();
        }

        private ObservableCollection<string> _listaOperadores;
        public ObservableCollection<string> ListaOperadores
        {
            get { return _listaOperadores; }
            set { SetProperty(ref _listaOperadores, value); }
        }

        private string _nomOperador;
        public string NomOperador
        {
            get { return _nomOperador; }
            set { SetProperty(ref _nomOperador, value); }
        }

        private string _numCelular;
        public string NumCelular
        {
            get { return _numCelular; }
            set { SetProperty(ref _numCelular, value); }
        }

        private string _monto;
        public string Monto
        {
            get { return _monto; }
            set { SetProperty(ref _monto, value); }
        }

        private string _lbTipoCambio;
        public string LbTipoCambio
        {
            get { return _lbTipoCambio; }
            set { SetProperty(ref _lbTipoCambio, value); }
        }

        private DelegateCommand _accionSiguienteIC;
        public DelegateCommand AccionSiguienteIC =>
            _accionSiguienteIC ?? (_accionSiguienteIC = new DelegateCommand(ExecuteAccionSiguienteIC));

        async void ExecuteAccionSiguienteIC()
        {
            string msj = ValidarCampos();
            if (msj != "")
            {
                await DialogService.DisplayAlertAsync(Constantes.MSJ_VALIDACION, msj, Constantes.MSJ_BOTON_OK);
            }
            else
            {
                var navParametros = new NavigationParameters();
                navParametros.Add("Monto", Monto);
                navParametros.Add("NumCelular", NumCelular);
                navParametros.Add("NomOperador", NomOperador);

                await NavigationService.NavigateAsync(Constantes.pageCtaCargo, navParametros);
            }
        }

        public string ValidarCampos()
        {
            string msj = "";

            if (Monto == null || Monto == "")
            {
                msj = "Ingrese un monto válido";
            }

            return msj;
        }

    }
}
