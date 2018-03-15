using NS.MBX_amarin.Events;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Events;
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
        private IEventAggregator EventAggregator { get; set; }

        public ConfPagoServicioEmpresaViewModel(IOperacionService operacionService, ICuentaService cuentaService, INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            this.CuentaService = cuentaService;
            this.OperacionService = operacionService;
            this.EventAggregator = eventAggregator;
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

        public void PublicarEventoOpeFrecuente()
        {
            EventAggregator.GetEvent<OpeFrecuenteAddedEvent>().Publish();
        }
    }
}
