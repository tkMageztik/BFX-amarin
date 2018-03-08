using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModels
{
	public class EmpresaViewModel : ViewModelBase
	{
        public EmpresaViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public async Task NavegarServicioEmpresa()
        {
            await NavigationService.NavigateAsync("ServicioEmpresa");
        }
    }
}
