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
	public class ConfPagoServicioEmpresaViewModel : ViewModelBase
	{
        private ICuentaService CuentaService { get; set; }
        private IOperacionService OperacionService { get; set; }

        public ConfPagoServicioEmpresaViewModel(IOperacionService operacionService, ICuentaService cuentaService, INavigationService navigationService)
            : base(navigationService)
        {
            this.CuentaService = cuentaService;
            this.OperacionService = operacionService;
        }

        public ICuentaService ObtenerCuentaService()
        {
            return CuentaService;
        }

        public IOperacionService ObtenerOperacionService()
        {
            return OperacionService;
        }

        public async Task RetornarInicio()
        {
            await NavigationService.GoBackToRootAsync();
        }
    }
}
