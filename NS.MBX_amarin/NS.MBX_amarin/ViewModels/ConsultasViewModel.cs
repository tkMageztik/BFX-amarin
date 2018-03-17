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
	public class ConsultasViewModel : ViewModelBase
	{
        private IOperacionService OperacionService;

        public ConsultasViewModel(INavigationService navigationService, IOperacionService operacionService)
            : base(navigationService)
        {
            OperacionService = operacionService;
        }

        public async Task Navegar(string destino)
        {
            await NavigationService.NavigateAsync(destino);
        }

        public async Task Navegar(string destino, NavigationParameters nav)
        {
            await NavigationService.NavigateAsync(destino, nav);
        }

        public async Task NavegarModal(string destino)
        {
            await NavigationService.NavigateAsync(destino, null, true);
        }

        public IOperacionService GetOperacionService()
        {
            return OperacionService;
        }
    }
}
