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
            DetalleRecibo detReciboIBS = RefNavParameters[Constantes.keyDetalleReciboIBS] as DetalleRecibo;
            Catalogo empresa = RefNavParameters[Constantes.keyEmpresa] as Catalogo;
            Servicio servicio = RefNavParameters[Constantes.keyServicio] as Servicio;
            string codigoServicio = RefNavParameters[Constantes.keyCodigoServicio] as string;

            Cuenta ctaOrigen = parametros[Constantes.keyCtaCargo] as Cuenta;

            LblNombreCta1 = ctaOrigen.NombreCta;
            LblCodCta1 = ctaOrigen.CodigoCta;

            LblNomEmpresa = empresa.Nombre;
            LblNomServicio = servicio.Nombre;

            LblNomCliente = reciboIBS.NombreCliente;
            LblCodServicio = codigoServicio;

            LblMonedaMonto = CatalogoService.BuscarMonedaPorCodigo("PEN").Descripcion + " " + detReciboIBS.Monto;

        }

        private string _lblMonedaMonto;
        public string LblMonedaMonto
        {
            get { return _lblMonedaMonto; }
            set { SetProperty(ref _lblMonedaMonto, value); }
        }

        private string _lblNomEmpresa;
        public string LblNomEmpresa
        {
            get { return _lblNomEmpresa; }
            set { SetProperty(ref _lblNomEmpresa, value); }
        }

        private string _lblNomServicio;
        public string LblNomServicio
        {
            get { return _lblNomServicio; }
            set { SetProperty(ref _lblNomServicio, value); }
        }

        private string _lblNomCliente;
        public string LblNomCliente
        {
            get { return _lblNomCliente; }
            set { SetProperty(ref _lblNomCliente, value); }
        }

        private string _lblCodServicio;
        public string LblCodServicio
        {
            get { return _lblCodServicio; }
            set { SetProperty(ref _lblCodServicio, value); }
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
                        await AccionLuz();
                    }
                    else if (empresa.Codigo == "5")//agua
                    {
                        await AccionAgua();
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
            parametros.Add(Constantes.keyMoneda, CatalogoService.BuscarMonedaPorCodigo("PEN"));

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
            using (DataSet CtrlInput = ObjPagoServicios.ObtenerControlesInput(CodigoServicioTemporal, 2))
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
                        PagarConTrama7031(CodigoServicioTemporal, out _strError, out strTrama, out dsHeader, strNumero, strNumeroRecibo, strFechaEmision, moneda, strImporteRecibo, strCuentaCargo, strDescripcion, strNombreAbonado);
                        if (_strError != "0000")
                        {
                            await DialogService.DisplayAlertAsync("Validación", "Código de error IBS: " + _strError, "OK");
                            return;
                        }
                        break;
                    case "7054":
                        strNumeroTelefeno = strNumero.PadRight(10, ' ');
                        strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' ');
                        strCuentaCargo = ctaOrigen.CodigoCta.ToString().PadLeft(12, '0');
                        strNombreCliente = "Juan Perez";
                        strImporteRecibo = Convert.ToString(System.Math.Round(montoDec, 2) * 100).PadLeft(11, '0'); // string.Format("{0:N2}", TotalPagar).ToString().Replace(".", "").PadLeft(11, '0');
                        //PagarConTrama7054(int.Parse(CodigoServicioTemporal), out _strError, out strTrama, out dsHeader, strNumeroTelefeno, strCuentaCargo, strNombreCliente, strImporteRecibo, moneda, CodigoProducto, EstadoDeuda, TipoDocumento, NumeroDocumento, NumeroReferencia, FechaEmision, FechaVencimiento, "".PadLeft(12, ' '), "".PadLeft(12, ' '), "".PadLeft(6, ' '), "".PadLeft(6, ' '), strDescripcion, "0");
                        PagarConTrama7054(CodigoServicioTemporal, out _strError, out strTrama, out dsHeader, strNumeroTelefeno, strCuentaCargo, strNombreCliente, strImporteRecibo, moneda, "", "", "", "", "", "", "", "".PadLeft(12, ' '), "".PadLeft(12, ' '), "".PadLeft(6, ' '), "".PadLeft(6, ' '), strDescripcion, "0");
                        if (_strError != "0000")
                        {
                            await DialogService.DisplayAlertAsync("Validación", "Código de error IBS: " + _strError, "OK");
                            return;
                        }
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
                        PagarConTrama7031(CodigoServicioTemporal, out _strError, out strTrama, out dsHeader, strNumero, strNumeroRecibo, strFechaEmision, moneda, strImporteRecibo, strCuentaCargo, strDescripcion, strNombreAbonado);
                        if(_strError != "0000")
                        {
                            await DialogService.DisplayAlertAsync("Validación", "Código de error IBS: " + _strError, "OK");
                            return;
                        }
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
                    retroceso = "../../../../../../";
                }
                else
                {
                    retroceso = "../../../../";
                }

                await NavigationService.NavigateAsync(retroceso + Constantes.pagePagoServResumen, parametros);

            }
        }
        
        private void PagarConTrama7031(string intCodigoServicio, out string _strError, out string strTrama, out DataSet dsHeader, string strNumeroTelefeno, string strNumeroRecibo, string strFechaEmision, string strMoneda, string strImporteRecibo, string strCuentaCargo, string strDescripcion, string strNombreAbonado)
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
                        _strError = "7777";
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

        private void PagarConTrama7054(string intCodigoServicio, out string _strError, out string strTrama, out DataSet dsHeader, string strNumeroTelefeno, string strCuentaAcargo, string strNombreCliente, string strImporteRecibo, string strMoneda, string strCodigoProducto, string strEstadoDeuda, string TipoDocumento, string strNumeroDocumento, string strReferenciaDeuda, string strFechaEmision, string strFechaVencimiento, string ref1, string ref2, string strTraceReca, string strHoraReca, string strGlosa, string strIndicadorProceso)
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
                        _strError = "7777";
                        //     await DialogService.DisplayAlertAsync("Validación", "Código de error IBS: " + _strError, "OK");
                        //return;
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
            parametros.Add(Constantes.keyMoneda, CatalogoService.BuscarMonedaPorCodigo("PEN"));

            Operacion operacion = parametros["Operacion"] as Operacion;
            SubOperacion suboperacion = parametros["SubOperacion"] as SubOperacion;

            Catalogo empresa = parametros[Constantes.keyEmpresa] as Catalogo;
            Servicio servicio = parametros[Constantes.keyServicio] as Servicio;
            string codServicio = parametros[Constantes.keyCodigoServicio] as string;
            Recibo recibo = parametros[Constantes.keyReciboIBS] as Recibo;
            DetalleRecibo detRecibo = parametros[Constantes.keyDetalleReciboIBS] as DetalleRecibo;

            Cuenta ctaCargo = parametros["CtaCargo"] as Cuenta;

            decimal montoDec = decimal.Parse(detRecibo.Monto);
            string moneda = "PEN";

            //seccion IBS
            //DropDownList ddlproveedor = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlProveedor");
            //DropDownList ddlCuentas = (DropDownList)Step2.ContentTemplateContainer.FindControl("ddlCuentas");
            //TextBox txtDescripcion = (TextBox)Step2.ContentTemplateContainer.FindControl("txtDescripcion");
            string strDescripcion = "";// (txtDescripcion.Text.ToString() != string.Empty) ? txtDescripcion.Text.ToString().PadRight(30, ' ') : "".PadRight(30, ' ');
            string strNumero = string.Empty;
            //using (HBC.ComboBoxText CtrlNumerosuministros = (HBC.ComboBoxText)Step1.ContentTemplateContainer.FindControl("ComboBoxText1"))
            strNumero = codServicio;//CtrlNumerosuministros.Text;
            BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();
            DataSet dsHeader = new DataSet();
            string strNumeroTelefeno = strNumero.PadRight(10, ' ');
            string strNumeroRecibo = detRecibo.IdDetalleRecibo.PadLeft(10, '0');
            string strFechaEmision = detRecibo.FechaEmision.PadRight(8, '0');
            string strMoneda = moneda.PadRight(3, '0');
            string strImporteRecibo = Convert.ToString(System.Math.Round(montoDec, 2) * 100).PadLeft(14, '0');  //string.Format("{0:N}", TotalPagar).ToString().Replace(".", "").PadLeft(14, '0');
            string strCuentaCargo = ctaCargo.CodigoCta.ToString().PadLeft(12, '0');
            string NombreAbonado = recibo.NombreCliente.ToString().PadRight(30, ' ');
            string strTrama = "";
            CodigoServicioTemporal = (int.Parse(CodigoServicio) == 0) ? CodigoServicioTemporal : CodigoServicio;
            CodigoServicioTemporal = empresa.Codigo + servicio.Codigo;//aniadido temporalmente
            using (DataSet dsSalida = ObjPagoServicios.EjecutarTransaccion(230, CodigoServicioTemporal, out _strError, out strTrama, out dsHeader, strNumeroTelefeno, strNumeroRecibo, strFechaEmision, strMoneda, strImporteRecibo, strCuentaCargo, "", strDescripcion, NombreAbonado, "0")) //(DataSet)ViewState["dsSalidaglobal"]) //
            {
                if (_strError == "0000")
                {
                    //base.RefillData();
                    CodOperacionGenerado = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(); //["OHlogtx"].ToString();
                    FechaOperacion = DateTime.Now;
                    //((Label)Step4.ContentTemplateContainer.FindControl("lblNumeroOperacion")).Text = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(); //["OHlogtx"].ToString();
                    //ObtenerDatosStep4();
                    //try
                    //{
                    //    base.InsertaLogPagoServicios(
                    //    BE.TipoTransaccion.PagoServicios(),
                    //    BE.TipoOperacion.Transaccion(),
                    //    BE.DefaultValues.PAGO_SERVICIOS(),
                    //    strTrama,
                    //    dsSalida,
                    //    _strError,
                    //    dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(),
                    //    NumeroCuenta, // ddlCuenta.SelectedValue.Substring(4, 12).ToString(),
                    //    DescripcionCuenta, // ddlCuenta.SelectedItem.Text,
                    //    dsSalida.Tables["OData"].Rows[0]["ODmtocv"].ToString(),
                    //    MonedaCuenta, // strMonedaOrigen,
                    //    CodigoProveedor, //ddlProveedor.SelectedValue,
                    //    NombreProveedor, //ddlProveedor.SelectedItem.Text,
                    //    CodigoServicioTemporal,//ddlServicio.SelectedValue,
                    //    NombreServicio, // ddlServicio.SelectedItem.Text,
                    //    "0", //ddlSector.SelectedValue,
                    //    string.Concat("|", NombreCliente, " ", strNumeroTelefeno, "|"),//ddlSector.SelectedItem.Text,
                    //    strNumeroTelefeno, // txtCodigo.Text.Trim(),
                    //    ((Label)Step4.ContentTemplateContainer.FindControl("lblRecibo")).Text,
                    //    ((Label)Step4.ContentTemplateContainer.FindControl("lblFecha")).Text.ToString(),
                    //    Moneda,
                    //    TotalPagar.ToString(),
                    //    CodigoMenuItem, strNombreFormulario, TipoCambio.ToString(), ITF.ToString(), Comision.ToString(), strDescripcion);
                    //}
                    //catch (Exception ex)
                    //{
                    //    //ExceptionPolicy.HandleException(ex, "Hide");
                    //}
                    //DataTable dt;
                    //bool existeTarjeta = false;
                    //BL.General _objGeneral = new BL.General();
                    //try
                    //{
                    //    string codigoCliente = (string)Session["strCodigoClienteIBS"];
                    //    dt = _objGeneral.ObtenerDatosFrecuentes(codigoCliente, (string)((int)BL.DatosFrecuentes.TelefonoFijos).ToString(), (int)BL.Estado.Habilitado);
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
                    await DialogService.DisplayAlertAsync("Validación", "Código de error IBS: " + _strError, "OK");
                    return;
                    //Step4.ContentTemplateContainer.FindControl("TablaStep4Datos1").Visible = false;
                    //Step4.ContentTemplateContainer.FindControl("TablaStep4Cabecera1").Visible = false;
                    //OcultarInformacionStep4(Step4, false);
                    //try
                    //{
                    //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Estimado Cliente", _strError, HBC.ListImages.AdvertenciaPeligro);
                    //}
                    //catch (Exception ex)
                    //{
                    //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Estimado Cliente", ex.Message, HBC.ListImages.AdvertenciaPeligro);
                    //}
                }
                
                // InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), BE.TipoTransaccion.PagoServicios(), BE.TipoOperacionesMonitoreo.PagoServiciosTelefoniaFija(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
                
            }

            //acciones mock
            string rptaTrx = CuentaService.efectuarMovimiento(ctaCargo, montoDec, moneda, false);

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
                    retroceso = "../../../../../../";
                }
                else
                {
                    retroceso = "../../../../";
                }

                await NavigationService.NavigateAsync(retroceso + Constantes.pagePagoServResumen, parametros);

            }
        }

        public async Task AccionLuz()
        {
            NavigationParameters parametros = GetNavigationParameters();
            parametros.Add(Constantes.pageOrigen, Constantes.pagePagoServConfirmacion);
            parametros.Add(Constantes.keyMoneda, CatalogoService.BuscarMonedaPorCodigo("PEN"));

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
            string strTrama = string.Empty;
            //DropDownList ddlCuentaOrigen = (DropDownList)Step2.ContentTemplateContainer.FindControl("ddlcuentas");
            //DropDownList ddlProveedor = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlProveedor");
            //DropDownList ddlServicio = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddServicios1");
            //HBC.ComboBoxText txtCodigo = (HBC.ComboBoxText)Step1.ContentTemplateContainer.FindControl("ComboBoxText1");
            //TextBox txtDescripcion = (TextBox)Step2.ContentTemplateContainer.FindControl("txtDescripcion");
            //Label lblProveedor = ((Label)Step2.ContentTemplateContainer.FindControl("lblProveedor"));
            //Label lblServicio = ((Label)Step2.ContentTemplateContainer.FindControl("lblServicio"));
            //Label lblCodigo = ((Label)Step2.ContentTemplateContainer.FindControl("lblCodigo"));
            //DropDownList ddlCuenta = (DropDownList)Step2.ContentTemplateContainer.FindControl("ddlcuentas");
            //Label lblProveedorFin = ((Label)Step3.ContentTemplateContainer.FindControl("lblProveedor"));
            //Label lblServicioFin = ((Label)Step3.ContentTemplateContainer.FindControl("lblServicio"));
            string strPendiente = detRecibo.IdDetalleRecibo.ToString().PadLeft(10, '0') + detRecibo.FechaVencimiento.ToString() + moneda.ToString() + Convert.ToString(System.Math.Round(montoDec, 2) * 100).PadLeft(14, '0'); // ObtenerPendientePago();
            DataSet dsHeader = new DataSet();
            BL.PagoServicios ObjPagoServicio = new BL.PagoServicios();
            //BL.General ObjGeneral = new BL.General();
            string strMensaje = codServicio.PadLeft(7, '0').PadRight(10, ' ') + strPendiente + ctaOrigen.CodigoCta.PadLeft(12, '0') + "" + "".ToString().PadRight(30, ' ') + recibo.NombreCliente.ToString().PadRight(30, ' ');
            CodigoServicioTemporal = (int.Parse(CodigoServicio) == 0) ? CodigoServicioTemporal : CodigoServicio;
            CodigoServicioTemporal = empresa.Codigo + servicio.Codigo;//aniadido temporalmente
            using (DataSet dsSalida = ObjPagoServicio.EjecutarTransaccion(230, CodigoServicioTemporal, out _strError, out strTrama, out dsHeader, strMensaje, "0"))
            {
                if (_strError == "0000")
                {
                    //base.RefillData();
                    //DataView dvMoneda = (DataView)Session["dvMoneda"];
                    CodOperacionGenerado = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(); //["OHlogtx"].ToString();
                    FechaOperacion = DateTime.Now;
                    //((Label)Step4.ContentTemplateContainer.FindControl("lblNumeroOperacion")).Text = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString();
                    //ObtenerDatosStep4();
                    try
                    {
                        //DataView dvCuentas = ((DataTable)ViewState["dtCuentas"]).DefaultView;
                        //dvCuentas.RowFilter = "ODcodct = '" + ddlCuenta.SelectedValue + "'";
                        //string strMonedaOrigen = dvCuentas[0]["ODDmoned"].ToString();
                        //base.InsertaLogPagoServicios(
                        //BE.TipoTransaccion.PagoServicios(),
                        //BE.TipoOperacion.Transaccion(),
                        //BE.DefaultValues.PAGO_SERVICIOS(),
                        //strTrama,
                        //dsSalida,
                        //_strError,
                        //dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(),
                        //ddlCuenta.SelectedValue.ToString(),
                        //ddlCuenta.SelectedItem.Text,
                        //dsSalida.Tables["OData"].Rows[0]["ODmtocv"].ToString(),
                        //MonedaCuenta,
                        //ddlProveedor.SelectedValue,
                        //ddlProveedor.SelectedItem.Text,
                        //ddlServicio.SelectedValue,
                        //ddlServicio.SelectedItem.Text,
                        //"0", //ddlSector.SelectedValue,
                        //string.Concat("|", NombreCliente, " ", txtCodigo.Text.ToString(), "|"),//ddlSector.SelectedItem.Text,
                        //txtCodigo.Text.Trim(),
                        //((Label)Step4.ContentTemplateContainer.FindControl("lblRecibo")).Text,
                        //((Label)Step4.ContentTemplateContainer.FindControl("lblFecha")).Text.ToString(),
                        //Moneda,
                        //TotalPagar.ToString(),
                        //CodigoMenuItem.ToString(),
                        //strNombreFormulario, TipoCambio.ToString(), ITF.ToString(), Comision.ToString(), txtDescripcion.Text);

                    }
                    catch //(Exception ex)
                    {
                        //ExceptionPolicy.HandleException(ex, "Hide");
                    }
                    //DataTable dt;
                    //bool existeTarjeta = false;
                    //BL.General _objGeneral = new BL.General();
                    //try
                    //{
                    //    string codigoCliente = (string)Session["strCodigoClienteIBS"];
                    //    dt = _objGeneral.ObtenerDatosFrecuentes(codigoCliente, (string)((int)BL.DatosFrecuentes.Luz).ToString(), (int)BL.Estado.Habilitado);
                    //    //Session["ds_botonera"] = dt;
                    //    if (dt.Rows.Count > 0)
                    //        for (int i = 0; i < dt.Rows.Count; i++)
                    //            if (txtCodigo.Text.ToString().TrimEnd().Trim() == dt.Rows[i]["strValor"].ToString())
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
                    await DialogService.DisplayAlertAsync("Validación", "Código de error IBS: " + _strError, "OK");
                    return;
                    //OcultarInformacionStep4(Step4, false);
                    //try
                    //{
                    //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Estimado Cliente", _strError, HBC.ListImages.AdvertenciaPeligro);
                    //}
                    //catch (Exception ex)
                    //{
                    //    MostrarMensaje(((HBC.Mensaje)Step4.ContentTemplateContainer.FindControl("Mensaje3")), "Estimado Cliente", ex.Message, HBC.ListImages.AdvertenciaPeligro);
                    //}

                }

                //Inicio PRN:80290 TAR:00010 AP:MARCAT  
                //InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), BE.TipoTransaccion.PagoServicios(), BE.TipoOperacionesMonitoreo.PagoServiciosLuz(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
                //Fin PRN:80290 TAR:00010 AP:MARCAT  
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
                    retroceso = "../../../../../../";
                }
                else
                {
                    retroceso = "../../../../";
                }

                await NavigationService.NavigateAsync(retroceso + Constantes.pagePagoServResumen, parametros);

            }
        }

        public async Task AccionAgua()
        {
            NavigationParameters parametros = GetNavigationParameters();
            parametros.Add(Constantes.pageOrigen, Constantes.pagePagoServConfirmacion);
            parametros.Add(Constantes.keyMoneda,CatalogoService.BuscarMonedaPorCodigo("PEN"));

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
            //DropDownList ddlCuentas = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlCuentas");
            ////TextBox txtSuministro = (TextBox)Step1.ContentTemplateContainer.FindControl("txtNumeroSuministro");
            //HBC.ComboBoxText txtSuministro = (HBC.ComboBoxText)Step1.ContentTemplateContainer.FindControl("ComboBoxText1");
            //TextBox txtFechaVencimiento = (TextBox)Step1.ContentTemplateContainer.FindControl("txtFechaVencimiento");
            //TextBox txtReferencia = (TextBox)Step1.ContentTemplateContainer.FindControl("txtReferencia");
            //TextBox txtImporte = (TextBox)Step1.ContentTemplateContainer.FindControl("txtImporte");
            //TextBox txtDescripcion = (TextBox)Step1.ContentTemplateContainer.FindControl("txtDescripcion");
            string strDescripicon = "";// (txtDescripcion.Text.ToString() != string.Empty) ? txtDescripcion.Text.ToString() : string.Empty;
            BL.PagoServicios ObjServicios = new BL.PagoServicios();
            DataSet dsHeader = new DataSet();
            string strTrama = "";
            //string strMensaje = txtSuministro.Text.Trim().ToString() + txtImporte.ToString().Trim().PadLeft(14, '0') + Session["strNombreCliente"].Trim().PadRight(30, ' ') + txtFechaVencimiento.Text.Trim().Replace("/","") + "00000000000000" + txtReferencia.Text.Trim().PadLeft(15, '0') + Moneda.ToString() + "0000000000" + ddlCuentas.SelectedValue.ToString().Substring(4, 12);
            CodigoServicioTemporal = (int.Parse(CodigoServicio) == 0) ? CodigoServicioTemporal : CodigoServicio;
            CodigoServicioTemporal = empresa.Codigo + servicio.Codigo;//aniadido temporalmente
            using (DataSet dsSalida = ObjServicios.EjecutarTransaccion(250, CodigoServicioTemporal, out _strError, out strTrama, out dsHeader, codServicio.ToString() + Convert.ToString(System.Math.Round(double.Parse(detRecibo.Monto), 2) * 100).PadLeft(14, '0') + "".ToString().Trim().PadRight(31, ' ').Substring(0, 30) + detRecibo.FechaVencimiento.Trim().Replace("/", "") + "".Trim().PadLeft(15, '0') + ctaOrigen.CodigoCta.PadLeft(12, '0'), "".ToString().PadRight(30, ' '), "0"))
            {
                if (_strError == "0000")
                {
                    //Moneda a Cargar 
                    //base.RefillData();
                    CodOperacionGenerado = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(); //["OHlogtx"].ToString();
                    FechaOperacion = DateTime.Now;
                    //string strPrefixMonedaCargado = string.Empty;
                    //string strCodigoInternoMonedaCargado = string.Empty;
                    //DataView dvMoneda = ListaMonedas.DefaultView;
                    //DataTable dtCuentas = (DataTable)ViewState["dtCuentas"];
                    //DataRow[] drCuentas = dtCuentas.Select("ODcodct='" + ddlCuentas.SelectedValue.PadLeft(12, '0') + "'");
                    //dvMoneda.RowFilter = string.Empty;
                    //dvMoneda.RowFilter = "strCodigoInterno='" + drCuentas[0]["ODDmoned"].ToString() + "'";
                    //strPrefixMonedaCargado = dvMoneda[0]["strPrefijo"].ToString();
                    //strCodigoInternoMonedaCargado = drCuentas[0]["ODDmoned"].ToString();

                    //((Label)Step3.ContentTemplateContainer.FindControl("lblFecha")).Text = DateTime.Now.ToShortDateString();
                    //((Label)Step3.ContentTemplateContainer.FindControl("lblHora")).Text = DateTime.Now.ToShortTimeString();
                    //((Label)Step3.ContentTemplateContainer.FindControl("lblNumeroOperacion")).Text = dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString();
                    //using (Label CtrllblProveedor = (Label)Step3.ContentTemplateContainer.FindControl("lblproveedor")) CtrllblProveedor.Text = NombreProveedor;//string.Format("{0:N}", TotalPagar);
                    //using (Label Ctrllblservicio = (Label)Step3.ContentTemplateContainer.FindControl("lblservicio")) Ctrllblservicio.Text = ConceptoPago; //string.Format("{0:N}", TotalPagar);

                    //using (Label CtrlTipoCuentaNumero = (Label)Step3.ContentTemplateContainer.FindControl("lblTipoNumeroCuenta"))
                    //{
                    //    using (DropDownList CtrlCuenta = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlcuentas"))
                    //        CtrlTipoCuentaNumero.Text = CtrlCuenta.SelectedItem.Text.Split('(').GetValue(0).ToString();
                    //}
                    //using (Label CtrlMontoPagar = (Label)Step3.ContentTemplateContainer.FindControl("lblMontoPagar")) CtrlMontoPagar.Text = strPrefixMonedaCargado + " " + string.Format("{0:N}", TotalPagar);
                    //using (Label CtrllblITF = (Label)Step3.ContentTemplateContainer.FindControl("lblITF")) CtrllblITF.Text = strPrefixMonedaCargado + " " + string.Format("{0:N}", ITF);
                    //using (Label CtrllblComisiones = (Label)Step3.ContentTemplateContainer.FindControl("lblComisiones")) CtrllblComisiones.Text = strPrefixMonedaCargado + " " + string.Format("{0:N}", Comision);
                    //using (Label CtrllblTotalDebitar = (Label)Step3.ContentTemplateContainer.FindControl("lblTotalDebitar")) CtrllblTotalDebitar.Text = strPrefixMonedaCargado + " " + string.Format("{0:N}", (Comision + ITF + TotalPagar));
                    //using (Label CtrllblNumero = (Label)Step3.ContentTemplateContainer.FindControl("lblclienteNumero"))
                    //{
                    //    using (Label CtrlNumero = (Label)Step2.ContentTemplateContainer.FindControl("lblclienteNumero"))
                    //        CtrllblNumero.Text = CtrlNumero.Text;
                    //}
                    //using (Label CtrllblNumero = (Label)Step3.ContentTemplateContainer.FindControl("lblRecibo"))
                    //{
                    //    CtrllblNumero.Text = NumeroRecibo;
                    //}
                    //using (Label CtrllblMontoPrestamoPagar = (Label)Step3.ContentTemplateContainer.FindControl("lblMontoPagarSedapal")) CtrllblMontoPrestamoPagar.Text = ObtenerImporte(Moneda, TotalPagar.ToString());
                    //using (Label CtrllblTotalPrestamoPagar = (Label)Step3.ContentTemplateContainer.FindControl("lblTotalPagarSedapal")) CtrllblTotalPrestamoPagar.Text = ObtenerImporte(Moneda, TotalPagar.ToString());
                    //using (Label CtrllblDescripicion = (Label)Step3.ContentTemplateContainer.FindControl("lblDescripcion")) CtrllblDescripicion.Text = ((TextBox)Step1.ContentTemplateContainer.FindControl("txtDescripcion")).Text;

                    try
                    {
                        //string strDescripcion = string.Empty;
                        //string strCliente = string.Empty;
                        //using (Label CtrllblDescripicion = (Label)Step3.ContentTemplateContainer.FindControl("lblDescripcion")) strDescripcion = CtrllblDescripicion.Text;
                        //using (Label CtrllblNumero = (Label)Step3.ContentTemplateContainer.FindControl("lblclienteNumero")) strCliente = CtrllblNumero.Text;


                     //   base.InsertaLogPagoServicios(
                     //BE.TipoTransaccion.PagoServicios(),
                     //BE.TipoOperacion.Transaccion(),
                     //BE.DefaultValues.PAGO_SERVICIOS_RECAUDACIONES(),
                     //strTrama,
                     //dsSalida,
                     //_strError,
                     //dsHeader.Tables["OHead"].Rows[0]["OLOG"].ToString(),
                     //ddlCuentas.SelectedValue.PadLeft(12, '0'),
                     //ddlCuentas.SelectedItem.Text,
                     //txtImporte.Text.ToString(),
                     //MonedaCuenta,
                     //"93",
                     //"Sedapal",
                     //CodigoServicioTemporal,
                     //"Pago Sedapal",
                     //"0",
                     //string.Concat("|", NombreCliente, " ", NumeroRecibo, "|"),
                     //NumeroRecibo,
                     //txtReferencia.Text.ToString(),
                     //((Label)Step3.ContentTemplateContainer.FindControl("lblFecha")).Text.ToString(),
                     //Moneda,
                     //Comunes.FormateaNumero(txtImporte.Text.ToString()),
                     //CodigoMenuItem,
                     //strNombreFormulario, "0.0", ITF.ToString(), Comision.ToString(), strDescripcion);
                    }
                    catch //(Exception ex)
                    {
                        //ExceptionPolicy.HandleException(ex, "Hide");
                    }
                    //DataTable dt;
                    //bool existeTarjeta = false;
                    //BL.General _objGeneral = new BL.General();
                    //try
                    //{
                    //    string codigoCliente = (string)Session["strCodigoClienteIBS"];
                    //    dt = _objGeneral.ObtenerDatosFrecuentes(codigoCliente, (string)((int)BL.DatosFrecuentes.SuministrosSedapal).ToString(), (int)BL.Estado.Habilitado);
                    //    if (dt.Rows.Count > 0)
                    //        for (int i = 0; i < dt.Rows.Count; i++)
                    //            if (txtSuministro.Text.ToString().Trim().TrimEnd() == dt.Rows[i]["strValor"].ToString())
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
                    //this.Botonera1.MostrarBotonera(true);
                }
                else
                {
                    await DialogService.DisplayAlertAsync("Validación", "Código de error IBS: " + _strError, "OK");
                    return;
                    //OcultarInformacionStep3(Step3, false);
                    //try
                    //{
                    //    MostrarMensaje(((HBC.Mensaje)Step3.ContentTemplateContainer.FindControl("Mensaje3")), "Estimado Cliente", _strError, HBC.ListImages.AdvertenciaPeligro);
                    //}
                    //catch (Exception ex)
                    //{
                    //    MostrarMensaje(((HBC.Mensaje)Step3.ContentTemplateContainer.FindControl("Mensaje3")), "Estimado Cliente", ex.Message, HBC.ListImages.AdvertenciaPeligro);
                    //}
                }

                //Inicio PRN:80290 TAR:00010 AP:MARCAT  
                //InsertaLogMonitoreo(Session["strCodigoClienteIBS"].ToString(), Session["strPad"].ToString().PadRight(19, ' '), BE.TipoTransaccion.PagoServicios(), BE.TipoOperacionesMonitoreo.PagoServiciosAgua(), System.Web.HttpContext.Current.Session.SessionID.ToString(), 2, _strError, string.Empty);
                //Fin PRN:80290 TAR:00010 AP:MARCAT  

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
                    retroceso = "../../../../../../";
                }
                else
                {
                    retroceso = "../../../../";
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
                
                Catalogo moneda = CatalogoService.BuscarMonedaPorCodigo("PEN");
                Cuenta ctaOrigen = RefNavParameters[Constantes.keyCtaCargo] as Cuenta;
                Catalogo empresa = RefNavParameters[Constantes.keyEmpresa] as Catalogo;
                Servicio servicio = RefNavParameters[Constantes.keyServicio] as Servicio;
                string codServicio = RefNavParameters[Constantes.keyCodigoServicio] as string;
                Recibo recibo = RefNavParameters[Constantes.keyReciboIBS] as Recibo;
                DetalleRecibo detRecibo = RefNavParameters[Constantes.keyDetalleReciboIBS] as DetalleRecibo;


                OperacionFrecuente opeFrec = new OperacionFrecuente
                {
                    FechaOperacion = fechaOperacion,
                    SubOperacion = suboperacion,
                    Operacion = operacion,
                    NombreFrecuente = NomOpeFrec
                };

                opeFrec.CtaOrigen = ctaOrigen;
                opeFrec.Empresa = empresa;
                opeFrec.Servicio = servicio;
                opeFrec.Moneda = moneda;
                opeFrec.CodigoServicio = codServicio;
                opeFrec.Recibo = recibo;
                opeFrec.DetalleRecibo = detRecibo;

                OperacionService.AgregarOperacionFrecuente(opeFrec);
                EventAggregator.GetEvent<OpeFrecuenteAddedEvent>().Publish();
            }
        }

    }
}
