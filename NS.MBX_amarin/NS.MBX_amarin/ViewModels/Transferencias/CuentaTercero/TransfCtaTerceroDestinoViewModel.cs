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
	public class TransfCtaTerceroDestinoViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        public TransfCtaTerceroDestinoViewModel(ICuentaService cuentaService, ITipoCambioService tipoCambioService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
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

            Cuenta ctaOrigen = parameters["CtaOrigen"] as Cuenta;

            ListaMonedas = CatalogoService.ListarMonedas();
            LblTipoCambio = TipoCambioService.ObtenerDescTipoCambio();
            ListaTiposCta = CatalogoService.ObtenerListaPorCodigo(CatalogoService.COD_TIPOS_CTA);
            LblDesde = "Desde: " + ctaOrigen.NombreCta;
            Monto = null;
        }

        private string _lblDesde;
        public string LblDesde
        {
            get { return _lblDesde; }
            set { SetProperty(ref _lblDesde, value); }
        }

        private string _numCtaDestino;
        public string NumCtaDestino
        {
            get { return _numCtaDestino; }
            set { SetProperty(ref _numCtaDestino, value); }
        }

        private ObservableCollection<Catalogo> _listaTiposCta;
        public ObservableCollection<Catalogo> ListaTiposCta
        {
            get { return _listaTiposCta; }
            set { SetProperty(ref _listaTiposCta, value); }
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

        private Catalogo _monedaSel;
        public Catalogo MonedaSel
        {
            get { return _monedaSel; }
            set { SetProperty(ref _monedaSel, value); }
        }

        private Catalogo _tipoCtaSel;
        public Catalogo TipoCtaSel
        {
            get { return _tipoCtaSel; }
            set { SetProperty(ref _tipoCtaSel, value); }
        }

        private DelegateCommand _accionConfirmarIC;
        public DelegateCommand AccionConfirmarIC =>
            _accionConfirmarIC ?? (_accionConfirmarIC = new DelegateCommand(ExecuteAccionConfirmarIC));

        async void ExecuteAccionConfirmarIC()
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
                    navParametros.Add(Constantes.pageOrigen, Constantes.pageTransfCtaTerceroDestino);
                    navParametros.Add("Monto", Monto);
                    navParametros.Add("Moneda", MonedaSel);

                    //crear cuenta destion ficticia
                    Cuenta ctaDestino = new Cuenta();
                    ctaDestino.NombreTitular = "Juan Manuel Rodriguez";
                    ctaDestino.CodigoCta = NumCtaDestino;
                    ctaDestino.idMoneda = TipoCtaSel.Descripcion;//idmoneda

                    navParametros.Add("CtaDestino", ctaDestino);

                    await NavigationService.NavigateAsync(Constantes.pageTransfConfirmacion, navParametros);
                }
            }
            catch (Exception ex)
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
            else if (string.IsNullOrEmpty(NumCtaDestino))
            {
                msj = "Ingrese un número de cuenta destino válido";
            }
            else if (TipoCtaSel == null)
            {
                msj = "Ingrese tipo de cuenta destino.";
            }
            else if (MonedaSel == null)
            {
                msj = "Ingrese moneda.";
            }
            else if (!CuentaService.ValidarSaldoOperacion(ctaOrigen, decimal.Parse(Monto), MonedaSel.Codigo))
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
            //Cuenta ctaDestino = RefNavParameters["CtaDestino"] as Cuenta;

            double dblMonto = double.Parse(Monto);
            string strMonto = Convert.ToString(System.Math.Round(dblMonto, 2) * 100);
            string strMontoReal = Convert.ToString(System.Math.Round(dblMonto, 2) * 100).PadLeft(14, '0');

            string strMensaje = '*' + NumCtaDestino.PadLeft(12, '0') + ctaOrigen.CodigoCta.PadLeft(12, '0') + strMonto.PadLeft(14, '0') + MonedaSel.Codigo + "".PadRight(30, ' ') + '*';
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
            //Agrego tipo cambio preferencial
            strMontoReal = Convert.ToString(System.Math.Round(dblMonto, 2) * 100).PadLeft(14, '0');
            //strMensaje = '%' + strCuentaOrigen.PadRight(12, ' ') + '%' + tlog + '%' + strMonto.PadLeft(14, '0') + '%' + strCuentaDestino.PadRight(12, ' ') + '%' + strMonedaDesOrigen + strDescrpcion.PadRight(30, ' ') + '1';
            strMensaje = '%' + ctaOrigen.CodigoCta.PadRight(12, ' ') + '%' + tlog + '%' + strMonto.PadLeft(14, '0') + '%' + NumCtaDestino.PadRight(12, ' ') + '%' + MonedaSel.Codigo + "".PadRight(30, ' ') + '1' + "".PadLeft(48, ' ') + strMontoReal;

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
