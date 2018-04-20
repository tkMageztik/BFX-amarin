using NS.MBX_amarin.BusinessLogic;
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
	public class PagoTCPropiaDatosViewModel : ViewModelBase
    {
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        public PagoTCPropiaDatosViewModel(ICuentaService cuentaService, ITipoCambioService tipoCambioService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
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

            string tipoPropTarjeta = RefNavParameters[Constantes.keyTipoPropTarjeta] as string;

            Tarjeta tarjeta = RefNavParameters[Constantes.keyTCPropia] as Tarjeta;
            LblMarcaTarjeta = tarjeta.MarcaTarjeta;
            LblNumTarjeta = tarjeta.NroTarjeta;

            LblPagoMinMes = RefNavParameters["TCPagoMinSol"] as string;
            LblPagoTotMes = RefNavParameters["TCPagoTotSol"] as string;

            ListaMonedas = CatalogoService.ListarMonedas();
            LblTipoCambio = TipoCambioService.ObtenerDescTipoCambio();
        }

        private string _lblPagoMinMes;
        public string LblPagoMinMes
        {
            get { return _lblPagoMinMes; }
            set { SetProperty(ref _lblPagoMinMes, value); }
        }

        private string _lblPagoTotMes;
        public string LblPagoTotMes
        {
            get { return _lblPagoTotMes; }
            set { SetProperty(ref _lblPagoTotMes, value); }
        }

        private string _lblMarcaTarjeta;
        public string LblMarcaTarjeta
        {
            get { return _lblMarcaTarjeta; }
            set { SetProperty(ref _lblMarcaTarjeta, value); }
        }

        private string _lblNumTarjeta;
        public string LblNumTarjeta
        {
            get { return _lblNumTarjeta; }
            set { SetProperty(ref _lblNumTarjeta, value); }
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
                    navParametros.Add(Constantes.pageOrigen, Constantes.pagePagoTCPropiaDatos);
                    navParametros.Add(Constantes.keyMonto, Monto);
                    navParametros.Add(Constantes.keyMoneda, Moneda);
                    navParametros.Add(Constantes.keyTCDestino, RefNavParameters[Constantes.keyTCPropia]);

                    await NavigationService.NavigateAsync(Constantes.pagePagoTCConfirmacion, navParametros);
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
            else if (Moneda == null)
            {
                msj = "Ingrese moneda.";
            }
            else if (!CuentaService.ValidarSaldoOperacion(ctaOrigen, decimal.Parse(Monto), Moneda.Codigo))
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
            Tarjeta tarjetaPropia = RefNavParameters[Constantes.keyTCPropia] as Tarjeta;

            string strCuentaOrigen = ctaOrigen.CodigoCta.PadLeft(12, '0');
            string strCuentaDestino = tarjetaPropia.NroTarjeta;
            string strMonedaCodMonto = Moneda.Codigo;
            double dblMonto = System.Math.Round(double.Parse(Monto), 2);
            string strDescrpcion = "";// Comunes.RemoverCaracteresEspeciales(txtDescripcion.Text.Trim()).ToUpper();

            //DataView dvCuentas = ((DataTable)ViewState["dtCuentas"]).DefaultView;
            //dvCuentas.RowFilter = "ODcodct='" + ddlCuentaOrigen.SelectedValue + "'";
            string strMonedaOrigen = ctaOrigen.idMoneda == "PEN" ? "S/. " : "US$ ";
            string strCodigoMoneda = ctaOrigen.idMoneda == "PEN" ? "01" : "02";
            string strMonedaDesMonto = strMonedaCodMonto == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesOrigen = strMonedaOrigen == "S/. " ? "PEN" : "USD";
            double dblMontoOrigen = 0.0;
            double dblItf = 0.0;
            double dblComisiones = 0.0;
            double dblTotalDebitar = 0.0;
            double dblTipoCambio = 0.0;
            TransaccionesMBX tx = new TransaccionesMBX();
            DataSet dsOut = new DataSet();
            try
            {

                if (strMonedaOrigen == strMonedaDesMonto)
                    dblMontoOrigen = dblMonto;
                else
                {
                    StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(strCuentaDestino.ToString(), strCuentaOrigen, strMonedaCodMonto, dblMonto);
                    double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambiopre["venta"].ToString()), 3);
                    double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambiopre["compra"].ToString()), 3);
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

                string strMonto = Convert.ToString(System.Math.Round(dblMontoOrigen, 2) * 100);

                string strCodLog = "0000000111";
                string strMensaje = '%' + ctaOrigen.CodigoCta.PadLeft(12, '0') + '%' + strCuentaDestino.Trim() + '%' + "   " + '%' + strCodigoMoneda + '%' + strCodLog + '%' + strMonto.PadLeft(14, '0') + '%' + strMonedaOrigen.Trim() + '%' + strDescrpcion.PadRight(30, ' ') + '1';
                dsOut = tx.EjecutarTransaccion(ListaTransacciones.PagoTarjetaBancoFinanciero, 181, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);

                if (_strError != "0000")
                {
                    try
                    {
                        mensajeRpta = "La cuenta de origen no tiene saldo suficiente. Código IBS:" + _strError;
                        //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                    }
                    catch (Exception)
                    {
                        //MostrarMensaje("", "Error no manejado, intente otra vez!", ListImages.Error);
                    }
                    //return false;
                }

                dblItf = (double)dsOut.Tables["OData"].Rows[0]["ODitfx"];
                dblComisiones = (double)dsOut.Tables["OData"].Rows[0]["ODcomx"];
                dblTotalDebitar = dblMontoOrigen + dblItf + dblComisiones;
                
                //Step3.ContentTemplateContainer.FindControl("tblStep3").Visible = true;
                //Step3.CustomNavigationTemplateContainer.FindControl("btnStep3Continuar").Visible = true;

                //((Label)Step3.ContentTemplateContainer.FindControl("lblDescripOrigen")).Text = DescripcionCuenta(ddlCuentaOrigen.SelectedItem.Text);
                //((Label)Step4.ContentTemplateContainer.FindControl("lblFecha")).Text = Convert.ToString(System.Math.Round(dblMontoOrigen, 2) * 100);
                //if (strMonedaOrigen == strMonedaDesMonto)
                //    ((Label)Step3.ContentTemplateContainer.FindControl("lblImporteOrigen")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblMontoOrigen.ToString());
                //else
                //    ((Label)Step3.ContentTemplateContainer.FindControl("lblImporteOrigen")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblMontoOrigen.ToString()) + " (" + strMonedaDesMonto + Comunes.FormateaNumero(dblMonto.ToString()) + " )";
                //((Label)Step3.ContentTemplateContainer.FindControl("lblTipoCambio")).Text = "S/. " + Comunes.FormateaNumero(dblTipoCambio.ToString());
                //((Label)Step3.ContentTemplateContainer.FindControl("lblComision")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblComisiones.ToString());
                //((Label)Step3.ContentTemplateContainer.FindControl("lblMontoDebito")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblTotalDebitar.ToString());
                //((Label)Step3.ContentTemplateContainer.FindControl("lblItf")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblItf.ToString());
                //((Label)Step3.ContentTemplateContainer.FindControl("lblDescripDestino")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblTarjeta")).Text;
                //HtmlTableRow trDatosTerceros3 = (HtmlTableRow)Step3.ContentTemplateContainer.FindControl("trDatosTerceros3");
                //if (rbtCuentaSi.Checked)
                //    trDatosTerceros3.Style.Add("display", "none");
                //else
                //{
                //    ((Label)Step3.ContentTemplateContainer.FindControl("lblTitular")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblDiaPago")).Text;
                //    trDatosTerceros3.Style.Remove("display");
                //}
                //((Label)Step3.ContentTemplateContainer.FindControl("lblImporteDestino")).Text = strMonedaDesMonto + Comunes.FormateaNumero(dblMonto.ToString());
                //((Label)Step3.ContentTemplateContainer.FindControl("lblDescripcion")).Text = strDescrpcion + "&nbsp";

                //return true;

            }
            catch (Exception ex)
            {
                //GenerarLogError(ex.Message, "ValidaCuenta() Exception");
                //return false;
            }
            finally
            {
                tx = null; dsOut = null;
                //rbtCuentaSi = null;
                //ddlTarjetaPropia = null;
                //ddlCuentaDestino = null;
                //ddlCuentaOrigen = null;
                //ddlMoneda = null;
                //txtMonto = null;
                //txtDescripcion = null;
            }

            return mensajeRpta;
        }
    }
}
