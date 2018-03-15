using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModels
{
	public class CtaCargoViewModel : ViewModelBase
	{
        public CtaCargoViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;
        }

        public async Task Navegar(string ruta)
        {
            await NavigationService.NavigateAsync(ruta);
        }

        public NavigationParameters ObtenerNavParametros()
        {
            return GetNavigationParameters(true);
        }

        public async Task Navegar(string ruta, NavigationParameters navParameters)
        {
            await NavigationService.NavigateAsync(ruta, navParameters);
        }
    }
}
