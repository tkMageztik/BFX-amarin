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
	public class PagoTCDatosViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        public PagoTCDatosViewModel(ICuentaService cuentaService, ITipoCambioService tipoCambioService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            CuentaService = cuentaService;
            CatalogoService = catalogoService;
            TipoCambioService = tipoCambioService;
        }

        //init
        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;
        }

    }
}
