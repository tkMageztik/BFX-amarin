using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class PagoTCPropTipoViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }

        public PagoTCPropTipoViewModel(ICatalogoService catalogoService, INavigationService navigationService)
            : base(navigationService)
        {
            CatalogoService = catalogoService;
        }
	}
}
