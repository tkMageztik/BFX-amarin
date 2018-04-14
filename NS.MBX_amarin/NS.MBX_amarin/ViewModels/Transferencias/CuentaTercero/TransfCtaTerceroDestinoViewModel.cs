using NS.MBX_amarin.BusinessLogic;
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
using System.Collections.Specialized;
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

            string strCuentaOrigen = ctaOrigen.CodigoCta;
            string strCuentaDestino = NumCtaDestino;
            string strMonedaCodMonto = MonedaSel.Codigo;
            double dblMonto = System.Math.Round(double.Parse(Monto), 2);
            string strDescrpcion = "";// Comunes.RemoverCaracteresEspeciales(txtDescripcion.Text.Trim()).ToUpper();
            //strDescrpcion = (strDescrpcion.ToString().Trim().Length == 0) ? "TRANSF.CTA." : strDescrpcion.ToString().Trim();

            //DataView dvCuentas = ((DataTable)ViewState["dtCuentas"]).DefaultView;
            //dvCuentas.RowFilter = "ODcodct='" + ddlCuentaOrigen.SelectedValue + "'";
            string strMonedaOrigen = ctaOrigen.idMoneda == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesMonto = strMonedaCodMonto == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesOrigen = strMonedaOrigen == "S/. " ? "PEN" : "USD";
            double dblMontoOrigen = 0.0;
            double dblMontoDestino = 0.0;
            double dblItf = 0.0;
            double dblComisiones = 0.0;
            double dblTotalDebitar = 0.0;
            double dblTipoCambio = 0.0;
            /* Inicio  Tipo Cambio Preferencial */
            StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(NumCtaDestino.ToString(), strCuentaOrigen, strMonedaCodMonto, dblMonto);
            double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambiopre["venta"].ToString()), 3);
            double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambiopre["compra"].ToString()), 3);
            /* Fin  Tipo Cambio Preferencial */
            //StringDictionary dsTipoCambio = ObtenerTipoCambio();
            //double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambio["venta"].ToString()), 3);
            //double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambio["compra"].ToString()), 3);

            if (strMonedaOrigen == strMonedaDesMonto)
                dblMontoOrigen = dblMonto;
            else
            {
                if (strMonedaOrigen == "S/. ")
                    dblMontoOrigen = System.Math.Round((dblMonto * dblCambioVenta), 2);
                else
                    dblMontoOrigen = System.Math.Round((dblMonto / dblCambioCompra), 2);
            }

            string strMonto = Convert.ToString(System.Math.Round(dblMontoOrigen, 2) * 100);
            string strMensaje = '*' + strCuentaDestino.PadLeft(12, '0') + strCuentaOrigen.PadLeft(12, '0') + strMonto.PadLeft(14, '0') + strMonedaDesOrigen + strDescrpcion.PadRight(30, ' ') + '*';
            //Agregado Tipo Cambio Preferencial
            //string strMontoReal = Convert.ToString(System.Math.Round(dblMonto, 2) * 100).PadLeft(14, '0');
            //string strMensaje = '%' + strCuentaDestino.PadLeft(12, '0') + strCuentaOrigen.PadLeft(12, '0') + strMonto.PadLeft(14, '0') + strMonedaDesOrigen + strDescrpcion.PadRight(30, ' ') + '%' + strMontoReal;
            TransaccionesMBX tx = new TransaccionesMBX();
            DataSet dsSalida = tx.EjecutarTransaccion(ListaTransacciones.TrasferenciaValidaCuentas, 155, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            //base.InsertaLogGeneral(TipoTransaccion.Consultas(), TipoOperacion.Consulta(), DefaultValues.TrasferenciaValidaCuentas, strMensaje, dsSalida, _strError);

            if (_strError != "0000")
            {
                dsSalida = null;
                try
                {
                    mensajeRpta = "Cuenta de origen se encuentra inactiva.";
                    //Mensaje1.MostrarFalla(ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)));
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                }
                catch (Exception)
                {
                    //Mensaje1.MostrarFalla("Error no manejado, intente otra vez!");
                   // MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(6666), ListImages.Error);
                }
            }

            string tlog = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0');
            //Agrego tipo cambio preferencial
            string strMontoReal = Convert.ToString(System.Math.Round(dblMonto, 2) * 100).PadLeft(14, '0');
            //strMensaje = '%' + strCuentaOrigen.PadRight(12, ' ') + '%' + tlog + '%' + strMonto.PadLeft(14, '0') + '%' + strCuentaDestino.PadRight(12, ' ') + '%' + strMonedaDesOrigen + strDescrpcion.PadRight(30, ' ') + '1';
            strMensaje = '%' + strCuentaOrigen.PadRight(12, ' ') + '%' + tlog + '%' + strMonto.PadLeft(14, '0') + '%' + strCuentaDestino.PadRight(12, ' ') + '%' + strMonedaDesOrigen + strDescrpcion.PadRight(30, ' ') + '1' + "".PadLeft(48, ' ') + strMontoReal;
            //
            DataSet dsOut = tx.EjecutarTransaccion(ListaTransacciones.TrasferenciaConsultaGastos, 250, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            if (_strError != "0000")
            {
                dsOut = null;
                dsSalida = null;
                try
                {
                    mensajeRpta = "La cuenta de origen no posee saldo suficiente.";
                    //Mensaje1.MostrarFalla(ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)));
                    // MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                }
                catch (Exception)
                {
                    //Mensaje1.MostrarFalla("Error no manejado, intente otra vez!");
                   // MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(6666), ListImages.Error);
                }
            }

            

            return mensajeRpta;
        }
    }
}
