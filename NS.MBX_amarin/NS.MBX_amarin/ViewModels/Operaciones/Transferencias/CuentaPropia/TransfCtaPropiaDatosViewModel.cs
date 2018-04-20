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
    public class TransfCtaPropiaDatosViewModel : ViewModelBase
    {
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        private bool IsOperacionFrecuente;
        private Cuenta CtaCargo;
        private Cuenta CtaDestino;

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

            string pageOrigen = RefNavParameters[Constantes.pageOrigen] as string;

            ListaMonedas = CatalogoService.ListarMonedas();
            LblTipoCambio = TipoCambioService.ObtenerDescTipoCambio();
            Monto = null;
            IsOperacionFrecuente = false;


            //al final vemos si es ope frecuente
            if (pageOrigen == Constantes.pageOperaciones)//es una operacion frecuente
            {
                IsOperacionFrecuente = true;
                OperacionFrecuente opeFrec = RefNavParameters[Constantes.keyOperacionFrecuente] as OperacionFrecuente;
                //se agregan las referencias que deberian existir en este punto
                RefNavParameters.Add(Constantes.keyCtaCargo, opeFrec.CtaOrigen);
                RefNavParameters.Add(Constantes.keyCtaDestino, opeFrec.CtaDestino);
                Moneda = ListaMonedas.Where(p => p.Codigo == opeFrec.Moneda.Codigo).First();
            }
            
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
                    navParametros.Add(Constantes.keyMonto, Monto);
                    navParametros.Add(Constantes.keyMoneda, Moneda);

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
            Cuenta ctaOrigen = RefNavParameters[Constantes.keyCtaCargo] as Cuenta;

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
            Cuenta ctaOrigen = RefNavParameters[Constantes.keyCtaCargo] as Cuenta;
            Cuenta ctaDestino = RefNavParameters[Constantes.keyCtaDestino] as Cuenta;

            string strCuentaOrigen = ctaOrigen.CodigoCta;
            string strCuentaDestino = ctaDestino.CodigoCta;
            string strMonedaCodMonto = Moneda.Codigo;
            double dblMonto = System.Math.Round(double.Parse(Monto), 2);
            string strDescrpcion = "";// Comunes.RemoverCaracteresEspeciales(txtDescripcion.Text.Trim()).ToUpper();

            //DataView dvCuentas = ((DataTable)ViewState["dtCuentas"]).DefaultView;
            //dvCuentas.RowFilter = "ODcodct='" + ddlCuentaOrigen.SelectedValue + "'";
            string strMonedaOrigen = ctaOrigen.idMoneda == "PEN" ? "S/. " : "US$ ";
            //dvCuentas.RowFilter = "ODcodct='" + ddlCuentaDestino.SelectedValue + "'";
            string strMonedaDestino = ctaDestino.idMoneda == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesMonto = strMonedaCodMonto == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesOrigen = strMonedaOrigen == "S/. " ? "PEN" : "USD";
            double dblMontoOrigen = 0.0;
            double dblMontoDestino = 0.0;
            double dblItf = 0.0;
            double dblComisiones = 0.0;
            double dblTotalDebitar = 0.0;
            double dblTipoCambio = 0.0;

            if ((strMonedaOrigen == strMonedaDestino) && (strMonedaOrigen == strMonedaDesMonto))
                dblMontoDestino = dblMontoOrigen = dblMonto;
            else
            {
                /* Inicio  Tipo Cambio Preferencial */
                //StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(strMonedaOrigen == "S/. " ? "PEN" : "USD", strMonedaCodMonto, dblMonto);
                StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(strCuentaDestino.ToString(), strCuentaOrigen, strMonedaCodMonto, dblMonto);
                double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambiopre["venta"].ToString()), 3);
                double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambiopre["compra"].ToString()), 3);
                /* Fin  Tipo Cambio Preferencial */
                if (strMonedaOrigen == strMonedaDesMonto)
                {
                    dblMontoOrigen = dblMonto;
                    if (strMonedaOrigen == "S/. ")
                    {
                        dblMontoDestino = System.Math.Round((dblMonto / dblCambioVenta), 2);
                        dblTipoCambio = dblCambioVenta;
                    }
                    else
                    {
                        dblMontoDestino = System.Math.Round((dblMonto * dblCambioCompra), 2);
                        dblTipoCambio = dblCambioCompra;
                    }
                }
                else
                {
                    dblMontoDestino = dblMonto;
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
            }

            string strMonto = Convert.ToString(System.Math.Round(dblMontoOrigen, 2) * 100);
            //Agregado Tipo Cambio Preferencial
            string strMontoReal = Convert.ToString(System.Math.Round(dblMonto, 2) * 100).PadLeft(14, '0');
            string strMensaje = '%' + strCuentaDestino.PadLeft(12, '0') + strCuentaOrigen.PadLeft(12, '0') + strMonto.PadLeft(14, '0') + strMonedaDesOrigen + strDescrpcion.PadRight(30, ' ') + '%' + strMontoReal;
            //
            TransaccionesMBX tx = new TransaccionesMBX();
            DataSet dsSalida = tx.EjecutarTransaccion(ListaTransacciones.TrasferenciaValidaCuentas, 155, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            //base.InsertaLogGeneral(TipoTransaccion.Consultas(), TipoOperacion.Consulta(), DefaultValues.TrasferenciaValidaCuentas, strMensaje, dsSalida, _strError);

            if (_strError != "0000")
            {
                dsSalida = null;
                try
                {
                    mensajeRpta = "Cuenta de origen se encuentra inactiva.";
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                }
                catch (Exception)
                {
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(6666), ListImages.Error);
                }
            }

            string tlog = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0');
            //Agregado  Tipo Cambio preferencial
            strMensaje = '%' + strCuentaOrigen.PadRight(12, ' ') + '%' + tlog + '%' + strMonto.PadLeft(14, '0') + '%' + strCuentaDestino.PadRight(12, ' ') + '%' + strMonedaDesOrigen + strDescrpcion.PadRight(30, ' ') + '1' + "".PadLeft(48, ' ') + strMontoReal + strMonedaCodMonto.PadLeft(3, ' ');
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
            }

            return mensajeRpta;
        }
    }
}
