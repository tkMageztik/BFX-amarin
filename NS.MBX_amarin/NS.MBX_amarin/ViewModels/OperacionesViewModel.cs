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
        public OperacionesViewModel(INavigationService navigationService)
            : base(navigationService)
        {

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
	}
}
