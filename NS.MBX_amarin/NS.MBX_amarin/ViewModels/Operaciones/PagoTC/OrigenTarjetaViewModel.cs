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

//PERMITE ELEGIR SI LA TARJETA ES PROPIO BANCO O DE OTRO BANCO
namespace NS.MBX_amarin.ViewModels
{
	public class OrigenTarjetaViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }

        public OrigenTarjetaViewModel(ICatalogoService catalogoService, INavigationService navigationService)
            : base(navigationService)
        {
            CatalogoService = catalogoService;            
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
            NavigationParameters parametros = GetNavigationParameters();
            parametros.Add(Constantes.pageOrigen, Constantes.pageOrigenTarjeta);
            parametros.Add(Constantes.keyOrigenTarjeta, ItemSeleccionado);

            await NavigationService.NavigateAsync(Constantes.pageCtaCargo, parametros);

            ItemSeleccionado = null;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;
            Lista = CatalogoService.ListarTiposTarjetaCredito();
        }
    }
}
