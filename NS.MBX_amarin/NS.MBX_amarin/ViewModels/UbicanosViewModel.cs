using NS.MBX_amarin.Model;
using NS.MBX_amarin.Model.Gplaces;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class UbicanosViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private IGPlacesService GPlacesService { get; set; }

        public UbicanosViewModel(IGPlacesService gplacesService, ICatalogoService catalogoService, INavigationService navigationService)
            : base(navigationService)
        {
            GPlacesService = gplacesService;
            CatalogoService = catalogoService;
        }

        private Catalogo _searchItemSelected;
        public Catalogo SearchItemSelected
        {
            get { return _searchItemSelected; }
            set { SetProperty(ref _searchItemSelected, value); }
        }

        private ObservableCollection<AutoCompletePrediction> _listaResultados;
        public ObservableCollection<AutoCompletePrediction> ListaResultados
        {
            get { return _listaResultados; }
            set { SetProperty(ref _listaResultados, value); }
        }

        private Boolean _isSearchBarFocused;
        public Boolean IsSearchBarFocused
        {
            get { return _isSearchBarFocused; }
            set { SetProperty(ref _isSearchBarFocused, value); }
        }

        private string _txtBuscador;
        public string TxtBuscador
        {
            get { return _txtBuscador; }
            set { SetProperty(ref _txtBuscador, value); }
        }

        private DelegateCommand _focusedIC;
        public DelegateCommand FocusedIC =>
            _focusedIC ?? (_focusedIC = new DelegateCommand(ExecuteFocusedIC));

        void ExecuteFocusedIC()
        {
            IsSearchBarFocused = true;
        }

        private DelegateCommand _unfocusedIC;
        public DelegateCommand UnfocusedIC =>
            _unfocusedIC ?? (_unfocusedIC = new DelegateCommand(ExecuteUnfocusedIC));

        void ExecuteUnfocusedIC()
        {
            IsSearchBarFocused = false;
        }

        private DelegateCommand _buscarIC;
        public DelegateCommand BuscarIC =>
            _buscarIC ?? (_buscarIC = new DelegateCommand(ExecuteBuscarIC));

        async void ExecuteBuscarIC()
        {
            try
            {
                //ListaResultados = CatalogoService.BuscarCoincidePorNombre(CatalogoService.COD_UBICACIONES_MAPS, TxtBuscador);
                ListaResultados = await GPlacesService.GetPlaces(TxtBuscador);
                string ejem = "";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        private DelegateCommand _tapListaIC;
        public DelegateCommand TapListaIC =>
            _tapListaIC ?? (_tapListaIC = new DelegateCommand(ExecuteTapListaIC));

        void ExecuteTapListaIC()
        {
            SearchItemSelected = null;
            TxtBuscador = string.Empty;
        }
    }

}
