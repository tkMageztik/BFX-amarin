using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModels
{
	public class PagoServicioEmpresaViewModel : ViewModelBase
	{
        private ITipoCambioService TipoCambioService { get; set; }

        public PagoServicioEmpresaViewModel(ITipoCambioService tipoCambioService, INavigationService navigationService)
            : base(navigationService)
        {
            TipoCambioService = tipoCambioService;
        }

        public ITipoCambioService ObtenerTipoCambioService()
        {
            return TipoCambioService;
        }

        public async Task NavegarCtaCargo()
        {
            await NavigationService.NavigateAsync("CtaCargo");
        }
    }
}
