using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NS.MBX_amarin.ViewModels
{
	public class OperacionesViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private IOperacionService OperacionService { get; set; }

        public OperacionesViewModel(IOperacionService operacionService, ICatalogoService catalogoService, INavigationService navigationService)
            : base(navigationService)
        {
            CatalogoService = catalogoService;
            OperacionService = operacionService;
        }

        public ICatalogoService ObtenerCatalogoService()
        {
            return CatalogoService;
        }

        public IOperacionService ObtenerOperacionService()
        {
            return OperacionService;
        }

        public async Task NavegarSuboperaciones()
        {
            //var navParameters = new NavigationParameters();
            //navParameters.Add("IdOperacion", idOperacion);
            //navParameters.Add("OrigenMisCuentas", origenMisCuentas);
            //navParameters.Add("RefPage", refPage);
            await NavigationService.NavigateAsync("SubOperaciones");
            //await Navigation.PushAsync(new SubOperacionesView(ope.Id, false), false);
        }

        public async Task Navegar(string destino)
        {
            //var navParameters = new NavigationParameters();
            //navParameters.Add("IdOperacion", idOperacion);
            //navParameters.Add("OrigenMisCuentas", origenMisCuentas);
            //navParameters.Add("RefPage", refPage);
            await NavigationService.NavigateAsync(destino);
            //await Navigation.PushAsync(new SubOperacionesView(ope.Id, false), false);
        }
    }
}
