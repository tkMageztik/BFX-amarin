using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class NavBarViewModel : ViewModelBase
	{
        public NavBarViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }
	}
}
