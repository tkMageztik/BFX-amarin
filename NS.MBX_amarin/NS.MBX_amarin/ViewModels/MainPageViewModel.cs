using Acr.UserDialogs;
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
	public class MainPageViewModel : ViewModelBase
	{
        private ICuentaService CuentaService { get; set; }

        public MainPageViewModel(ICuentaService cuentaService, INavigationService navigationService, IUserDialogs userDialogs)
            : base(navigationService, userDialogs)
        {
            this.CuentaService = cuentaService;
        }

        public async Task NavegarSiguiente()
        {
            //await NavigationService.NavigateAsync("NavBar");
            await NavigationService.NavigateAsync("app:///NavBar");
        }

        public IUserDialogs ObtenerUserDialogs()
        {
            return UserDialogs;
        }

        public ICuentaService ObtenerCuentaService()
        {
            return CuentaService;
        }

        public async Task Navegar(string destino)
        {
            await NavigationService.NavigateAsync(destino);
        }
	}
}
