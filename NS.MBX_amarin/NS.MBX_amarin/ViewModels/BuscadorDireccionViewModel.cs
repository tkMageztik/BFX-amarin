using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using NS.MBX_amarin.Views;
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
	public class BuscadorDireccionViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }

        //public static readonly BindableProperty IsSearchBarFocusedProperty = BindableProperty.Create("IsSearchBarFocused", typeof(Boolean), typeof(BuscadorDireccion), false, BindingMode.TwoWay, IsFocused);

        public BuscadorDireccionViewModel(ICatalogoService catalogoService, INavigationService navigationService)
            : base(navigationService)
        {
            CatalogoService = catalogoService;
        }

        private string _txtBuscador;
        public string TxtBuscador
        {
            get { return _txtBuscador; }
            set { SetProperty(ref _txtBuscador, value); }
        }

        private Boolean _isFocused;
        public Boolean IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private Catalogo _itemSelected;
        public Catalogo ItemSelected
        {
            get { return _itemSelected; }
            set { SetProperty(ref _itemSelected, value); }
        }

        private ObservableCollection<Catalogo> _listaResultados;
        public ObservableCollection<Catalogo> ListaResultados
        {
            get { return _listaResultados; }
            set { SetProperty(ref _listaResultados, value); }
        }

        private DelegateCommand _buscarIC;
        public DelegateCommand BuscarIC =>
            _buscarIC ?? (_buscarIC = new DelegateCommand(ExecuteBuscarIC));

        void ExecuteBuscarIC()
        {
            ListaResultados = CatalogoService.BuscarEmpresa(TxtBuscador);
        }

        private DelegateCommand _focusedIC;
        public DelegateCommand FocusedIC =>
            _focusedIC ?? (_focusedIC = new DelegateCommand(ExecuteFocusedIC));

        void ExecuteFocusedIC()
        {
            IsFocused = true;
        }

        private DelegateCommand _unfocusedIC;
        public DelegateCommand UnfocusedIC =>
            _unfocusedIC ?? (_unfocusedIC = new DelegateCommand(ExecuteUnfocusedIC));

        void ExecuteUnfocusedIC()
        {
            IsFocused = false;
        }

        private DelegateCommand _tapListaIC;
        public DelegateCommand TapListaIC =>
            _tapListaIC ?? (_tapListaIC = new DelegateCommand(ExecuteTapListaIC));

        void ExecuteTapListaIC()
        {
            IsFocused = false;
        }
    }
}
