using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public async void NavegarSiguiente()
        {
            //await NavigationService.NavigateAsync("NavBar");
            await NavigationService.NavigateAsync("app:///NavBar");
        }
	}
}
