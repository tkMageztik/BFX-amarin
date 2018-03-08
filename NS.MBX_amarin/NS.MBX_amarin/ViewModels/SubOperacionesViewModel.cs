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
        public SubOperacionesViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            var fal = false;
        }

        //cuando se navega hacia aqui
        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            var text = false;
            //text = (bool)parameters["model"];
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            var text = false;
            //text = (bool)parameters["model"];
        }

        public async Task NavegarEmpresa()
        {
            await NavigationService.NavigateAsync("Empresa");
        }

        public async Task NavegarBuscadorEmpresa()
        {
            await NavigationService.NavigateAsync("BuscadorEmpresa");
        }
    }

}

