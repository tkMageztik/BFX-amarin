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
	public class TransfCtaOtroBancoDestinoViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        public TransfCtaOtroBancoDestinoViewModel(ICuentaService cuentaService, ITipoCambioService tipoCambioService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
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
            Monto = null;
        }

        private ObservableCollection<Catalogo> _listaMonedas;
        public ObservableCollection<Catalogo> ListaMonedas
        {
            get { return _listaMonedas; }
            set { SetProperty(ref _listaMonedas, value); }
        }

        private Catalogo _monedaSel;
        public Catalogo MonedaSel
        {
            get { return _monedaSel; }
            set { SetProperty(ref _monedaSel, value); }
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

        private string _numCtaDestino;
        public string NumCtaDestino
        {
            get { return _numCtaDestino; }
            set { SetProperty(ref _numCtaDestino, value); }
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
                    navParametros.Add(Constantes.pageOrigen, Constantes.pageTransfCtaOtroBancoDestino);
                    navParametros.Add("Monto", Monto);
                    navParametros.Add("Moneda", MonedaSel);

                    //crear cuenta destion ficticia
                    Cuenta ctaDestino = new Cuenta();
                    ctaDestino.NombreTitular = "BCP Banco de Crédito del Perú";
                    ctaDestino.CodigoCta = NumCtaDestino;

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
                msj = "Ingrese un número CCI válido";
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
            string _strError = null;
            string mensajeRpta = "";

            Cuenta ctaOrigen = RefNavParameters["CtaOrigen"] as Cuenta;

            string strCuentaOrigen = ctaOrigen.CodigoCta;
            string strCuentaDestino = NumCtaDestino;
            string strMonedaCodMonto = MonedaSel.Codigo;
            double dblMonto = System.Math.Round(double.Parse(Monto), 2);
            string strDescrpcion = "";//Comunes.RemoverCaracteresEspeciales(txtDescripcion.Text.Trim()).ToUpper();
            //strDescrpcion = (strDescrpcion.ToString().Trim().Length == 0) ? "TRANSF.CCI." : strDescrpcion.ToString().Trim();

            //DataView dvCuentas = ((DataTable)ViewState["dtCuentas"]).DefaultView;
            //dvCuentas.RowFilter = "ODcodct='" + ddlCuentaOrigen.SelectedValue + "'";
            string strMonedaOrigen = ctaOrigen.idMoneda == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesMonto = strMonedaCodMonto == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesOrigen = strMonedaOrigen == "S/. " ? "PEN" : "USD";
            double dblMontoOrigen = 0.0;
            double dblItf = 0.0;
            double dblComisiones = 0.0;
            double dblTotalDebitar = 0.0;
            double dblTipoCambio = 0.0;
            string strNombres = string.Empty;

            //if (ddlTipoDocumento.SelectedValue.Trim().Equals("6"))
            //    strNombres = txtRazonSocial.Text.Trim();
            //else
            //    strNombres = txtApellidos.Text.Trim() + "-" + txtNombres.Text.Trim();

            if (strMonedaOrigen == strMonedaDesMonto)
                dblMontoOrigen = dblMonto;
            else
            {

                StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(strCuentaDestino.ToString(), strCuentaOrigen, strMonedaCodMonto, dblMonto);
                double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambiopre["venta"].ToString()), 3);
                double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambiopre["compra"].ToString()), 3);
                //StringDictionary dsTipoCambio = ObtenerTipoCambio();
                //double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambio["venta"].ToString()), 3);
                //double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambio["compra"].ToString()), 3);
                if (strMonedaOrigen == "S/. ")
                {
                    dblMontoOrigen = System.Math.Round((dblMonto * dblCambioVenta), 2);
                    dblTipoCambio = dblCambioVenta;
                }
                else
                {
                    dblMontoOrigen = System.Math.Round((dblMonto / dblCambioCompra), 2);
                    dblTipoCambio = dblCambioCompra;
                }
            }
            string strMontoOriginal = Convert.ToString(System.Math.Round(dblMonto, 2) * 100);
            string strMonto = Convert.ToString(System.Math.Round(dblMontoOrigen, 2) * 100);

            TransaccionesMBX tx = new TransaccionesMBX();
            string tlog = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0');
            //string strMensaje = strCuentaOrigen.PadRight(12, ' ') + tlog + strMonto.PadLeft(14, '0') + strMonedaCodMonto + strCuentaDestino.PadRight(20, '0') + ddlTipoDocumento.SelectedValue.Trim() + txtNumeroDocumentoBeneficiario.Text.Trim().PadLeft(12, '0') + strNombres.PadRight(45, ' ') + txtDireccion.Text.Trim().PadRight(65, ' ') + "0000000000" + (rbtCuentaSi.Checked ? '1' : '0') + '0' + strDescrpcion.PadRight(30, ' ') + (rbtCuentaSi.Checked ? '1' : '0') + '1' + strMontoOriginal.PadLeft(14, '0');
            string strMensaje = strCuentaOrigen.PadRight(12, ' ') + tlog + strMonto.PadLeft(14, '0') + strMonedaCodMonto + strCuentaDestino.PadRight(20, '0') + "***" + "*".Trim().PadLeft(12, '0') + strNombres.PadRight(45, ' ') + "*".Trim().PadRight(65, ' ') + "0000000000" + ( '0') + '0' + strDescrpcion.PadRight(30, ' ') + ('0') + '1' + strMontoOriginal.PadLeft(14, '0');

            DataSet dsOut = tx.EjecutarTransaccion(ListaTransacciones.TrasferenciaOtroBancoConsultaGastos, 330, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            //DataSet dsHeader = tx.ObtenerCabecera(DefaultValues.TrasferenciaOtroBancoConsultaGastos, DefaultValues.NombreMensajeOut(), 0);
            //if (dsOut != null && dsHeader != null)
            //    base.InsertaLogTransferencias(TipoTransaccion.Transferencias(), TipoOperacion.Transaccion(), DefaultValues.TRANS_FONDOS_PROPIAS(), strMensaje, dsOut, _strError, dsHeader.Tables["OHead"].Rows[0]["OHlogtx"].ToString(), ddlCuentaOrigen.SelectedValue.Substring(4, 12), ddlCuentaOrigen.SelectedItem.Text + ' ' + dsOut.Tables["OData"].Rows[0]["ODclifr"].ToString(), dsOut.Tables["OData"].Rows[0]["ODmtoor"].ToString(), strMonedaOrigen, ddlCuentaDestino.SelectedValue.Substring(4, 12), ddlCuentaDestino.SelectedItem.Text + dsOut.Tables["OData"].Rows[0]["ODclito"].ToString(), dsOut.Tables["OData"].Rows[0]["ODmtocv"].ToString(), strMonedaDestino, string.Empty, string.Empty);

            if (_strError != "0000")
            {
                try
                {
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                    mensajeRpta = "La cuenta destino no existe o no está activa.";
                }
                catch (Exception)
                {
                   // MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse("6666")), ListImages.Error);
                }
            }

            return mensajeRpta;
        }

    }
}
