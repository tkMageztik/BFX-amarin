using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModels
{
	public class CtaDestinoViewModel : ViewModelBase
	{
        public CtaDestinoViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public async Task Navegar(string destino)
        {
            await NavigationService.NavigateAsync(destino);
        }
	}
}
