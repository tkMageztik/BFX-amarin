using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NS.MBX_amarin.ViewModels
{
	public class EmpresaViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }

        public EmpresaViewModel(ICatalogoService catalogoService, INavigationService navigationService)
            : base(navigationService)
        {
            CatalogoService = catalogoService;

            MostrarBuscador = false;
        }

        public ICatalogoService ObtenerCatalogoService()
        {
            return CatalogoService;
        }

        private bool _mostrarBuscador;
        public bool MostrarBuscador
        {
            get { return _mostrarBuscador; }
            set { SetProperty(ref _mostrarBuscador, value); }
        }     

        public async Task NavegarServicioEmpresa()
        {
            await NavigationService.NavigateAsync("ServicioEmpresa");
        }

        private string _txtBuscador;
        public string TxtBuscador
        {
            get { return _txtBuscador; }
            set { SetProperty(ref _txtBuscador, value); }
        }

        private Catalogo _itemSeleccionado;
        public Catalogo ItemSeleccionado
        {
            get { return _itemSeleccionado; }
            set { SetProperty(ref _itemSeleccionado, value); }
        }

        private ObservableCollection<Catalogo> _listaEmpresas;
        public ObservableCollection<Catalogo> ListaEmpresas
        {
            get { return _listaEmpresas; }
            set { SetProperty(ref _listaEmpresas, value); }
        }

        private DelegateCommand _buscarIC;
        public DelegateCommand BuscarIC =>
            _buscarIC ?? (_buscarIC = new DelegateCommand(ExecuteBuscarIC));

        void ExecuteBuscarIC()
        {
            ListaEmpresas = CatalogoService.BuscarEmpresa(TxtBuscador);
        }

        private DelegateCommand _itemTappedIC;
        public DelegateCommand ItemTappedIC =>
            _itemTappedIC ?? (_itemTappedIC = new DelegateCommand(ExecuteItemTappedIC));

        async void ExecuteItemTappedIC()
        {
            var navParameters = new NavigationParameters();
            navParameters.Add("TipoTarjeta", ItemSeleccionado);
            Application.Current.Properties[Constantes.pageOrigen] = Constantes.pageTipoTarjeta;

            await NavigationService.NavigateAsync("CtaCargo", navParameters);

            ItemSeleccionado = null;
        }

        private DelegateCommand _buscadorFocusedIC;
        public DelegateCommand BuscadorFocusedIC =>
            _buscadorFocusedIC ?? (_buscadorFocusedIC = new DelegateCommand(ExecuteBuscadorFocusedIC));

        void ExecuteBuscadorFocusedIC()
        {
            MostrarBuscador = true;
        }
    }
}
