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
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BL = NS.MBX_amarin.BusinessLogic;

namespace NS.MBX_amarin.ViewModels
{
	public class PagoServConfirmacionViewModel : ViewModelBase
    {
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private IOperacionService OperacionService { get; set; }
        private IEventAggregator EventAggregator { get; set; }

        private string _strError = "";
        private string CodigoServicioTemporal;
        private string NombreServicio;
        private string CodigoServicio = "0";
        private string CodOperacionGenerado;
        private DateTime FechaOperacion;

        public PagoServConfirmacionViewModel(IOperacionService operacionService, ICuentaService cuentaService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService, IEventAggregator eventAggregator)
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

            Recibo reciboIBS = RefNavParameters[Constantes.keyReciboIBS] as Recibo;
            Catalogo empresa = RefNavParameters[Constantes.keyEmpresa] as Catalogo;
            Servicio servicio = RefNavParameters[Constantes.keyServicio] as Servicio;
            string codigoServicio = RefNavParameters[Constantes.keyCodigoServicio] as string;



            //Cuenta ctaOrigen = parametros[Constantes.keyCtaCargo] as Cuenta;
            //Tarjeta tarDestino = parametros[Constantes.keyTCDestino] as Tarjeta;//cuando el objeto no existe, esto devuelve null
            //Catalogo origenTarjeta = parametros[Constantes.keyOrigenTarjeta] as Catalogo;

            //LblNombreCta1 = ctaOrigen.NombreCta;
            //LblCodCta1 = ctaOrigen.CodigoCta;
            //LblTipoOpe = "Pago de Tarjetas";

            //if (origenTarjeta.Codigo == "0")
            //{
            //    Catalogo tipoPropTarjeta = parametros[Constantes.keyTipoPropTarjeta] as Catalogo;

            //    if (tipoPropTarjeta.Codigo == "0")
            //    {
            //        LblNombreCta2 = tarDestino.MarcaTarjeta;
            //        LblCodCta2 = tarDestino.NroTarjeta;
            //    }
            //    else
            //    {
            //        LblNombreCta2 = tarDestino.NombreCliente;
            //        LblCodCta2 = tarDestino.NroTarjeta;
            //    }
            //}
            //else
            //{
            //    LblNombreCta2 = tarDestino.MarcaTarjeta;
            //    LblCodCta2 = tarDestino.NroTarjeta;
            //}

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
                if (operacion.Id == "1" && (suboperacion.Id == "0" || suboperacion.Id == "1"))//pago de tc
                {
                    Catalogo empresa = RefNavParameters[Constantes.keyEmpresa] as Catalogo;
                    Servicio servicio = RefNavParameters[Constantes.keyServicio] as Servicio;

                    //dependiendo de la empresa coloca el label
                    if (empresa.Codigo == "0" || empresa.Codigo == "2" || (empresa.Codigo == "1" && servicio.Codigo == "0")) //claro o entel
                    {
                        await AccionTelefonoCelular();
                    }
                    else if (empresa.Codigo == "1" && servicio.Codigo == "1")
                    {
                        await AccionTelefonoFijo();
                    }
                    else if (empresa.Codigo == "3" || empresa.Codigo == "4")//3 y 4 son luz
                    {
                       // msj = AccionLuz();
                    }
                    else if (empresa.Codigo == "5")//agua
                    {
                        //msj = AccionAgua();
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

        public async Task AccionTelefonoCelular()
        {
            NavigationParameters parametros = GetNavigationParameters();
            parametros.Add(Constantes.pageOrigen, Constantes.pagePagoServConfirmacion);

            Operacion operacion = parametros["Operacion"] as Operacion;
            SubOperacion suboperacion = parametros["SubOperacion"] as SubOperacion;

            Catalogo empresa = parametros[Constantes.keyEmpresa] as Catalogo;
            Servicio servicio = parametros[Constantes.keyServicio] as Servicio;
            string codServicio = parametros[Constantes.keyCodigoServicio] as string;
            Recibo recibo = parametros[Constantes.keyReciboIBS] as Recibo;
            DetalleRecibo detRecibo = parametros[Constantes.keyDetalleReciboIBS] as DetalleRecibo;

            Cuenta ctaOrigen = parametros["CtaCargo"] as Cuenta;
            Tarjeta tcDestino = RefNavParameters[Constantes.keyTCDestino] as Tarjeta;

            decimal montoDec = decimal.Parse(detRecibo.Monto);
            string moneda = "PEN";

            //seccion IBS
            string strNumero = "";
            string strNumeroTelefeno = ""; // strNumero.PadRight(10, ' ');
            string strNumeroRecibo = ""; // NumeroRecibo.PadLeft(10, '0');
            string strFechaEmision = ""; //Fecha.PadRight(8, '0');
            string strMoneda = ""; //Moneda.PadRight(3, '0');
            string strImporteRecibo = ""; // string.Format("{0:N2}", TotalPagar).ToString().Replace(".", "").PadLeft(14, '0');
            string strCuentaCargo = ""; //ddlCuentas.SelectedValue.ToString().PadLeft(12, '0');//ddlCuentas.SelectedValue.ToString().PadLeft(12, '0');
            string strNombreAbonado = ""; //NombreCliente.PadRight(30, ' ');
            string strNombreCliente = ""; //Session["strNombreClienteIBS"].ToString().PadRight(30, ' ');
            string strReferencia = ""; // strNumeroTelefeno;
            string strTrama = "";
            //using (HBC.ComboBoxText CtrlddlProveedor = (HBC.ComboBoxText)Step1.ContentTemplateContainer.FindControl("ComboBoxText1"))
            //    if (CtrlddlProveedor != null)
            //        strNumero = CtrlddlProveedor.Text;
            //DropDownList ddlproveedor = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlProveedor");
            //DropDownList ddlCuentas = (DropDownList)Step2.ContentTemplateContainer.FindControl("ddlCuentas");
            //TextBox txtDescripcion = (TextBox)Step2.ContentTemplateContainer.FindControl("txtDescripcion");
            string strDescripcion = "";// (txtDescripcion.Text.ToString() != string.Empty) ? txtDescripcion.Text.ToString().PadRight(30, ' ') : "".PadRight(30, ' ');
            BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();
            DataSet dsHeader = new DataSet();
            CodigoServicioTemporal = (int.Parse(CodigoServicio) == 0) ? CodigoServicioTemporal : CodigoServicio;
            CodigoServicioTemporal = empresa.Codigo + servicio.Codigo;//aniadido temporalmente
            string strCodigoTrama = string.Empty;
            using (DataSet CtrlInput = ObjPagoServicios.ObtenerControlesInput(int.Parse(CodigoServicioTemporal), 2))
            {
                strCodigoTrama = CtrlInput.Tables[0].Rows[0]["CtrlTrama"].ToString();
                switch (strCodigoTrama)
                {
                    case "7031":
                        strNumeroTelefeno = codServicio.PadRight(10, ' ');
                        strNumeroRecibo = detRecibo.IdDetalleRecibo.PadLeft(10, '0');
                        strFechaEmision = detRecibo.FechaEmision.PadRight(8, '0');
                        strMoneda = "PEN";//Moneda.PadRight(3, '0');
                        strImporteRecibo = Convert.ToString(System.Math.Round(montoDec, 2) * 100).PadLeft(14, '0');  //string.Format("{0:N2}", TotalPagar).ToString().Replace(".", "").PadLeft(14, '0');
                        strCuentaCargo = ctaOrigen.CodigoCta.ToString().PadLeft(12, '0');//ddlCuentas.SelectedValue.ToString().PadLeft(12, '0');
                        strNombreAbonado = recibo.NombreCliente.PadRight(30, ' ');
                        strNombreCliente = "Juan Perez";// Session["strNombreClienteIBS"].ToString().Trim().PadRight(31, ' ').Substring(0, 30);
                        strReferencia = strNumeroTelefeno;
                        strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' ');
                        PagarConTrama7031(int.Parse(CodigoServicioTemporal), out _strError, out strTrama, out dsHeader, strNumero, strNumeroRecibo, strFechaEmision, moneda, strImporteRecibo, strCuentaCargo, strDescripcion, strNombreAbonado);
                        break;
                    case "7054":
                        strNumeroTelefeno = strNumero.PadRight(10, ' ');
                        strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' ');
                        strCuentaCargo = ctaOrigen.CodigoCta.ToString().PadLeft(12, '0');
                        strNombreCliente = "Juan Perez";
                        strImporteRecibo = Convert.ToString(System.Math.Round(montoDec, 2) * 100).PadLeft(11, '0'); // string.Format("{0:N2}", TotalPagar).ToString().Replace(".", "").PadLeft(11, '0');
                        //PagarConTrama7054(int.Parse(CodigoServicioTemporal), out _strError, out strTrama, out dsHeader, strNumeroTelefeno, strCuentaCargo, strNombreCliente, strImporteRecibo, moneda, CodigoProducto, EstadoDeuda, TipoDocumento, NumeroDocumento, NumeroReferencia, FechaEmision, FechaVencimiento, "".PadLeft(12, ' '), "".PadLeft(12, ' '), "".PadLeft(6, ' '), "".PadLeft(6, ' '), strDescripcion, "0");
                        PagarConTrama7054(int.Parse(CodigoServicioTemporal), out _strError, out strTrama, out dsHeader, strNumeroTelefeno, strCuentaCargo, strNombreCliente, strImporteRecibo, moneda, "", "", "", "", "", "", "", "".PadLeft(12, ' '), "".PadLeft(12, ' '), "".PadLeft(6, ' '), "".PadLeft(6, ' '), strDescripcion, "0");

                        break;
                    default:
                        strNumeroTelefeno = codServicio.PadRight(10, ' ');
                        strNumeroRecibo = detRecibo.IdDetalleRecibo.PadLeft(10, '0');
                        strFechaEmision = detRecibo.FechaEmision.PadRight(8, '0');
                        strMoneda = "PEN";//Moneda.PadRight(3, '0');
                        strImporteRecibo = Convert.ToString(System.Math.Round(montoDec, 2) * 100).PadLeft(14, '0');  //string.Format("{0:N2}", TotalPagar).ToString().Replace(".", "").PadLeft(14, '0');
                        strCuentaCargo = ctaOrigen.CodigoCta.ToString().PadLeft(12, '0');//ddlCuentas.SelectedValue.ToString().PadLeft(12, '0');
                        strNombreAbonado = recibo.NombreCliente.PadRight(30, ' ');
                        strNombreCliente = "Juan Perez";// Session["strNombreClienteIBS"].ToString().Trim().PadRight(31, ' ').Substring(0, 30);
                        strReferencia = strNumeroTelefeno;
                        strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' ');
                        PagarConTrama7031(int.Parse(CodigoServicioTemporal), out _strError, out strTrama, out dsHeader, strNumero, strNumeroRecibo, strFechaEmision, moneda, strImporteRecibo, strCuentaCargo, strDescripcion, strNombreAbonado);
                        break;
                }
                
                //InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), BE.TipoTransaccion.PagoServicios(), BE.TipoOperacionesMonitoreo.PagoServiciosTelefoniaMovil(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
            }
            //

            //acciones mock
            string rptaTrx = CuentaService.efectuarMovimiento(ctaOrigen, montoDec, moneda, false);

            if (rptaTrx != "")
            {
                await DialogService.DisplayAlertAsync(Constantes.MSJ_INFO, rptaTrx, Constantes.MSJ_BOTON_OK);
            }
            else
            {
                RegistrarOperacionFrecuente(FechaOperacion);

                parametros.Add(Constantes.keyCodOperacionGenerado, CodOperacionGenerado);
                parametros.Add(Constantes.keyFechaOperacion, FechaOperacion);

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

                await NavigationService.NavigateAsync(retroceso + Constantes.pagePagoServResumen, parametros);

            }
        }


        private void PagarConTrama7031(int intCodigoServicio, out string _strError, out string strTrama, out DataSet dsHeader, string strNumeroTelefeno, string strNumeroRecibo, string strFechaEmision, string strMoneda, string strImporteRecibo, string strCuentaCargo, string strDescripcion, string strNombreAbonado)
        {
            try
            {
                BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();
                using (DataSet dsSalida = ObjPagoServicios.EjecutarTransaccion(294, intCodigoServicio, out _strError, out strTrama, out dsHeader, strNumeroTelefeno, strNumeroRecibo, strFechaEmision, strMoneda, strImporteRecibo, strCuentaCargo, "", strDescripcion, strNombreAbonado, "0"))
                {
                    if (_strError == "0000")
                    {
                        CodOperacionGenerado = "22154";
                        FechaOperacion = DateTime.Now;
                        //base.RefillData();
                        //DataView dvMoneda = ListaMonedas.DefaultView;
                        //string strPrefixMoneda = string.Empty;
                        //((Label)Step4.ContentTemplateContainer.FindControl("lblNumeroOperacion")).Text = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString();
                        //ObtenerDatosStep4();
                        //try
                        //{
                        //    base.InsertaLogPagoServicios(
                        //        BE.TipoTransaccion.PagoServicios(),
                        //        BE.TipoOperacion.Transaccion(),
                        //        BE.DefaultValues.PAGO_SERVICIOS(),
                        //        strTrama,
                        //        dsSalida,
                        //        _strError,
                        //        dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(),
                        //        NumeroCuenta, // ddlCuenta.SelectedValue.Substring(4, 12).ToString(),
                        //        DescripcionCuenta, // ddlCuenta.SelectedItem.Text,
                        //        dsSalida.Tables["OData"].Rows[0]["ODmtocv"].ToString(),
                        //        MonedaCuenta, // strMonedaOrigen,
                        //        CodigoProveedor, //ddlProveedor.SelectedValue,
                        //        NombreProveedor, //ddlProveedor.SelectedItem.Text,
                        //        CodigoServicioTemporal,//ddlServicio.SelectedValue,
                        //        NombreServicio, // ddlServicio.SelectedItem.Text,
                        //        "0", //ddlSector.SelectedValue,
                        //        string.Concat("|", NombreCliente, " ", NumeroAbonado),//ddlSector.SelectedItem.Text,
                        //        strNumeroTelefeno, // txtCodigo.Text.Trim(),
                        //        ((Label)Step4.ContentTemplateContainer.FindControl("lblRecibo")).Text,
                        //        ObtenerFecha(((Label)Step4.ContentTemplateContainer.FindControl("lblFecha")).Text.ToString()).ToString(),
                        //        Moneda,
                        //        TotalPagar.ToString(),
                        //        CodigoMenuItem, strNombreFormulario, TipoCambio.ToString(), ITF.ToString(), Comision.ToString(), strDescripcion);
                        //}
                        //catch //(Exception ex)
                        //{
                        //    //ExceptionPolicy.HandleException(ex, "Hide");
                        //}

                        //DataTable dt;
                        //bool existeTarjeta = false;
                        //BL.General _objGeneral = new BL.General();
                        //try
                        //{
                        //    string codigoCliente = (string)Session["strCodigoClienteIBS"];
                        //    dt = _objGeneral.ObtenerDatosFrecuentes(codigoCliente, (string)((int)BL.DatosFrecuentes.TelefonoMovil).ToString(), (int)BL.Estado.Habilitado);
                        //    if (dt.Rows.Count > 0)
                        //        for (int i = 0; i < dt.Rows.Count; i++)
                        //            if (strNumeroTelefeno.ToString().Trim().TrimEnd() == dt.Rows[i]["strValor"].ToString())
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
                        //    HtmlTable paso3TablaTarjeta1 = (HtmlTable)Step4.ContentTemplateContainer.FindControl("tblGuardarTarjeta1");
                        //    paso3TablaTarjeta1.Style.Add("display", "block");
                        //}
                        //this.Botonera1.MostrarBotonera(true);
                    }
                    else
                    {

                        //OcultarInformacionStep4(Step4, false);
                        //try
                        //{
                        //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Advertencia", _strError, HBC.ListImages.AdvertenciaPeligro);
                        //}
                        //catch (Exception ex)
                        //{
                        //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Advertencia", ex.Message, HBC.ListImages.AdvertenciaPeligro);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                _strError = "6666";
                strTrama = string.Empty;
                dsHeader = null;
                //GeneraEntrada(ex.Message, string.Concat("Exception PagarConTrama7031", HttpContext.Current.Session["strPad"].ToString().Trim()));
                //MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Advertencia", "6666", HBC.ListImages.AdvertenciaPeligro);
            }


        }

        private void PagarConTrama7054(int intCodigoServicio, out string _strError, out string strTrama, out DataSet dsHeader, string strNumeroTelefeno, string strCuentaAcargo, string strNombreCliente, string strImporteRecibo, string strMoneda, string strCodigoProducto, string strEstadoDeuda, string TipoDocumento, string strNumeroDocumento, string strReferenciaDeuda, string strFechaEmision, string strFechaVencimiento, string ref1, string ref2, string strTraceReca, string strHoraReca, string strGlosa, string strIndicadorProceso)
        {
            try
            {
                BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();
                using (DataSet dsSalida = ObjPagoServicios.EjecutarTransaccion(294, intCodigoServicio, out _strError, out strTrama, out dsHeader, strNumeroTelefeno, strCuentaAcargo, strNombreCliente, strImporteRecibo, strMoneda, strCodigoProducto, strEstadoDeuda, TipoDocumento, strNumeroDocumento, strReferenciaDeuda, strFechaEmision, strFechaVencimiento, ref1, ref2, strTraceReca, strHoraReca, strGlosa, strIndicadorProceso))
                {
                    if (_strError == "0000")
                    {

                        CodOperacionGenerado = "22156";
                        FechaOperacion = DateTime.Now;
                        //base.RefillData();
                        //DataView dvMoneda = ListaMonedas.DefaultView; //(DataView)Session["dvMoneda"];
                        //string strPrefixMoneda = string.Empty;
                        //((Label)Step4.ContentTemplateContainer.FindControl("lblNumeroOperacion")).Text = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString();
                        //ObtenerDatosStep4();
                        //try
                        //{
                        //    base.InsertaLogPagoServicios(
                        //         BE.TipoTransaccion.PagoServicios(),
                        //         BE.TipoOperacion.Transaccion(),
                        //         BE.DefaultValues.PAGO_SERVICIOS(),
                        //         strTrama,
                        //         dsSalida,
                        //         _strError,
                        //         dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(),
                        //          NumeroCuenta, // ddlCuenta.SelectedValue.Substring(4, 12).ToString(),
                        //        DescripcionCuenta, // ddlCuenta.SelectedItem.Text,
                        //         dsSalida.Tables["OData"].Rows[0]["ODmtocv"].ToString(),
                        //         MonedaCuenta, // strMonedaOrigen,
                        //         CodigoProveedor, //ddlProveedor.SelectedValue,
                        //         NombreProveedor, //ddlProveedor.SelectedItem.Text,
                        //         CodigoServicioTemporal,//ddlServicio.SelectedValue,
                        //         NombreServicio, // ddlServicio.SelectedItem.Text,
                        //         "0", //ddlSector.SelectedValue,
                        //         string.Concat("|", NombreCliente, " ", strNumeroTelefeno, "|"),//ddlSector.SelectedItem.Text,
                        //         strNumeroTelefeno, // txtCodigo.Text.Trim(),
                        //         ((Label)Step4.ContentTemplateContainer.FindControl("lblRecibo")).Text,
                        //         ((Label)Step4.ContentTemplateContainer.FindControl("lblFecha")).Text.ToString(),
                        //         Moneda,
                        //         TotalPagar.ToString(),
                        //         CodigoMenuItem, strNombreFormulario, TipoCambio.ToString(), ITF.ToString(), Comision.ToString(), strGlosa);
                        //}
                        //catch //(Exception ex)
                        //{
                        //    //ExceptionPolicy.HandleException(ex, "Hide");
                        //}

                        //DataTable dt;
                        //bool existeTarjeta = false;
                        //BL.General _objGeneral = new BL.General();
                        //try
                        //{
                        //    string codigoCliente = (string)Session["strCodigoClienteIBS"];
                        //    dt = _objGeneral.ObtenerDatosFrecuentes(codigoCliente, (string)((int)BL.DatosFrecuentes.TelefonoMovil).ToString(), (int)BL.Estado.Habilitado);
                        //    if (dt.Rows.Count > 0)
                        //        for (int i = 0; i < dt.Rows.Count; i++)
                        //            if (strNumeroTelefeno.ToString().Trim().TrimEnd() == dt.Rows[i]["strValor"].ToString())
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
                        //    HtmlTable paso3TablaTarjeta1 = (HtmlTable)Step4.ContentTemplateContainer.FindControl("tblGuardarTarjeta1");
                        //    paso3TablaTarjeta1.Style.Add("display", "block");
                        //}
                        //this.Botonera1.MostrarBotonera(true);
                    }
                    else
                    {
                        //OcultarInformacionStep4(Step4, false);
                        //try
                        //{
                        //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Advertencia", _strError, HBC.ListImages.AdvertenciaPeligro);
                        //}
                        //catch (Exception ex)
                        //{
                        //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Advertencia", ex.Message, HBC.ListImages.AdvertenciaPeligro);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                _strError = "6666";
                strTrama = string.Empty;
                dsHeader = null;
                //GeneraEntrada(ex.Message, string.Concat("Exception PagarConTrama7054", HttpContext.Current.Session["strPad"].ToString().Trim()));
                //MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Advertencia", "6666", HBC.ListImages.AdvertenciaPeligro);
            }
        }

        public async Task AccionTelefonoFijo()
        {
            NavigationParameters parametros = GetNavigationParameters();
            parametros.Add(Constantes.pageOrigen, Constantes.pageTransfConfirmacion);

            Operacion operacion = parametros["Operacion"] as Operacion;
            SubOperacion suboperacion = parametros["SubOperacion"] as SubOperacion;

            Catalogo empresa = parametros[Constantes.keyEmpresa] as Catalogo;
            Servicio servicio = parametros[Constantes.keyServicio] as Servicio;
            string codServicio = parametros[Constantes.keyCodigoServicio] as string;
            Recibo recibo = parametros[Constantes.keyReciboIBS] as Recibo;
            DetalleRecibo detRecibo = parametros[Constantes.keyDetalleReciboIBS] as DetalleRecibo;

            Cuenta ctaOrigen = parametros["CtaCargo"] as Cuenta;

            decimal montoDec = decimal.Parse(detRecibo.Monto);
            string moneda = "PEN";

            //seccion IBS
            //DropDownList ddlproveedor = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlProveedor");
            //DropDownList ddlCuentas = (DropDownList)Step2.ContentTemplateContainer.FindControl("ddlCuentas");
            //TextBox txtDescripcion = (TextBox)Step2.ContentTemplateContainer.FindControl("txtDescripcion");
            //string strDescripcion = (txtDescripcion.Text.ToString() != string.Empty) ? txtDescripcion.Text.ToString().PadRight(30, ' ') : "".PadRight(30, ' ');
            //string strNumero = string.Empty;
            //using (HBC.ComboBoxText CtrlNumerosuministros = (HBC.ComboBoxText)Step1.ContentTemplateContainer.FindControl("ComboBoxText1"))
            //    strNumero = CtrlNumerosuministros.Text;
            //BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();
            //DataSet dsHeader = new DataSet();
            //string strNumeroTelefeno = strNumero.PadRight(10, ' ');
            //string strNumeroRecibo = NumeroRecibo.PadLeft(10, '0');
            //string strFechaEmision = Fecha.PadRight(8, '0');
            //string strMoneda = Moneda.PadRight(3, '0');
            //string strImporteRecibo = Convert.ToString(System.Math.Round(TotalPagar, 2) * 100).PadLeft(14, '0');  //string.Format("{0:N}", TotalPagar).ToString().Replace(".", "").PadLeft(14, '0');
            //string strCuentaCargo = ddlCuentas.SelectedValue.ToString().PadLeft(12, '0');
            //string NombreAbonado = NombreCliente.ToString().PadRight(30, ' ');
            //string strTrama = "";
            //CodigoServicioTemporal = (int.Parse(CodigoServicio) == 0) ? CodigoServicioTemporal : CodigoServicio;
            //using (DataSet dsSalida = ObjPagoServicios.EjecutarTransaccion(230, int.Parse(CodigoServicioTemporal), out _strError, out strTrama, out dsHeader, strNumeroTelefeno, strNumeroRecibo, strFechaEmision, strMoneda, strImporteRecibo, strCuentaCargo, CodigoSecuencia, strDescripcion, NombreAbonado, "0")) //(DataSet)ViewState["dsSalidaglobal"]) //
            //{
            //    if (_strError == "0000")
            //    {
            //        //base.RefillData();
            //        CodOperacionGenerado = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(); //["OHlogtx"].ToString();

            //        //((Label)Step4.ContentTemplateContainer.FindControl("lblNumeroOperacion")).Text = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(); //["OHlogtx"].ToString();
            //        //ObtenerDatosStep4();
            //        //try
            //        //{
            //        //    base.InsertaLogPagoServicios(
            //        //    BE.TipoTransaccion.PagoServicios(),
            //        //    BE.TipoOperacion.Transaccion(),
            //        //    BE.DefaultValues.PAGO_SERVICIOS(),
            //        //    strTrama,
            //        //    dsSalida,
            //        //    _strError,
            //        //    dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(),
            //        //    NumeroCuenta, // ddlCuenta.SelectedValue.Substring(4, 12).ToString(),
            //        //    DescripcionCuenta, // ddlCuenta.SelectedItem.Text,
            //        //    dsSalida.Tables["OData"].Rows[0]["ODmtocv"].ToString(),
            //        //    MonedaCuenta, // strMonedaOrigen,
            //        //    CodigoProveedor, //ddlProveedor.SelectedValue,
            //        //    NombreProveedor, //ddlProveedor.SelectedItem.Text,
            //        //    CodigoServicioTemporal,//ddlServicio.SelectedValue,
            //        //    NombreServicio, // ddlServicio.SelectedItem.Text,
            //        //    "0", //ddlSector.SelectedValue,
            //        //    string.Concat("|", NombreCliente, " ", strNumeroTelefeno, "|"),//ddlSector.SelectedItem.Text,
            //        //    strNumeroTelefeno, // txtCodigo.Text.Trim(),
            //        //    ((Label)Step4.ContentTemplateContainer.FindControl("lblRecibo")).Text,
            //        //    ((Label)Step4.ContentTemplateContainer.FindControl("lblFecha")).Text.ToString(),
            //        //    Moneda,
            //        //    TotalPagar.ToString(),
            //        //    CodigoMenuItem, strNombreFormulario, TipoCambio.ToString(), ITF.ToString(), Comision.ToString(), strDescripcion);
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    //ExceptionPolicy.HandleException(ex, "Hide");
            //        //}
            //        //DataTable dt;
            //        //bool existeTarjeta = false;
            //        //BL.General _objGeneral = new BL.General();
            //        //try
            //        //{
            //        //    string codigoCliente = (string)Session["strCodigoClienteIBS"];
            //        //    dt = _objGeneral.ObtenerDatosFrecuentes(codigoCliente, (string)((int)BL.DatosFrecuentes.TelefonoFijos).ToString(), (int)BL.Estado.Habilitado);
            //        //    if (dt.Rows.Count > 0)
            //        //        for (int i = 0; i < dt.Rows.Count; i++)
            //        //            if (strNumeroTelefeno.ToString().Trim().TrimEnd() == dt.Rows[i]["strValor"].ToString())
            //        //            {
            //        //                existeTarjeta = true;
            //        //                break;
            //        //            }
            //        //}
            //        //catch (Exception)
            //        //{ }
            //        //finally
            //        //{ dt = null; }

            //        //if (!existeTarjeta)
            //        //{
            //        //    HtmlTable paso3TablaTarjeta1 = (HtmlTable)Step4.ContentTemplateContainer.FindControl("tblGuardarTarjeta1");
            //        //    paso3TablaTarjeta1.Style.Add("display", "block");
            //        //}
            //        //this.Botonera1.MostrarBotonera(true);
            //    }
            //    else
            //    {
            //        //Step4.ContentTemplateContainer.FindControl("TablaStep4Datos1").Visible = false;
            //        //Step4.ContentTemplateContainer.FindControl("TablaStep4Cabecera1").Visible = false;
            //        //OcultarInformacionStep4(Step4, false);
            //        //try
            //        //{
            //        //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Estimado Cliente", _strError, HBC.ListImages.AdvertenciaPeligro);
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Estimado Cliente", ex.Message, HBC.ListImages.AdvertenciaPeligro);
            //        //}
            //    }

            //    //Inicio PRN:80290 TAR:00010 AP:MARCAT  
            //   // InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), BE.TipoTransaccion.PagoServicios(), BE.TipoOperacionesMonitoreo.PagoServiciosTelefoniaFija(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
            //    //Fin PRN:80290 TAR:00010 AP:MARCAT  
            //}
            //

            //acciones mock
            string rptaTrx = CuentaService.efectuarMovimiento(ctaOrigen, montoDec, moneda, false);

            if (rptaTrx != "")
            {
                await DialogService.DisplayAlertAsync(Constantes.MSJ_INFO, rptaTrx, Constantes.MSJ_BOTON_OK);
            }
            else
            {
                RegistrarOperacionFrecuente(FechaOperacion);

                parametros.Add(Constantes.keyCodOperacionGenerado, CodOperacionGenerado);
                parametros.Add(Constantes.keyFechaOperacion, FechaOperacion);

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

                await NavigationService.NavigateAsync(retroceso + Constantes.pagePagoServResumen, parametros);

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
