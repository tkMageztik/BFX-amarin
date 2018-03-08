using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using NS.MBX_amarin.ViewModels;
using Prism.Navigation;

namespace NS.MBX_amarin.ViewModel
{
    public class BuscadorEmpresaViewModel : ViewModelBase
    {
        private string _txtBuscador;
        private ObservableCollection<Catalogo> _listaEmpresas;

        public ICommand BuscarEmpresaIC { get; private set; }

        public BuscadorEmpresaViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            BuscarEmpresaIC = new Command(BuscarEmpresa);
        }

        public void BuscarEmpresa()
        {
            ObservableCollection<Catalogo> listaTotal = CatalogoService.ListarEmpresas();
            List<Catalogo> listaFiltro = listaTotal.Where(c => c.Nombre.Contains(TxtBuscador)).ToList();

            ListaEmpresas = new ObservableCollection<Catalogo>(listaFiltro);
        }

        public string TxtBuscador
        {
            get { return _txtBuscador; }
            set { if (_txtBuscador != value) { _txtBuscador = value; OnPropertyChanged("TxtBuscador"); } }
        }

        public ObservableCollection<Catalogo> ListaEmpresas
        {
            get { return _listaEmpresas; }
            set { if (_listaEmpresas != value) { _listaEmpresas = value; OnPropertyChanged("ListaEmpresas"); } }
        }
    }
}
