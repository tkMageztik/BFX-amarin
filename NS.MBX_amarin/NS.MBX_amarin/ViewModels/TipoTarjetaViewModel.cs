using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace NS.MBX_amarin.ViewModels
{
	public class TipoTarjetaViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private NavigationParameters NavParameters { get; set; }

        public TipoTarjetaViewModel(ICatalogoService catalogoService, INavigationService navigationService)
            : base(navigationService)
        {
            CatalogoService = catalogoService;
            Lista = CatalogoService.ListarTiposTarjetaCredito();
        }

        private Catalogo _itemSeleccionado;
        public Catalogo ItemSeleccionado
        {
            get { return _itemSeleccionado; }
            set { SetProperty(ref _itemSeleccionado, value); }
        }

        private ObservableCollection<Catalogo> _lista;
        public ObservableCollection<Catalogo> Lista
        {
            get { return _lista; }
            set { SetProperty(ref _lista, value); }
        }

        private DelegateCommand _itemTappedIC;
        public DelegateCommand ItemTappedIC =>
            _itemTappedIC ?? (_itemTappedIC = new DelegateCommand(ExecuteItemTappedIC));

        async void ExecuteItemTappedIC()
        {
            NavParameters.Add("TipoTarjeta", ItemSeleccionado);
            NavParameters.Add("CodTipoTarjeta", ItemSeleccionado.Codigo);

            Application.Current.Properties["strTipoTransf"] = "0";
            Application.Current.Properties["strOrigenMisCuentas"] = false;
            Application.Current.Properties["strPageOrigen"] = Constantes.pageTipoTarjeta;
            Application.Current.Properties["CodTipoTarjeta"] = ItemSeleccionado.Codigo;
            await NavigationService.NavigateAsync(Constantes.pageCtaCargo, NavParameters);

            ItemSeleccionado = null;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            NavParameters = parameters;
        }
    }
}
