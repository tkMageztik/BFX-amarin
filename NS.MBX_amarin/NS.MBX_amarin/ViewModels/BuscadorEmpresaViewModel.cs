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
	public class BuscadorEmpresaViewModel : ViewModelBase
	{
        public BuscadorEmpresaViewModel(INavigationService navigationService)
            : base(navigationService)
        {

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

        private DelegateCommand<Object> _itemTappedIC;
        public DelegateCommand<Object> ItemTappedIC =>
            _itemTappedIC ?? (_itemTappedIC = new DelegateCommand<Object>(ExecuteItemTappedIC));

        async void ExecuteItemTappedIC(Object item)
        {
            var navParameters = new NavigationParameters();
            navParameters.Add("Empresa", ItemSeleccionado);
            Application.Current.Properties["empresa"] = ItemSeleccionado;

            await NavigationService.NavigateAsync("ServicioEmpresa", navParameters);
            //await((EmpresaViewModel)BindingContext).NavegarServicioEmpresa();
            //await Navigation.PushAsync(new ServicioEmpresaView(), false);

            //Deselect Item
            ItemSeleccionado = null;
        }

	}
}
