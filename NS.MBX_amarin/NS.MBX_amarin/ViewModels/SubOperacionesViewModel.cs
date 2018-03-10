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
	public class SubOperacionesViewModel : ViewModelBase
	{
        private IOperacionService OperacionService { get; set; }

        public SubOperacionesViewModel(IOperacionService operacionService, INavigationService navigationService)
            : base(navigationService)
        {
            this.OperacionService = operacionService;
        }

        public IOperacionService ObtenerOperacionService()
        {
            return OperacionService;
        }

        //cuando se navega hacia aqui
        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            //text = (bool)parameters["model"];
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            //text = (bool)parameters["model"];
        }

        public async Task Navegar(string destino)
        {
            await NavigationService.NavigateAsync(destino);
        }

        public async Task NavegarEmpresa()
        {
            await NavigationService.NavigateAsync("Empresa");
        }

        public async Task NavegarBuscadorEmpresa()
        {
            await NavigationService.NavigateAsync("BuscadorEmpresa");
        }

        public async Task NavegarTipoTarjeta()
        {
            await NavigationService.NavigateAsync("TipoTarjeta");
        }
    }

}

