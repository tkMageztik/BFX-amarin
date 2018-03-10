using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModels
{
	public class TransferenciaViewModel : ViewModelBase
	{
        public TransferenciaViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public async Task PopToRoot()
        {
            await NavigationService.GoBackToRootAsync();
        }
	}
}
