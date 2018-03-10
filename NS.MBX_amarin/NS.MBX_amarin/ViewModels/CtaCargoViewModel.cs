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
        public NavigationParameters NavParametros { get; set; }

        public CtaCargoViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            NavParametros = parameters;
        }

        public async Task Navegar(string ruta)
        {
            await NavigationService.NavigateAsync(ruta);
        }

        public async Task Navegar(string ruta, NavigationParameters navParameters)
        {
            await NavigationService.NavigateAsync(ruta, navParameters);
        }
    }
}
