using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

//PERMITE ELEGIR SI LA TARJETA ES PROPIA O DE TERCERO
namespace NS.MBX_amarin.ViewModels
{
	public class PagoTCPropTipoViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }

        public PagoTCPropTipoViewModel(ICatalogoService catalogoService, INavigationService navigationService)
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
            parametros.Add(Constantes.keyTipoPropTarjeta, ItemSeleccionado);
            parametros.Add(Constantes.pageOrigen, Constantes.pagePagoTCPropTipo);

            if (ItemSeleccionado.Codigo == "0")
            {
                await NavigationService.NavigateAsync(Constantes.pageTCPropia, parametros);
            }
            else
            {
                await NavigationService.NavigateAsync(Constantes.pagePagoTCDatos, parametros);
            }

            ItemSeleccionado = null;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;
            Lista = CatalogoService.ObtenerListaPorCodigo(CatalogoService.COD_TIPOS_PROP_TAR);
        }
    }
}
