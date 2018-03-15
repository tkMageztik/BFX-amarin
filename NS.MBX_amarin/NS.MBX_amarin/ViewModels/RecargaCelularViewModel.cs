using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace NS.MBX_amarin.ViewModels
{
	public class RecargaCelularViewModel : ViewModelBase
    {
        private ICatalogoService CatalogoService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }
        
        public RecargaCelularViewModel(ITipoCambioService tipService, ICatalogoService catService, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            CatalogoService = catService;
            TipoCambioService = tipService;

            ListaOperadores = CatalogoService.ListarOperadoresMoviles();
            LbTipoCambio = TipoCambioService.ObtenerDescTipoCambio();
        }

        private ObservableCollection<Catalogo> _listaOperadores;
        public ObservableCollection<Catalogo> ListaOperadores
        {
            get { return _listaOperadores; }
            set { SetProperty(ref _listaOperadores, value); }
        }

        private Catalogo _operadorSelected;
        public Catalogo OperadorSelected
        {
            get { return _operadorSelected; }
            set { SetProperty(ref _operadorSelected, value); }
        }

        private string _numCelular;
        public string NumCelular
        {
            get { return _numCelular; }
            set { SetProperty(ref _numCelular, value); }
        }

        private string _monto;
        public string Monto
        {
            get { return _monto; }
            set { SetProperty(ref _monto, value); }
        }

        private string _lbTipoCambio;
        public string LbTipoCambio
        {
            get { return _lbTipoCambio; }
            set { SetProperty(ref _lbTipoCambio, value); }
        }

        private DelegateCommand _accionSiguienteIC;
        public DelegateCommand AccionSiguienteIC =>
            _accionSiguienteIC ?? (_accionSiguienteIC = new DelegateCommand(ExecuteAccionSiguienteIC));

        async void ExecuteAccionSiguienteIC()
        {
            try
            {
                string msj = ValidarCampos();
                if (msj != "")
                {
                    await DialogService.DisplayAlertAsync(Constantes.MSJ_VALIDACION, msj, Constantes.MSJ_BOTON_OK);
                }
                else
                {
                    NavigationParameters navParametros = GetNavigationParameters();

                    navParametros.Add("Monto", Monto);
                    navParametros.Add("NumCelular", NumCelular);
                    navParametros.Add("NomOperador", OperadorSelected.Nombre);
                    navParametros.Add("Operador", OperadorSelected);
                    navParametros.Add(Constantes.pageOrigen, Constantes.pageRecargaCelular);
                    navParametros.Add("Moneda", CatalogoService.BuscarMonedaPorCodigo("PEN"));

                    Application.Current.Properties["strTipoTransf"] = "0";
                    Application.Current.Properties["strOrigenMisCuentas"] = false;
                    Application.Current.Properties["strPageOrigen"] = Constantes.pageRecargaCelular;
                    await NavigationService.NavigateAsync(Constantes.pageCtaCargo, navParametros);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }            
        }

        public string ValidarCampos()
        {
            string msj = "";

            if (Monto == null || Monto == "")
            {
                msj = "Ingrese un monto válido";
            }

            return msj;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;

            string pageOrigen = RefNavParameters[Constantes.pageOrigen] as string;

            if(pageOrigen == Constantes.pageOperaciones)
            {
                OperacionFrecuente opeFrec = parameters["OperacionFrecuente"] as OperacionFrecuente;
                OperadorSelected = ListaOperadores.Where(p => p.Codigo == opeFrec.Picker1.Codigo).First();
                NumCelular = opeFrec.parametro1;
            }
        }

    }
}
