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

namespace NS.MBX_amarin.ViewModels
{
	public class DatosPagoTarjetaViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }
        public NavigationParameters NavParametros { get; set; }

        public DatosPagoTarjetaViewModel(ITipoCambioService tipoCambioService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            CatalogoService = catalogoService;
            TipoCambioService = tipoCambioService;

            ListaMonedas = CatalogoService.ListarMonedas();
            LblTipoCambio = TipoCambioService.ObtenerDescTipoCambio();
            Monto = null;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            NavParametros = parameters;
            string pageOrigen = parameters[Constantes.pageOrigen] as string;
            string codTipoTarjeta = "";

            if (pageOrigen == Constantes.pageOperaciones)//operacion frecuente
            {
                OperacionFrecuente opeFrec = parameters["OperacionFrecuente"] as OperacionFrecuente;
                codTipoTarjeta = opeFrec.Picker1.Codigo;
                NumTarjeta = opeFrec.parametro1;
                Moneda = ListaMonedas.Where(p => p.Codigo == opeFrec.Picker2.Codigo).First();
            }
            else
            {
                codTipoTarjeta = parameters["CodTipoTarjeta"] as string;
            }
            
            if(codTipoTarjeta == "2")
            {
                LblTipoTarjeta = "Número de tarjeta Diners";
            }
            else
            {
                LblTipoTarjeta = "Número de tarjeta de crédito";
            }
            
        }

        private string _lblTipoTarjeta;
        public string LblTipoTarjeta
        {
            get { return _lblTipoTarjeta; }
            set { SetProperty(ref _lblTipoTarjeta, value); }
        }

        private string _numTarjeta;
        public string NumTarjeta
        {
            get { return _numTarjeta; }
            set { SetProperty(ref _numTarjeta, value); }
        }

        private ObservableCollection<Catalogo> _listaMonedas;
        public ObservableCollection<Catalogo> ListaMonedas
        {
            get { return _listaMonedas; }
            set { SetProperty(ref _listaMonedas, value); }
        }

        private string _monto;
        public string Monto
        {
            get { return _monto; }
            set { SetProperty(ref _monto, value); }
        }

        private string _lblTipoCambio;
        public string LblTipoCambio
        {
            get { return _lblTipoCambio; }
            set { SetProperty(ref _lblTipoCambio, value); }
        }

        private DelegateCommand _accionSiguienteIC;
        public DelegateCommand AccionSiguienteIC =>
            _accionSiguienteIC ?? (_accionSiguienteIC = new DelegateCommand(ExecuteAccionSiguienteIC));

        private Catalogo _moneda;
        public Catalogo Moneda
        {
            get { return _moneda; }
            set { SetProperty(ref _moneda, value); }
        }

        async void ExecuteAccionSiguienteIC()
        {
            string msj = ValidarCampos();
            if (msj != "")
            {
                await DialogService.DisplayAlertAsync("Validación", msj, "OK");
            }
            else
            {
                NavParametros.Add("Monto", Monto);
                NavParametros.Add("NumTarjeta", NumTarjeta);
                NavParametros.Add("Moneda", Moneda);
                NavParametros.Add(Constantes.pageOrigen, Constantes.pageDatosPagoTarjeta);

                await NavigationService.NavigateAsync(Constantes.pageConfDatosPago, NavParametros);
            }
        }

        public string ValidarCampos()
        {
            string msj = "";

            if(Monto == null || Monto == "")
            {
                msj = "Ingrese un monto válido";
            }

            return msj;
        }

    }
}
