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
	public class ServicioEmpresaViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }

        public ServicioEmpresaViewModel(ICatalogoService catalogoService, INavigationService navigationService)
            : base(navigationService)
        {
            CatalogoService = catalogoService;
        }

        public ICatalogoService ObtenerCatalogoService()
        {
            return CatalogoService;
        }

        public async Task NavegarPagoServicioEmpresa()
        {
            await NavigationService.NavigateAsync("PagoServicioEmpresa");
        }
    }
}
