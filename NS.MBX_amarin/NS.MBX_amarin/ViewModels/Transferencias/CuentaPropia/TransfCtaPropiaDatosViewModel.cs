using NS.MBX_amarin.BusinessLogic.Transacciones;
using NS.MBX_amarin.Common;
using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
    public class TransfCtaPropiaDatosViewModel : ViewModelBase
    {
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        public TransfCtaPropiaDatosViewModel(ICuentaService cuentaService, ITipoCambioService tipoCambioService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            CuentaService = cuentaService;
            CatalogoService = catalogoService;
            TipoCambioService = tipoCambioService;

        }

        //init
        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;

            ListaMonedas = CatalogoService.ListarMonedas();
            LblTipoCambio = TipoCambioService.ObtenerDescTipoCambio();
            Monto = null;
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

        private Catalogo _moneda;
        public Catalogo Moneda
        {
            get { return _moneda; }
            set { SetProperty(ref _moneda, value); }
        }

        private DelegateCommand _accionSigIC;
        public DelegateCommand AccionSigIC =>
            _accionSigIC ?? (_accionSigIC = new DelegateCommand(ExecuteAccionSigIC));

        async void ExecuteAccionSigIC()
        {
            try
            {
                string msj = ValidarCampos();
                if (msj != "")
                {
                    await DialogService.DisplayAlertAsync("Validación", msj, "OK");
                }
                else
                {
                    NavigationParameters navParametros = GetNavigationParameters();
                    navParametros.Add(Constantes.pageOrigen, Constantes.pageTransfCtaPropiaDatos);
                    navParametros.Add("Monto", Monto);
                    navParametros.Add("Moneda", Moneda);

                    await NavigationService.NavigateAsync(Constantes.pageTransfConfirmacion, navParametros);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string ValidarCampos()
        {
            string msj = "";
            Cuenta ctaOrigen = RefNavParameters["CtaCargo"] as Cuenta;

            if (string.IsNullOrEmpty(Monto))
            {
                msj = "Ingrese un monto válido";
            }
            else if(Moneda == null)
            {
                msj = "Ingrese moneda.";
            }
            else if(!CuentaService.ValidarSaldoOperacion(ctaOrigen, decimal.Parse(Monto), Moneda.Codigo))
            {
                msj = "La cuenta de origen no tiene saldo suficiente";
            }
            else
            {
                msj = ValidarCuentas();
            }

            return msj;
        }

        private string ValidarCuentas()
        {
            string _strError = "";
            string mensajeRpta = "";
            Cuenta ctaOrigen = RefNavParameters["CtaCargo"] as Cuenta;
            Cuenta ctaDestino = RefNavParameters["CtaDestino"] as Cuenta;

            double dblMonto = double.Parse(Monto);
            string strMonto = Convert.ToString(System.Math.Round(dblMonto, 2) * 100);
            string strMontoReal = Convert.ToString(System.Math.Round(dblMonto, 2) * 100).PadLeft(14, '0');

            string strMensaje = '%' + ctaDestino.CodigoCta.PadLeft(12, '0') + ctaOrigen.CodigoCta.PadLeft(12, '0') + strMonto.PadLeft(14, '0') + Moneda.Codigo + "".PadRight(30, ' ') + '%' + strMontoReal;
            Transacciones tx = new Transacciones();
            DataSet dsSalida = tx.EjecutarTransaccion(ListaTransacciones.TrasferenciaValidaCuentas, 155, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            //insertar log TODO

            if (_strError != "0000")
            {
                dsSalida = null;
                try
                {
                    mensajeRpta = "Cuenta de origen se encuentra inactiva.";
                }
                catch (Exception)
                {
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(6666), ListImages.Error);
                }
            }

            string tlog = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0');
            //Agregado  Tipo Cambio preferencial
            strMensaje = '%' + ctaOrigen.CodigoCta.PadRight(12, ' ') + '%' + tlog + '%' + strMonto.PadLeft(14, '0') + '%' + ctaDestino.CodigoCta.PadRight(12, ' ') + '%' + Moneda.Codigo + "".PadRight(30, ' ') + '1' + "".PadLeft(48, ' ') + strMontoReal + Moneda.Codigo.PadLeft(3, ' ');
            DataSet dsOut = tx.EjecutarTransaccion(ListaTransacciones.TrasferenciaConsultaGastos, 250, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            //DataSet dsHeader = tx.ObtenerCabecera(DefaultValues.TrasferenciaConsultaGastos, DefaultValues.NombreMensajeOut(), 0);
            if (_strError != "0000")
            {
                dsOut = null;
                dsSalida = null;
                try
                {
                    mensajeRpta = "La cuenta de origen no posee saldo suficiente.";
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                }
                catch (Exception)
                {
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(6666), ListImages.Error);
                }
                //return false;
            }

            return mensajeRpta;
        }
    }
}
