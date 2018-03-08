
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class CuentasViewModel : ViewModelBase
	{
        public CuentasViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public async void NavegarSiguiente()
        {
            await NavigationService.NavigateAsync("Consultas");
        }
	}
}
