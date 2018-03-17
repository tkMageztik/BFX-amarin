using Acr.UserDialogs;
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

namespace NS.MBX_amarin.ViewModels
{
	public class ServicioEmpresaViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }

        public ServicioEmpresaViewModel(ICatalogoService catalogoService, INavigationService navigationService, IUserDialogs userDialogs)
            : base(navigationService, userDialogs)
        {
            CatalogoService = catalogoService;
        }

        private string _nomEmpresa;
        public string NomEmpresa
        {
            get { return _nomEmpresa; }
            set { SetProperty(ref _nomEmpresa, value); }
        }

        private ObservableCollection<Servicio> _listaServicios;
        public ObservableCollection<Servicio> ListaServicios
        {
            get { return _listaServicios; }
            set { SetProperty(ref _listaServicios, value); }
        }

        private Servicio _servicioSelected;
        public Servicio ServicioSelected
        {
            get { return _servicioSelected; }
            set { SetProperty(ref _servicioSelected, value); }
        }

        private string _codigoCliente;
        public string CodigoCliente
        {
            get { return _codigoCliente; }
            set { SetProperty(ref _codigoCliente, value); }
        }

        private DelegateCommand _siguienteIC;
        public DelegateCommand SiguienteIC =>
            _siguienteIC ?? (_siguienteIC = new DelegateCommand(ExecuteSiguienteIC));

        async void ExecuteSiguienteIC()
        {
            try
            {
                if (string.IsNullOrEmpty(CodigoCliente))
                {
                    await UserDialogs.AlertAsync("Ingrese un código válido", "Mensaje", "OK");
                    return;
                }

                NavigationParameters parametros = GetNavigationParameters("Servicio");//indicamos que no considere servicio, porque recien sera añadido

                Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;
                parametros.Add("Servicio", ServicioSelected);
                parametros.Add("stringEmpresa", NomEmpresa);
                parametros.Add("stringPicServicio", ServicioSelected.Nombre);
                parametros.Add("stringCodigo", CodigoCliente);

                await NavigationService.NavigateAsync("PagoServicioEmpresa", parametros);

                ServicioSelected = null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;
            string pageOrigen = RefNavParameters[Constantes.pageOrigen] as string;
            Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;

            NomEmpresa = empresa.Nombre;

            ListaServicios = CatalogoService.ListarServiciosxEmpresa(empresa.Codigo);

            if (pageOrigen == "OperacionesView")
            {
                Servicio servicio = RefNavParameters["Servicio"] as Servicio;
                ServicioSelected = ListaServicios.Where(p => p.Codigo == servicio.Codigo).First();
            }

        }
    }
}
