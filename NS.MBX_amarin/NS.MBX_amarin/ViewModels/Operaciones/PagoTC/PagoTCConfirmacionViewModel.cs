using NS.MBX_amarin.BusinessLogic;
using NS.MBX_amarin.Common;
using NS.MBX_amarin.Events;
using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModels
{
	public class PagoTCConfirmacionViewModel : ViewModelBase
    {
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private IOperacionService OperacionService { get; set; }
        private IEventAggregator EventAggregator { get; set; }

        public PagoTCConfirmacionViewModel(IOperacionService operacionService, ICuentaService cuentaService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService, IEventAggregator eventAggregator)
            : base(navigationService, dialogService)
        {
            this.CatalogoService = catalogoService;
            this.CuentaService = cuentaService;
            this.EventAggregator = eventAggregator;
            this.OperacionService = operacionService;
        }

        public override void OnNavigatingTo(NavigationParameters parametros)
        {
            RefNavParameters = parametros;

            string pageOrigen = parametros[Constantes.pageOrigen] as string;
            Cuenta ctaOrigen = parametros[Constantes.keyCtaCargo] as Cuenta;
            Tarjeta tarDestino = parametros[Constantes.keyTCDestino] as Tarjeta;//cuando el objeto no existe, esto devuelve null
            Catalogo origenTarjeta = parametros[Constantes.keyOrigenTarjeta] as Catalogo;

            LblNombreCta1 = ctaOrigen.NombreCta;
            LblCodCta1 = ctaOrigen.CodigoCta;
            LblTipoOpe = "Pago de Tarjetas";

            if(origenTarjeta.Codigo == "0")
            {
                Catalogo tipoPropTarjeta = parametros[Constantes.keyTipoPropTarjeta] as Catalogo;

                if(tipoPropTarjeta.Codigo == "0")
                {
                    LblNombreCta2 = tarDestino.MarcaTarjeta;
                    LblCodCta2 = tarDestino.NroTarjeta;
                }
                else
                {
                    LblNombreCta2 = tarDestino.NombreCliente;
                    LblCodCta2 = tarDestino.NroTarjeta;
                }
            }
            else
            {
                LblNombreCta2 = tarDestino.MarcaTarjeta;
                LblCodCta2 = tarDestino.NroTarjeta;
            }
            
        }

        private string _nomOpeFrec;
        public string NomOpeFrec
        {
            get { return _nomOpeFrec; }
            set { SetProperty(ref _nomOpeFrec, value); }
        }

        private string _lblNombreCta1;
        public string LblNombreCta1
        {
            get { return _lblNombreCta1; }
            set { SetProperty(ref _lblNombreCta1, value); }
        }

        private string _lblCodCta1;
        public string LblCodCta1
        {
            get { return _lblCodCta1; }
            set { SetProperty(ref _lblCodCta1, value); }
        }

        private string _lblNombreCta2;
        public string LblNombreCta2
        {
            get { return _lblNombreCta2; }
            set { SetProperty(ref _lblNombreCta2, value); }
        }

        private string _lblCodCta2;
        public string LblCodCta2
        {
            get { return _lblCodCta2; }
            set { SetProperty(ref _lblCodCta2, value); }
        }

        private string _claveSms;
        public string ClaveSms
        {
            get { return _claveSms; }
            set { SetProperty(ref _claveSms, value); }
        }

        private bool _isOperacionFrecuente;
        public bool IsOperacionFrecuente
        {
            get { return _isOperacionFrecuente; }
            set { SetProperty(ref _isOperacionFrecuente, value); }
        }

        private string _lblTipoOpe;
        public string LblTipoOpe
        {
            get { return _lblTipoOpe; }
            set { SetProperty(ref _lblTipoOpe, value); }
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
                    return;
                }

                Operacion operacion = RefNavParameters[Constantes.keyOperacion] as Operacion;
                SubOperacion suboperacion = RefNavParameters[Constantes.keySubOperacion] as SubOperacion;

                //parametros
                if (operacion.Id == "1" && suboperacion.Id == "2")//pago de tc
                {
                    Catalogo origenTarjeta = RefNavParameters[Constantes.keyOrigenTarjeta] as Catalogo;

                    //propio banco
                    if(origenTarjeta.Codigo == "0")
                    {
                        await AccionTCPropia();//para propio banco
                    }
                    else
                    {
                        await AccionTCOtroBanco();
                    }
                    
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

            if (string.IsNullOrEmpty(ClaveSms))
            {
                msj = "Ingrese una clave válida.";
            }
            else if (IsOperacionFrecuente && string.IsNullOrEmpty(NomOpeFrec))
            {
                msj = "Ingrese un nombre para la operación frecuente.";
            }

            return msj;
        }

        public async Task AccionTCPropia()
        {
            NavigationParameters parametros = GetNavigationParameters();
            parametros.Add(Constantes.pageOrigen, Constantes.pageTransfConfirmacion);

            Operacion operacion = parametros["Operacion"] as Operacion;
            SubOperacion suboperacion = parametros["SubOperacion"] as SubOperacion;

            string monto = parametros["Monto"] as string;
            Catalogo moneda = parametros["Moneda"] as Catalogo;
            Cuenta ctaOrigen = parametros["CtaCargo"] as Cuenta;
            Tarjeta tcDestino = RefNavParameters[Constantes.keyTCDestino] as Tarjeta;

            decimal montoDec = decimal.Parse(monto);
            string codOperacionGenerado = "";
            DateTime fechaOperacion = DateTime.Now;

            //seccion IBS
            string _strError = "";
            string strCuentaOrigen = ctaOrigen.CodigoCta.PadLeft(12, '0');
            string strCuentaDestino = tcDestino.NroTarjeta.PadLeft(16, '0');
            string strMonedaCodMonto = moneda.Codigo;
            double dblMonto = System.Math.Round(double.Parse(monto), 2);
            string strDescrpcion = "";// Comunes.RemoverCaracteresEspeciales(txtDescripcion.Text.Trim()).ToUpper();

            //DataView dvCuentas = ((DataTable)ViewState["dtCuentas"]).DefaultView;
            //dvCuentas.RowFilter = "ODcodct='" + ddlCuentaOrigen.SelectedValue + "'";
            string strMonedaOrigen = ctaOrigen.idMoneda == "PEN" ? "S/. " : "US$ ";
            string strCodigoMoneda = ctaOrigen.idMoneda == "PEN" ? "01" : "02";
            //---agregado para trx
            string strCodigoMonedaTrx = strMonedaCodMonto == "PEN" ? "01" : "02";
            //------------
            string strMonedaDesMonto = strMonedaCodMonto == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesOrigen = strMonedaOrigen == "S/. " ? "PEN" : "USD";
            string strMonedaDesMontoLog = strMonedaCodMonto == "PEN" ? "604" : "840";
            double dblMontoOrigen = 0.0;
            double dblItf = 0.0;
            double dblComisiones = 0.0;
            double dblTotalDebitar = 0.0;
            double dblTipoCambio = 0.0;

            if (strMonedaOrigen == strMonedaDesMonto)
                dblMontoOrigen = dblMonto;
            else
            {
                /* Inicio  Tipo Cambio Preferencial */
                //StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(strMonedaOrigen == "S/. " ? "PEN" : "USD", strMonedaCodMonto, dblMonto);
                StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(strCuentaDestino.ToString(), strCuentaOrigen, strMonedaCodMonto, dblMonto);

                double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambiopre["venta"].ToString()), 3);
                double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambiopre["compra"].ToString()), 3);
                /* Fin  Tipo Cambio Preferencial */

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

            string strMonto = DateTime.Now.ToShortDateString(); //((Label)Step4.ContentTemplateContainer.FindControl("lblFecha")).Text;
            //*---
            string strMontoTrx = Convert.ToString(dblMonto * 100);
            //-----
            TransaccionesMBX tx = new TransaccionesMBX();
            string strCodLog = "0000000111";
            string strMensaje = '%' + strCuentaOrigen + '%' + strCuentaDestino.Trim() + '%' + "   " + '%' + strCodigoMonedaTrx + '%' + strCodLog + '%' + strMontoTrx.PadLeft(14, '0') + '%' + strMonedaDesMonto.Trim() + '%' + strDescrpcion.PadRight(30, ' ') + '0';
            DataSet dsOut = tx.EjecutarTransaccion(ListaTransacciones.PagoTarjetaBancoFinancieroEjecuta, 181, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            DataSet dsHeader = tx.ObtenerCabecera(ListaTransacciones.PagoTarjetaBancoFinancieroEjecuta, ListaTransacciones.NombreMensajeOut(), 0);
            if (dsOut != null && dsHeader != null)
            {
                //try
                //{
                    
                //    string strDetalleCuentaDestino = string.Concat("|", ((Label)Step3.ContentTemplateContainer.FindControl("lblDescripDestino")).Text, "|", rbtCuentaSi.Checked == false ? ((Label)Step2.ContentTemplateContainer.FindControl("lblDiaPago")).Text : dsOut.Tables["OData"].Rows[0]["ODnomtj"].ToString(), "|");
                //    ///<B>FIN</B>
                //    base.InsertaLogTransferencias(TipoTransaccion.PagoTarjetas(),
                //        TipoOperacion.Transaccion(),
                //        DefaultValues.PagoTarjetaBancoFinancieroEjecuta,
                //        strMensaje,
                //        dsOut,
                //        _strError,
                //        dsHeader.Tables["OHead"].Rows[0]["OHlogtx"].ToString(),
                //        ddlCuentaOrigen.SelectedValue.Substring(4, 12),
                //        ddlCuentaOrigen.SelectedItem.Text,
                //        dsOut.Tables["OData"].Rows[0]["ODmtocv"].ToString(),
                //        dvCuentas[0]["ODmoned"].ToString(),
                //        strCuentaDestino,
                //        strCuentaDestino + strDetalleCuentaDestino,
                //        txtMonto.Text,
                //        strMonedaDesMontoLog, "", "",
                //        dsOut.Tables["OData"].Rows[0]["ODComx"].ToString(),
                //        "0.00", dsOut.Tables["OData"].Rows[0]["ODticam"].ToString(), dsOut.Tables["OData"].Rows[0]["ODItfx"].ToString(), strDescrpcion, rbtCuentaSi.Checked == true ? TipoDestinoCuentaOperacion.Propia : TipoDestinoCuentaOperacion.Tercero);
                //}
                //catch (Exception)
                //{ }
            }

            if (_strError == "0000")
            {
                //base.RefillData();

                dblItf = (double)dsOut.Tables["OData"].Rows[0]["ODitfx"];
                dblComisiones = (double)dsOut.Tables["OData"].Rows[0]["ODcomx"];
                dblTotalDebitar = dblMontoOrigen + dblItf + dblComisiones;
                fechaOperacion = DateTime.Now;

                codOperacionGenerado = (string)dsHeader.Tables["OHead"].Rows[0]["OHlogtx"].ToString();

                //((Label)Step4.ContentTemplateContainer.FindControl("lblNroComprobante")).Text = dsHeader.Tables["OHead"].Rows[0]["OHlogtx"].ToString();
                //((Label)Step4.ContentTemplateContainer.FindControl("lblDescripcion")).Text = strDescrpcion + "&nbsp";
                //((Label)Step4.ContentTemplateContainer.FindControl("lblFecha")).Text = DateTime.Now.ToShortDateString();
                //((Label)Step4.ContentTemplateContainer.FindControl("lblHora")).Text = DateTime.Now.ToShortTimeString();
                //((Label)Step4.ContentTemplateContainer.FindControl("lblCuentaCargo")).Text = ((Label)Step3.ContentTemplateContainer.FindControl("lblDescripOrigen")).Text;
                //((Label)Step4.ContentTemplateContainer.FindControl("lblImporteCargado")).Text = ((Label)Step3.ContentTemplateContainer.FindControl("lblImporteOrigen")).Text;
                //((Label)Step4.ContentTemplateContainer.FindControl("lblTipoCambio")).Text = ((Label)Step3.ContentTemplateContainer.FindControl("lblTipoCambio")).Text;
                //((Label)Step4.ContentTemplateContainer.FindControl("lblItf")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblItf.ToString());
                //((Label)Step4.ContentTemplateContainer.FindControl("lblComision")).Text = ((Label)Step3.ContentTemplateContainer.FindControl("lblComision")).Text;
                //((Label)Step4.ContentTemplateContainer.FindControl("lblImporteDebito")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblTotalDebitar.ToString());
                //((Label)Step4.ContentTemplateContainer.FindControl("lblCuentaDestino")).Text = ((Label)Step3.ContentTemplateContainer.FindControl("lblDescripDestino")).Text;
                //HtmlTableRow trDatosTerceros4 = (HtmlTableRow)Step4.ContentTemplateContainer.FindControl("trDatosTerceros4");
                //if (rbtCuentaSi.Checked)
                //{
                //    trDatosTerceros4.Style.Add("display", "none");
                //    InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), TipoTransaccion.PagoTarjetas(), TipoOperacionesMonitoreo.PagoTarjetasCreditoBFpropias(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
                //}
                //else
                //{
                //    ((Label)Step4.ContentTemplateContainer.FindControl("lblTitular")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblDiaPago")).Text;
                //    trDatosTerceros4.Style.Remove("display");
                //    InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), TipoTransaccion.PagoTarjetas(), TipoOperacionesMonitoreo.PagoTarjetasCreditoBFTerceros(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
                //}
                //((Label)Step4.ContentTemplateContainer.FindControl("lblImporteAbonado")).Text = strMonedaDesMonto + Comunes.FormateaNumero(dblMonto.ToString());

                //DataTable dt = null;
                //bool existeTarjeta = false;

                //this.Botonera1.MostrarBotonera(true);
                //try
                //{
                //    string codigoCliente = (string)Session["strCodigoClienteIBS"];
                //    if (rbtCuentaSi.Checked)
                //        existeTarjeta = true;
                //    else
                //    {
                //        dt = _objGeneral.ObtenerDatosFrecuentes(codigoCliente, (string)((int)DatosFrecuentes.TarjetaTerceroFinanciero).ToString(), (int)Estado.Habilitado);

                //        Session["ds_botonera"] = dt;
                //        if (dt.Rows.Count > 0)
                //            for (int i = 0; i < dt.Rows.Count; i++)
                //                if (ddlCuentaDestino.Text == dt.Rows[i]["strValor"].ToString())
                //                {
                //                    existeTarjeta = true;
                //                    break;
                //                }
                //    }
                //}
                //catch (Exception)
                //{ }
                //finally
                //{ dt = null; }

                //if (!existeTarjeta)
                //{
                //    HtmlTable paso3TablaTarjeta1 = (HtmlTable)Step4.ContentTemplateContainer.FindControl("tblGuardarTarjeta1");
                //    paso3TablaTarjeta1.Style.Add("display", "block");
                //}
            }
            else
            {
               // wzContenedor.Visible = false;
                try
                {
                    await DialogService.DisplayAlertAsync("Validación", "Código de error IBS: " + _strError, "OK");
                    return;
                    //Mensaje1.MostrarFalla(ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)));
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                }
                catch (Exception)
                {
                    //Mensaje1.MostrarFalla("Error no manejado, intente otra vez!");
                   // MostrarMensaje("", "Error no manejado, intente otra vez!", ListImages.Error);
                }
                //if (rbtCuentaSi.Checked)
                //    InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), TipoTransaccion.PagoTarjetas(), TipoOperacionesMonitoreo.PagoTarjetasCreditoBFpropias(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
                //else
                //    InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), TipoTransaccion.PagoTarjetas(), TipoOperacionesMonitoreo.PagoTarjetasCreditoBFTerceros(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
            }
        
        //

            //acciones mock
            string rptaTrx = CuentaService.efectuarMovimiento(ctaOrigen, montoDec, moneda.Codigo, false);

            if (rptaTrx != "")
            {
                await DialogService.DisplayAlertAsync(Constantes.MSJ_INFO, rptaTrx, Constantes.MSJ_BOTON_OK);
            }
            else
            {
                //string rptaTrx2 = CuentaService.efectuarMovimiento(ctaDestino, montoDec, moneda.Codigo, true);

                //if (rptaTrx != "")
                //{
                //    await DialogService.DisplayAlertAsync(Constantes.MSJ_INFO, rptaTrx, Constantes.MSJ_BOTON_OK);
                //}
                //else
                //{
                RegistrarOperacionFrecuente(fechaOperacion);

                parametros.Add("CodOperacionGenerado", codOperacionGenerado);
                parametros.Add("FechaOperacion", fechaOperacion);

                bool isOperacionFrecuente = RefNavParameters.ContainsKey(Constantes.keyOperacionFrecuente);
                string retroceso;
                Catalogo tipoPropTarjeta = RefNavParameters[Constantes.keyTipoPropTarjeta] as Catalogo;
                if (!isOperacionFrecuente)//2 mas para propia, uno mas para tercero. revisar opefrecuente
                {
                    if(tipoPropTarjeta.Codigo == "0")
                    {
                        retroceso = "../../../../../../../";
                    }
                    else
                    {
                        retroceso = "../../../../../../";
                    }
                }
                else
                {
                    retroceso = "../../";
                }

                await NavigationService.NavigateAsync(retroceso + Constantes.pagePagoTCResumen, parametros); 

            }
        }

        public async Task AccionTCOtroBanco()
        {
            NavigationParameters parametros = GetNavigationParameters();
            parametros.Add(Constantes.pageOrigen, Constantes.pageTransfConfirmacion);

            Operacion operacion = parametros["Operacion"] as Operacion;
            SubOperacion suboperacion = parametros["SubOperacion"] as SubOperacion;

            string monto = parametros["Monto"] as string;
            Catalogo moneda = parametros["Moneda"] as Catalogo;
            Cuenta ctaOrigen = parametros["CtaCargo"] as Cuenta;
            Tarjeta tcDestino = RefNavParameters[Constantes.keyTCDestino] as Tarjeta;

            decimal montoDec = decimal.Parse(monto);
            string codOperacionGenerado = "";
            DateTime fechaOperacion = DateTime.Now;
            
            //seccion IBS
            string _strError = "";
            TransaccionesMBX tx = new TransaccionesMBX();
            string strCuentaOrigen = ctaOrigen.CodigoCta.PadLeft(12, '0');
            string strCuentaDestino = tcDestino.NroTarjeta.PadLeft(16, '0');
            string strMonedaCodMonto = moneda.Codigo;
            double dblMonto = System.Math.Round(double.Parse(monto), 2);
            string strDescrpcion = "";// Comunes.RemoverCaracteresEspeciales(txtDescripcion.Text.Trim()).ToUpper();
            //strDescrpcion = (strDescrpcion.ToString().Trim().Length == 0) ? "TRANSF.CCI." : strDescrpcion.ToString().Trim();

            //DataView dvCuentas = ((DataTable)ViewState["dtCuentas"]).DefaultView;
            //dvCuentas.RowFilter = "ODcodct='" + ddlCuentaOrigen.SelectedValue + "'";
            string strMonedaOrigen = ctaOrigen.idMoneda == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesMonto = strMonedaCodMonto == "PEN" ? "S/. " : "US$ ";
            string strMonedaDesOrigen = strMonedaOrigen == "S/. " ? "PEN" : "USD";
            string strMonedaDesMontoLog = strMonedaCodMonto == "PEN" ? "604" : "840";
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
                /* Inicio  Tipo Cambio Preferencial */
                //StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(strMonedaOrigen == "S/. " ? "PEN" : "USD", strMonedaCodMonto, dblMonto);
                StringDictionary dsTipoCambiopre = ObtenerTipoCambioPreferencial(strCuentaDestinoCCI, strCuentaOrigen, strMonedaCodMonto, dblMonto);

                double dblCambioVenta = System.Math.Round(double.Parse(dsTipoCambiopre["venta"].ToString()), 3);
                double dblCambioCompra = System.Math.Round(double.Parse(dsTipoCambiopre["compra"].ToString()), 3);
                /* Fin  Tipo Cambio Preferencial */

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

            string strMonto = Convert.ToString(System.Math.Round(dblMontoOrigen, 2) * 100);
            //-----
            //string strMontoTrx = Convert.ToString(System.Math.Round(dblMonto, 2) * 100);
            //-----
            if (strCuentaDestino.ToString().Length == 15)
            {
                strCuentaDestino = strCuentaDestino.ToString().Trim().PadLeft(16, '0');
            }
            string tlog = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0');
            //string strMensaje = strCuentaOrigen.PadRight(12, ' ') + tlog + strMonto.PadLeft(14, '0') + strMonedaDesOrigen + ddlBanco.SelectedValue.Trim() + strCuentaDestino.PadRight(17, ' ') + ddlTipoDocumento.SelectedValue.Trim() + txtNumeroDocumentoBeneficiario.Text.Trim().PadLeft(12, '0') + strNombres.PadRight(45, ' ') + txtDireccion.Text.Trim().PadRight(65, ' ') + "0000000000" + (rbtCuentaSi.Checked ? '1' : '0') + '0' + strDescrpcion.PadRight(30, ' ') + (rbtCuentaSi.Checked ? '1' : '0') + '0'; // rc consultar si existe pago a proveedor (chkPagoProveedores.Checked ? "1" : "0");
            //string strMensaje = strCuentaOrigen.PadRight(12, ' ') + tlog + strMontoTrx.PadLeft(14, '0') + strMonedaCodMonto.Trim() + ddlBanco.SelectedValue.Trim() + strCuentaDestino.PadRight(17, ' ') + ddlTipoDocumento.SelectedValue.Trim() + txtNumeroDocumentoBeneficiario.Text.Trim().PadLeft(12, '0') + strNombres.PadRight(45, ' ') + txtDireccion.Text.Trim().PadRight(65, ' ') + "0000000000" + (rbtCuentaSi.Checked ? '1' : '0') + '0' + strDescrpcion.PadRight(30, ' ') + (rbtCuentaSi.Checked ? '1' : '0') + '0'; // rc consultar si existe pago a proveedor (chkPagoProveedores.Checked ? "1" : "0");
            string strMensaje = strCuentaOrigen.PadRight(12, ' ') + tlog + strMonto.PadLeft(14, '0') + strMonedaCodMonto.Trim() + "".Trim() + strCuentaDestino.PadRight(17, ' ') + "".Trim() + "".Trim().PadLeft(12, '0') + strNombres.PadRight(45, ' ') + "".Trim().PadRight(65, ' ') + "0000000000" + ('0') + '0' + strDescrpcion.PadRight(30, ' ') + ('0') + '0'; // rc consultar si existe pago a proveedor (chkPagoProveedores.Checked ? "1" : "0");
            DataSet dsOut = tx.EjecutarTransaccion(ListaTransacciones.PagoTarjetaOtroBancoEjecuta, 308, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            DataSet dsHeader = tx.ObtenerCabecera(ListaTransacciones.PagoTarjetaOtroBancoEjecuta, ListaTransacciones.NombreMensajeOut(), 0);
            if (dsOut != null && dsHeader != null)
            {
                try
                {
                    //string strDetalleCuentaDestino = string.Concat("|", rbtCuentaSi.Checked == false ? "No" : "Si", "|", strNombres, "|", ddlTipoDocumento.SelectedItem.Text + txtNumeroDocumentoBeneficiario.Text, "|", txtDireccion.Text, "|");
                    //base.InsertaLogTransferencias(TipoTransaccion.PagoTarjetasOtrosBancos(),
                    //        TipoOperacion.Transaccion(),
                    //        DefaultValues.PagoTarjetaOtroBancoEjecuta,
                    //        strMensaje, dsOut, _strError,
                    //        dsHeader.Tables["OHead"].Rows[0]["OHlogtx"].ToString(),
                    //        ddlCuentaOrigen.SelectedValue.Substring(4, 12),
                    //        ddlCuentaOrigen.SelectedItem.Text + dsOut.Tables["OData"].Rows[0]["ODclifr"].ToString(),
                    //        dsOut.Tables["OData"].Rows[0]["OutTot"].ToString(),
                    //        dvCuentas[0]["ODmoned"].ToString(),
                    //        ddlBanco.SelectedItem.Value.ToString().Trim() + ddlCuentaDestino.Text,
                    //        ddlCuentaDestino.Text + ' ' + ddlBanco.SelectedItem.Text + strDetalleCuentaDestino,
                    //        txtMonto.Text.Trim(),
                    //        strMonedaDesMontoLog,
                    //        "",
                    //        "",
                    //        Comunes.FormateaNumero(dsOut.Tables["OData"].Rows[0]["OutCo1"].ToString()),
                    //        Comunes.FormateaNumero(dsOut.Tables["OData"].Rows[0]["OutCom"].ToString()), Comunes.FormateaNumero(dsOut.Tables["OData"].Rows[0]["ODticam"].ToString()), Comunes.FormateaNumero(dsOut.Tables["OData"].Rows[0]["ODItf"].ToString()), strDescrpcion, TipoDestinoCuentaOperacion.Tercero);
                }
                catch (Exception)
                { }

            }
            if (_strError == "0000")
            {
                //base.RefillData();

                dblItf = (double)dsOut.Tables["OData"].Rows[0]["ODItf"];
                dblComisiones = (double)dsOut.Tables["OData"].Rows[0]["ODComix"];
                dblTotalDebitar = (double)dsOut.Tables["OData"].Rows[0]["ODttoor"];

                fechaOperacion = DateTime.Now;

                codOperacionGenerado = dsHeader.Tables["OHead"].Rows[0]["OHlogtx"].ToString();
                //Step2.ContentTemplateContainer.FindControl("tblStep2").Visible = true;
                //Step2.CustomNavigationTemplateContainer.FindControl("btnStep2Continuar").Visible = true;

                //((Label)Step3.ContentTemplateContainer.FindControl("lblNroComprobante")).Text = dsHeader.Tables["OHead"].Rows[0]["OHlogtx"].ToString();
                //((Label)Step3.ContentTemplateContainer.FindControl("lblDescripcion")).Text = strDescrpcion + "&nbsp";
                //((Label)Step3.ContentTemplateContainer.FindControl("lblFecha")).Text = DateTime.Now.ToShortDateString();
                //((Label)Step3.ContentTemplateContainer.FindControl("lblHora")).Text = DateTime.Now.ToShortTimeString();
                //((Label)Step3.ContentTemplateContainer.FindControl("lblCuentaCargo")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblDescripOrigen")).Text;
                //((Label)Step3.ContentTemplateContainer.FindControl("lblImporteCargado")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblImporteOrigen")).Text;
                //((Label)Step3.ContentTemplateContainer.FindControl("lblTipoCambio")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblTipoCambio")).Text;
                //((Label)Step3.ContentTemplateContainer.FindControl("lblComision")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblComision")).Text;
                //((Label)Step3.ContentTemplateContainer.FindControl("lblImporteDebito")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblTotalDebitar.ToString());
                //((Label)Step3.ContentTemplateContainer.FindControl("lblItf")).Text = strMonedaOrigen + Comunes.FormateaNumero(dblItf.ToString());
                //((Label)Step3.ContentTemplateContainer.FindControl("lblCuentaDestino")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblDescripDestino")).Text;
                //((Label)Step3.ContentTemplateContainer.FindControl("lblCuentaPropia")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblCuentaPropia")).Text;
                //HtmlTableRow trDatosTercerosPaso3_1 = (HtmlTableRow)Step3.ContentTemplateContainer.FindControl("trDatosTercerosPaso3_1");
                //HtmlTableRow trDatosTercerosPaso3_2 = (HtmlTableRow)Step3.ContentTemplateContainer.FindControl("trDatosTercerosPaso3_2");
                //HtmlTableRow trDatosTercerosPaso3_3 = (HtmlTableRow)Step3.ContentTemplateContainer.FindControl("trDatosTercerosPaso3_3");

                //if (rbtCuentaSi.Checked)
                //{
                //    trDatosTercerosPaso3_1.Style.Add("display", "none");
                //    trDatosTercerosPaso3_2.Style.Add("display", "none");
                //    trDatosTercerosPaso3_3.Style.Add("display", "none");
                //    InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), TipoTransaccion.PagoTarjetasOtrosBancos(), TipoOperacionesMonitoreo.PagoTarjetasCreditoOtrosBancos(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
                //}
                //else
                //{
                //    ((Label)Step3.ContentTemplateContainer.FindControl("lblEtiquetaNombre")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblEtiquetaNombre")).Text;
                //    ((Label)Step3.ContentTemplateContainer.FindControl("lblNombre")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblNombre")).Text;
                //    ((Label)Step3.ContentTemplateContainer.FindControl("lblDocumento")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblDocumento")).Text;
                //    ((Label)Step3.ContentTemplateContainer.FindControl("lblDirección")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblDirección")).Text;
                //    trDatosTercerosPaso3_1.Style.Remove("display");
                //    trDatosTercerosPaso3_2.Style.Remove("display");
                //    trDatosTercerosPaso3_3.Style.Remove("display");
                //    InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), TipoTransaccion.PagoTarjetasOtrosBancos(), TipoOperacionesMonitoreo.PagoTarjetasCreditoOtrosBancos(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
                //}
                //((Label)Step3.ContentTemplateContainer.FindControl("lblImporteAbonado")).Text = strMonedaDesMonto + Comunes.FormateaNumero(dblMonto.ToString());
                //((Label)Step3.ContentTemplateContainer.FindControl("lblDescripcion")).Text = ((Label)Step2.ContentTemplateContainer.FindControl("lblDescripcion")).Text;

                //DataTable dt;
                //bool existeTarjeta = false;

                //this.Botonera1.MostrarBotonera(true);
                //try
                //{
                //    string codigoCliente = (string)Session["strCodigoClienteIBS"];
                //    if (rbtCuentaSi.Checked)
                //        dt = _objGeneral.ObtenerDatosFrecuentes(codigoCliente, (string)((int)DatosFrecuentes.TarjetaPropiaOtroBanco).ToString(), (int)Estado.Habilitado);
                //    else
                //        dt = _objGeneral.ObtenerDatosFrecuentes(codigoCliente, (string)((int)DatosFrecuentes.TarjetaTerceroOtroBanco).ToString(), (int)Estado.Habilitado);

                //    Session["ds_botonera"] = dt;
                //    if (dt.Rows.Count > 0)
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //            if (ddlCuentaDestino.Text == dt.Rows[i]["strValor"].ToString())
                //            {
                //                existeTarjeta = true;
                //                break;
                //            }
                //}
                //catch (Exception)
                //{ }
                //finally
                //{ dt = null; }

                //if (!existeTarjeta)
                //{
                //    HtmlTable paso3TablaTarjeta1 = (HtmlTable)Step3.ContentTemplateContainer.FindControl("tblGuardarTarjeta1");
                //    paso3TablaTarjeta1.Style.Add("display", "block");
                //}
            }
            else
            {
                //wzContenedor.Visible = false;
                try
                {
                    await DialogService.DisplayAlertAsync("Validación", "Código de error IBS: " + _strError, "OK");
                    return;
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                }
                catch (Exception)
                {
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(6666), ListImages.Error);
                }
                //InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), TipoTransaccion.PagoTarjetasOtrosBancos(), TipoOperacionesMonitoreo.PagoTarjetasCreditoOtrosBancos(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
            }

            //

            //acciones mock
            string rptaTrx = CuentaService.efectuarMovimiento(ctaOrigen, montoDec, moneda.Codigo, false);

            if (rptaTrx != "")
            {
                await DialogService.DisplayAlertAsync(Constantes.MSJ_INFO, rptaTrx, Constantes.MSJ_BOTON_OK);
            }
            else
            {
                //string rptaTrx2 = CuentaService.efectuarMovimiento(ctaDestino, montoDec, moneda.Codigo, true);

                //if (rptaTrx != "")
                //{
                //    await DialogService.DisplayAlertAsync(Constantes.MSJ_INFO, rptaTrx, Constantes.MSJ_BOTON_OK);
                //}
                //else
                //{
                RegistrarOperacionFrecuente(fechaOperacion);

                parametros.Add("CodOperacionGenerado", codOperacionGenerado);
                parametros.Add("FechaOperacion", fechaOperacion);

                bool isOperacionFrecuente = RefNavParameters.ContainsKey(Constantes.keyOperacionFrecuente);
                string retroceso;
                if (!isOperacionFrecuente)
                {
                    retroceso = "../../../../../";
                }
                else
                {
                    retroceso = "../../";
                }

                await NavigationService.NavigateAsync(retroceso + Constantes.pagePagoTCResumen, parametros);

            }
        }

        public void RegistrarOperacionFrecuente(DateTime fechaOperacion)
        {
            if (IsOperacionFrecuente)
            {
                string pageOrigen = RefNavParameters[Constantes.pageOrigen] as string;
                Operacion operacion = RefNavParameters["Operacion"] as Operacion;
                SubOperacion suboperacion = RefNavParameters["SubOperacion"] as SubOperacion;

                string monto = RefNavParameters["Monto"] as string;
                Catalogo moneda = RefNavParameters["Moneda"] as Catalogo;
                Cuenta ctaOrigen = RefNavParameters[Constantes.keyCtaCargo] as Cuenta;
                Tarjeta tcDestino = RefNavParameters[Constantes.keyTCDestino] as Tarjeta;
                Catalogo origenTarjeta = RefNavParameters[Constantes.keyOrigenTarjeta] as Catalogo;
                

                OperacionFrecuente opeFrec = new OperacionFrecuente
                {
                    FechaOperacion = fechaOperacion,
                    SubOperacion = suboperacion,
                    Operacion = operacion,
                    NombreFrecuente = NomOpeFrec
                };

                opeFrec.CtaOrigen = ctaOrigen;
                opeFrec.TcDestino = tcDestino;
                opeFrec.OrigenTarjeta = origenTarjeta;
                opeFrec.Moneda = moneda;
                if (RefNavParameters.ContainsKey(Constantes.keyTipoPropTarjeta))
                {
                    opeFrec.TipoPropTarjeta = RefNavParameters[Constantes.keyTipoPropTarjeta] as Catalogo;
                }
                
                OperacionService.AgregarOperacionFrecuente(opeFrec);
                EventAggregator.GetEvent<OpeFrecuenteAddedEvent>().Publish();
            }
        }
    }
}
