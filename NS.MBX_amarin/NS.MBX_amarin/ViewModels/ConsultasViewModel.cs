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
        public ConsultasViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public async Task Navegar(string destino)
        {
            await NavigationService.NavigateAsync(destino);
        }

        public async Task NavegarModal(string destino)
        {
            await NavigationService.NavigateAsync(destino, null, true);
        }
    }
}
