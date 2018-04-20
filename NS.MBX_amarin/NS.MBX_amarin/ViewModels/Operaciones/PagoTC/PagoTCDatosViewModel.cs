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
using System.Text.RegularExpressions;

//SIRVE PARA PAGO DE TERCEROS Y DE OTRO BANCO
namespace NS.MBX_amarin.ViewModels
{
    public class PagoTCDatosViewModel : ViewModelBase
    {
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        private string UltimoDiaPago;

        private string PagoMinSol;
        private string PagoTotSol;
        private string PagoDiaSol;
        private string PagoMinDol;
        private string PagoTotDol;
        private string PagoDiaDol;

        public PagoTCDatosViewModel(ICuentaService cuentaService, ITipoCambioService tipoCambioService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
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
            string origenTC = RefNavParameters[Constantes.keyOrigenTarjeta] as string;

            LblOrigenTarjeta = "Número de Tarjeta de Crédito";

            ListaMonedas = CatalogoService.ListarMonedas();
            LblTipoCambio = TipoCambioService.ObtenerDescTipoCambio();

            if (pageOrigen == Constantes.pageOperaciones)
            {
                OperacionFrecuente opeFrec = RefNavParameters[Constantes.keyOperacionFrecuente] as OperacionFrecuente;
                RefNavParameters.Add(Constantes.keyCtaCargo, opeFrec.CtaOrigen);
                Catalogo origenTarjeta = RefNavParameters[Constantes.keyOrigenTarjeta] as Catalogo;

                if(origenTarjeta.Codigo == "0")//tar de tercero
                {
                    RefNavParameters.Add(Constantes.keyTipoPropTarjeta, opeFrec.TipoPropTarjeta);
                }
                NumTarjeta = opeFrec.TcDestino.NroTarjeta;
                Moneda = ListaMonedas.Where(p => p.Codigo == opeFrec.Moneda.Codigo).First();
            }
        }

        private string _lblOrigenTarjeta;
        public string LblOrigenTarjeta
        {
            get { return _lblOrigenTarjeta; }
            set { SetProperty(ref _lblOrigenTarjeta, value); }
        }

        private string _lblNumTarjeta;
        public string LblNumTarjeta
        {
            get { return _lblNumTarjeta; }
            set { SetProperty(ref _lblNumTarjeta, value); }
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
                    Catalogo origenTarjeta = RefNavParameters[Constantes.keyOrigenTarjeta] as Catalogo;

                    NavigationParameters navParametros = GetNavigationParameters();
                    navParametros.Add(Constantes.pageOrigen, Constantes.pagePagoTCDatos);
                    navParametros.Add("Monto", Monto);
                    navParametros.Add("Moneda", Moneda);

                    Tarjeta tcDestino = new Tarjeta {NroTarjeta = NumTarjeta, NombreCliente = "Miguel Perez Ochoa", IsTarjetaCredito = true, IdEstado = 1 };
                    if(origenTarjeta.Codigo == "0")
                    {
                        tcDestino.MarcaTarjeta = "Tarjeta de Crédito Propio Banco";
                    }
                    else
                    {
                        tcDestino.MarcaTarjeta = "BCP Banco de Crédito del Perú";
                    }

                    navParametros.Add(Constantes.keyTCDestino, tcDestino);

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
            Cuenta ctaOrigen = RefNavParameters[Constantes.keyCtaCargo] as Cuenta;
            Catalogo origenTarjeta = RefNavParameters[Constantes.keyOrigenTarjeta] as Catalogo;

            if (string.IsNullOrEmpty(Monto))
            {
                msj = "Ingrese un monto válido";
            }
            else if (string.IsNullOrEmpty(NumTarjeta))
            {
                msj = "Ingrese número de tarjeta de crédito.";
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
                if (origenTarjeta.Codigo == "0")//cuenta propio banco
                {
                    msj = ValidarCuentasPropioBancoTercero();
                }
                else//cuenta otro banco
                {
                    msj = ValidarCuentasOtroBanco();
                }

            }

            return msj;
        }

        private string ValidarCuentasPropioBancoTercero()
        {
            string _strError = "";
            string mensajeRpta = "";

            Cuenta ctaOrigen = RefNavParameters[Constantes.keyCtaCargo] as Cuenta;
            Cuenta ctaDestino = RefNavParameters[Constantes.keyCtaDestino] as Cuenta;

            string strCuentaOrigen = ctaOrigen.CodigoCta.PadLeft(12, '0');
            string strCuentaDestino = NumTarjeta;
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

        private string ValidarCuentasOtroBanco()
        {
            string _strError = "";
            string mensajeRpta = "";

            string strNumeroCuentaDestino = string.Empty;
            Cuenta ctaOrigen = RefNavParameters[Constantes.keyCtaCargo] as Cuenta;
            //ComboBoxText ddlCuentaDestino = (ComboBoxText)Step1.ContentTemplateContainer.FindControl("ddlCuentaDestino");

            //if (!Comunes.ValidaModulo10(ddlCuentaDestino.Text))
            //{
            //    try
            //    {
            //        MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(3011), ListImages.Error);
            //    }
            //    catch (Exception)
            //    {
            //        MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(6666), ListImages.Error);
            //    }
            //    return false;
            //}
            strNumeroCuentaDestino = NumTarjeta;// ddlCuentaDestino.Text;

            Regex exp = new Regex("^[0-9]+$");
            //if (!exp.IsMatch(strNumeroCuentaDestino))
            //{
            //    MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(3011), ListImages.Error);
            //    return false;
            //}


            //DropDownList ddlCuentaOrigen = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlCuentaOrigen");
            //DropDownList ddlMoneda = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlMoneda");
            //TextBox txtMonto = (TextBox)Step1.ContentTemplateContainer.FindControl("txtMonto");
            //RadioButton rbtCuentaSi = (RadioButton)Step1.ContentTemplateContainer.FindControl("rbtCuentaSi");
            //DropDownList ddlBanco = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlBanco");
            //TextBox txtDescripcion = (TextBox)Step1.ContentTemplateContainer.FindControl("txtDescripcion");
            //DropDownList ddlTipoDocumento = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlTipoDocumento");
            //TextBox txtNumeroDocumentoBeneficiario = (TextBox)Step1.ContentTemplateContainer.FindControl("txtNumeroDocumentoBeneficiario");
            //TextBox txtNombres = (TextBox)Step1.ContentTemplateContainer.FindControl("txtNombres");
            //TextBox txtApellidos = (TextBox)Step1.ContentTemplateContainer.FindControl("txtApellidos");
            //TextBox txtRazonSocial = (TextBox)Step1.ContentTemplateContainer.FindControl("txtRazonSocial");
            //TextBox txtDireccion = (TextBox)Step1.ContentTemplateContainer.FindControl("txtDireccion");
            //HiddenField hfbines = (HiddenField)Step1.ContentTemplateContainer.FindControl("hf_bines");

            string txtNombres = "";// Comunes.RemoverCaracteresEspeciales(txtNombres.Text.Trim()).ToUpper();
            string txtApellidos = "";//Comunes.RemoverCaracteresEspeciales(txtApellidos.Text.Trim()).ToUpper();
            string txtRazonSocial = "";//Comunes.RemoverCaracteresEspeciales(txtRazonSocial.Text.Trim()).ToUpper();
            string txtDireccion = "";//Comunes.RemoverCaracteresEspeciales(txtDireccion.Text.Trim()).ToUpper();
            string txtNumeroDocumentoBeneficiario = "";//Comunes.RemoverCaracteresEspeciales(txtNumeroDocumentoBeneficiario.Text.Trim()).ToUpper();

            string strCuentaOrigen = ctaOrigen.CodigoCta.PadLeft(12, '0');
            string strCuentaDestino = NumTarjeta;
            string strMonedaCodMonto = Moneda.Codigo;
            double dblMonto = System.Math.Round(double.Parse(Monto), 2);
            string strDescrpcion = Comunes.RemoverCaracteresEspeciales("").ToUpper();
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
            double douMontoCuenta = 0.00;
            try
            {
                //douMontoCuenta = dvCuentas[0]["ODsigno"].ToString().Trim() == "-" ? -1 * double.Parse(dvCuentas[0]["ODmonto"].ToString()) : double.Parse(dvCuentas[0]["ODmonto"].ToString());
            }
            catch (Exception ex)
            {

            }

            //if (ddlTipoDocumento.SelectedValue.Trim().Equals("6"))
            //    strNombres = txtRazonSocial.Text.Trim();
            //else
            //{
            //    if (string.IsNullOrEmpty(txtApellidos.Text.Trim()) && string.IsNullOrEmpty(txtNombres.Text.Trim()))
            //    {
            //        strNombres = string.Empty;
            //    }
            //    else
            //    {
            //        strNombres = txtApellidos.Text.Trim() + "-" + txtNombres.Text.Trim();
            //    }

            //}
            if (strMonedaOrigen == strMonedaDesMonto)
                dblMontoOrigen = dblMonto;
            else
            {
                string strCuentaDestinoCCI = string.Empty;
                //if (string.IsNullOrEmpty(hfbines.Value))
                //{
                //    strCuentaDestinoCCI = string.Concat(strCuentaDestino.ToString().Trim());
                //}
                //else
                //{
                    if (strCuentaDestino.ToString().Length == 15)
                    {
                        strCuentaDestinoCCI = string.Concat(strCuentaDestino.ToString().Trim().PadLeft(15, '0'));
                    }
                    if (strCuentaDestino.ToString().Length == 16)
                    {
                        strCuentaDestinoCCI = string.Concat(strCuentaDestino.ToString().Trim().PadLeft(16, '0'));
                    }
                //}

                StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(strCuentaDestinoCCI.ToString(), strCuentaOrigen, strMonedaCodMonto, dblMonto);
                double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambiopre["venta"].ToString()), 3);
                double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambiopre["compra"].ToString()), 3);

                //StringDictionary dsTipoCambio = ObtenerTipoCambio();
                //double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambio["venta"].ToString()), 3);
                //double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambio["compra"].ToString()), 3);
                if (strMonedaOrigen == "S/. ")
                {
                    dblMontoOrigen = System.Math.Round((dblMonto * dblCambioVenta), 3);
                    dblTipoCambio = dblCambioVenta;
                }
                else
                {
                    dblMontoOrigen = System.Math.Round((dblMonto / dblCambioCompra), 3);
                    dblTipoCambio = dblCambioCompra;
                }
            }

            if ((douMontoCuenta - dblMontoOrigen) < 0)
            {
                try
                {
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse("2037")), ListImages.Error);
                }
                catch (Exception)
                {
                    //Mensaje1.MostrarFalla("Error no manejado, intente otra vez!");
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse("6666")), ListImages.Error);
                }
                //return false;
            }

            string strMonto = Convert.ToString(System.Math.Round(dblMontoOrigen, 2, MidpointRounding.AwayFromZero) * 100);

            TransaccionesMBX tx = new TransaccionesMBX();
            if (strCuentaDestino.ToString().Length == 15)
            {
                strCuentaDestino = strCuentaDestino.ToString().Trim().PadLeft(16, '0');
            }
            string tlog = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0');
            //string strMensaje = strCuentaOrigen.PadRight(12, ' ') + tlog + strMonto.PadLeft(14, '0') + strMonedaDesOrigen + ddlBanco.SelectedValue.Trim() + strCuentaDestino.PadRight(17, ' ') + ddlTipoDocumento.SelectedValue.Trim() + txtNumeroDocumentoBeneficiario.Text.Trim().PadLeft(12, '0') + strNombres.PadRight(45, ' ') + txtDireccion.Text.Trim().PadRight(65, ' ') + "0000000000" + (rbtCuentaSi.Checked ? '1' : '0') + '0' + strDescrpcion.PadRight(30, ' ') + (rbtCuentaSi.Checked ? '1' : '0') + '1'; // rc consultar si existe pago a proveedor (chkPagoProveedores.Checked ? "1" : "0");
            string strMensaje = strCuentaOrigen.PadRight(12, ' ') + tlog + strMonto.PadLeft(14, '0') + strMonedaCodMonto + "".Trim() + strCuentaDestino.PadRight(17, ' ') + "".Trim() + "".Trim().PadLeft(12, '0') + strNombres.PadRight(45, ' ') + "".Trim().PadRight(65, ' ') + "0000000000" + ('0') + '0' + strDescrpcion.PadRight(30, ' ') + ( '0') + '1'; // rc consultar si existe pago a proveedor (chkPagoProveedores.Checked ? "1" : "0");

            DataSet dsOut = tx.EjecutarTransaccion(ListaTransacciones.PagoTarjetaOtroBanco, 308, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            //DataSet dsHeader = tx.ObtenerCabecera(DefaultValues.PagoTarjetaOtroBanco, DefaultValues.NombreMensajeOut(), 0);
            //if (dsOut != null && dsHeader != null)
            //    base.InsertaLogTransferencias(TipoTransaccion.Transferencias(), TipoOperacion.Transaccion(), DefaultValues.TRANS_FONDOS_PROPIAS(), strMensaje, dsOut, _strError, dsHeader.Tables["OHead"].Rows[0]["OHlogtx"].ToString(), ddlCuentaOrigen.SelectedValue.Substring(4, 12), ddlCuentaOrigen.SelectedItem.Text + ' ' + dsOut.Tables["OData"].Rows[0]["ODclifr"].ToString(), dsOut.Tables["OData"].Rows[0]["ODmtoor"].ToString(), strMonedaOrigen, ddlCuentaDestino.SelectedValue.Substring(4, 12), ddlCuentaDestino.SelectedItem.Text + dsOut.Tables["OData"].Rows[0]["ODclito"].ToString(), dsOut.Tables["OData"].Rows[0]["ODmtocv"].ToString(), strMonedaDestino, string.Empty, string.Empty);

            if (_strError != "0000")
            {
                try
                {
                    mensajeRpta = "La tarjeta no posee deuda pendiente. Error IBS: " + _strError;
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                }
                catch (Exception)
                {
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(6666), ListImages.Error);
                }
                //return false;
            }

            dblItf = (double)dsOut.Tables["OData"].Rows[0]["ODItf"];
            dblComisiones = (double)dsOut.Tables["OData"].Rows[0]["ODComix"];
            dblTotalDebitar = (double)dsOut.Tables["OData"].Rows[0]["ODttoor"];
            //Step2.ContentTemplateContainer.FindControl("tblStep2").Visible = true;
            //Step2.CustomNavigationTemplateContainer.FindControl("btnStep2Continuar").Visible = true;

        //    ((Label)Step2.ContentTemplateContainer.FindControl("lblDescripOrigen")).Text = DescripcionCuenta(ddlCuentaOrigen.SelectedItem.Text);
        //    ((Label)Step3.ContentTemplateContainer.FindControl("lblFecha")).Text = Convert.ToString(System.Math.Round(dblMontoOrigen, 2) * 100);
        //    if (strMonedaOrigen == strMonedaDesMonto)
        //        ((Label)Step2.ContentTemplateContainer.FindControl("lblImporteOrigen")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblMontoOrigen.ToString());
        //    else
        //        ((Label)Step2.ContentTemplateContainer.FindControl("lblImporteOrigen")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblMontoOrigen.ToString()) + " (" + strMonedaDesMonto + Comunes.FormateaNumero(dblMonto.ToString()) + " )";
        //    ((Label)Step2.ContentTemplateContainer.FindControl("lblTipoCambio")).Text = "S/. " + Comunes.FormateaNumero(dblTipoCambio.ToString());
        //    ((Label)Step2.ContentTemplateContainer.FindControl("lblComision")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblComisiones.ToString());
        //    ((Label)Step2.ContentTemplateContainer.FindControl("lblMontoDebito")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblTotalDebitar.ToString());
        //    ((Label)Step2.ContentTemplateContainer.FindControl("lblItf")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblItf.ToString());
        //    //((Label)Step2.ContentTemplateContainer.FindControl("lblDescripDestino")).Text = ddlCuentaDestino.Text + " (" + ddlBanco.SelectedItem.Text + ")";
        //    if (hfbines.Value.Contains(ddlCuentaDestino.Text.Substring(0, 6)))
        //    {
        //        ((Label)Step2.ContentTemplateContainer.FindControl("lblDescripDestino")).Text = ddlCuentaDestino.Text + " (" + ddlBanco.SelectedItem.Text + " - American Express)";
        //    }
        //    else
        //    {
        //        ((Label)Step2.ContentTemplateContainer.FindControl("lblDescripDestino")).Text = ddlCuentaDestino.Text + " (" + ddlBanco.SelectedItem.Text + ")";
        //    }
        //((Label)Step2.ContentTemplateContainer.FindControl("lblCuentaPropia")).Text = rbtCuentaSi.Checked ? "Si" : "No";
        //    HtmlTableRow trDatosTercerosPaso2_1 = (HtmlTableRow)Step2.ContentTemplateContainer.FindControl("trDatosTercerosPaso2_1");
        //    HtmlTableRow trDatosTercerosPaso2_2 = (HtmlTableRow)Step2.ContentTemplateContainer.FindControl("trDatosTercerosPaso2_2");
        //    HtmlTableRow trDatosTercerosPaso2_3 = (HtmlTableRow)Step2.ContentTemplateContainer.FindControl("trDatosTercerosPaso2_3");

        //    if (rbtCuentaSi.Checked)
        //    {
        //        trDatosTercerosPaso2_1.Style.Add("display", "none");
        //        trDatosTercerosPaso2_2.Style.Add("display", "none");
        //        trDatosTercerosPaso2_3.Style.Add("display", "none");
        //    }
        //    else
        //    {
        //        if (ddlTipoDocumento.SelectedValue.Trim().Equals("6"))
        //            ((Label)Step2.ContentTemplateContainer.FindControl("lblEtiquetaNombre")).Text = "Razón social";

        //        else
        //            ((Label)Step2.ContentTemplateContainer.FindControl("lblEtiquetaNombre")).Text = "Nombres y apellidos";
        //        ((Label)Step2.ContentTemplateContainer.FindControl("lblNombre")).Text = txtRazonSocial.Text == "" ? txtNombres.Text.Trim() + " " + txtApellidos.Text.Trim() : txtRazonSocial.Text;
        //        ((Label)Step2.ContentTemplateContainer.FindControl("lblDocumento")).Text = ddlTipoDocumento.SelectedItem.Text + " " + txtNumeroDocumentoBeneficiario.Text.Trim();
        //        ((Label)Step2.ContentTemplateContainer.FindControl("lblDirección")).Text = txtDireccion.Text.Trim();
        //        trDatosTercerosPaso2_1.Style.Remove("display");
        //        trDatosTercerosPaso2_2.Style.Remove("display");
        //    }
        //((Label)Step2.ContentTemplateContainer.FindControl("lblImporteDestino")).Text = strMonedaDesMonto + Comunes.FormateaNumero(dblMonto.ToString());
        //    ((Label)Step2.ContentTemplateContainer.FindControl("lblDescripcion")).Text = strDescrpcion + "&nbsp";


            return mensajeRpta;
        }
    }
}
