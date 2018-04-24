using Acr.UserDialogs;
using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using BL = NS.MBX_amarin.BusinessLogic;
using BE = NS.MBX_amarin.Common;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModels
{
	public class ServicioEmpresaViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private IReciboService ReciboService { get; set; }
        private string _strError = "";
        private string CodigoServicioTemporal;
        private string NombreServicio;
        private string CodigoServicio = "0";

        private Recibo ReciboIBS;//guarda el recibo que trae la consulta de servicio

        public ServicioEmpresaViewModel(IReciboService reciboService, ICatalogoService catalogoService, INavigationService navigationService, IUserDialogs userDialogs)
            : base(navigationService, userDialogs)
        {
            CatalogoService = catalogoService;
            ReciboService = reciboService;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                RefNavParameters = parameters;
                string pageOrigen = RefNavParameters[Constantes.pageOrigen] as string;
                Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;

                NomEmpresa = empresa.Nombre;

                ListaServicios = CatalogoService.ListarServiciosxEmpresa(empresa.Codigo);//numero de suministro

                //dependiendo de la empresa coloca el label
                if (empresa.Codigo == "0" || empresa.Codigo == "1" || empresa.Codigo == "2")
                {
                    LblCodigo = "Número de Teléfono";
                }
                else if (empresa.Codigo == "3" || empresa.Codigo == "4" || empresa.Codigo == "5")//3 y 4 son luz
                {
                    LblCodigo = "Número de Suministro";
                }

                //aqui si deberia estar la operacion frecuente
                if (pageOrigen == Constantes.pageOperaciones)
                {
                    OperacionFrecuente opeFrec = RefNavParameters[Constantes.keyOperacionFrecuente] as OperacionFrecuente;
                    ServicioSelected = ListaServicios.Where(p => p.Codigo == opeFrec.Servicio.Codigo).First();
                    Codigo = opeFrec.CodigoServicio;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            

        }

        private string _nomEmpresa;
        public string NomEmpresa
        {
            get { return _nomEmpresa; }
            set { SetProperty(ref _nomEmpresa, value); }
        }

        private ObservableCollection<Servicio> _listaServicios;
        public ObservableCollection<Servicio> ListaServicios
        {
            get { return _listaServicios; }
            set { SetProperty(ref _listaServicios, value); }
        }

        private Servicio _servicioSelected;
        public Servicio ServicioSelected
        {
            get { return _servicioSelected; }
            set { SetProperty(ref _servicioSelected, value); }
        }

        private string _codigo;
        public string Codigo
        {
            get { return _codigo; }
            set { SetProperty(ref _codigo, value); }
        }

        private string _lblCodigo;
        public string LblCodigo
        {
            get { return _lblCodigo; }
            set { SetProperty(ref _lblCodigo, value); }
        }

        private DelegateCommand _siguienteIC;
        public DelegateCommand SiguienteIC =>
            _siguienteIC ?? (_siguienteIC = new DelegateCommand(ExecuteSiguienteIC));

        async void ExecuteSiguienteIC()
        {
            try
            {
                string msj = ValidarCampos();
                if (msj != "")
                {
                    await DialogService.DisplayAlertAsync("Validación", msj, "OK");
                    return;
                }

                NavigationParameters parametros = GetNavigationParameters(Constantes.keyServicio);//indicamos que no considere servicio, porque recien sera añadido (es para el caso de ope frec)

                Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;
                    
                parametros.Add(Constantes.keyServicio, ServicioSelected);
                parametros.Add(Constantes.keyReciboIBS, ReciboIBS);
                parametros.Add(Constantes.keyCodigoServicio, Codigo);

                await NavigationService.NavigateAsync(Constantes.pagePagoServDatos, parametros);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public string ValidarCampos()
        {
            string msj = "";
            Cuenta ctaOrigen = RefNavParameters["CtaCargo"] as Cuenta;

            if (string.IsNullOrEmpty(Codigo))
            {
                msj = "Ingrese un número válido.";
            }
            else if (ServicioSelected == null)
            {
                msj = "Ingrese un servicio válido.";
            }
            else
            {
                Catalogo empresa = RefNavParameters[Constantes.keyEmpresa] as Catalogo;

                //dependiendo de la empresa coloca el label
                if (empresa.Codigo == "0" || empresa.Codigo == "2" || (empresa.Codigo == "1" && ServicioSelected.Codigo == "0")) //claro o entel
                {
                    msj = ValidarTelefonoCelular();
                }
                else if(empresa.Codigo == "1" && ServicioSelected.Codigo == "1")
                {
                    msj = ValidarTelefonoFijo();
                }
                else if (empresa.Codigo == "3" || empresa.Codigo == "4" )//3 y 4 son luz
                {
                    msj = ValidarLuz();
                }
                else if (empresa.Codigo == "5")//agua
                {
                    msj = ValidarAgua();
                }
            }

            return msj;
        }

        //valida si existen pagos pendientes
        public string ValidarTelefonoCelular()
        {
            string strLocalidad = "";
            string strNumero = "";
            string msjRpta = "";
            try
            {
                Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;
                strNumero = Codigo;

                //using (Label CtrlProveedor = (Label)Step2.ContentTemplateContainer.FindControl("lblProveedor"))
                //{
                //    using (DropDownList CtrlddlProveedor = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlProveedor"))
                //    {
                //        if (CtrlddlProveedor != null)
                //        {
                //CtrlProveedor.Text = CtrlddlProveedor.SelectedItem.Text.ToString();
                //BL.General objGeneral = new BL.General();
                //using (IDataReader iDr = objGeneral.ObtenerItemsPorIDPadre(int.Parse(CtrlddlProveedor.SelectedValue)))
                //{
                //    if (iDr != null)
                //    {
                //        while (iDr.Read())
                //        {
                CodigoServicioTemporal = "34345";//iDr.GetValue(iDr.GetOrdinal("intTablaID")).ToString();// iDr[0].ToString();
                NombreServicio = ServicioSelected.Nombre; //iDr.GetValue(iDr.GetOrdinal("strAliasDescriptivo")).ToString();

                            //        }
                            //    }
                            //}
                //        }

                //    }
                //}
                //using (Label CtrlServicio = (Label)Step2.ContentTemplateContainer.FindControl("lblServicio"))
                //{
                //    CtrlServicio.Text = NombreServicio;
                //}
                //using (HBC.ComboBoxText CtrlddlProveedor = (HBC.ComboBoxText)Step1.ContentTemplateContainer.FindControl("ComboBoxText1"))
                //{
                //    if (CtrlddlProveedor != null)
                //    {
                //        strNumero = CtrlddlProveedor.Text;
                //    }
                //}


                BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();
                CodigoServicioTemporal = (int.Parse(CodigoServicio) == 0) ? CodigoServicioTemporal : CodigoServicio;
                CodigoServicioTemporal = empresa.Codigo + ServicioSelected.Codigo;//aniadido temporalmente
                string strCodigoTrama = string.Empty;
                //using (DataSet CtrlInput = ObjPagoServicios.ObtenerControlesInput(int.Parse(CodigoServicioTemporal), 1))
                //{
                //    if (CtrlInput != null && CtrlInput.Tables[0].Rows.Count > 0)
                //    {
                //        strCodigoTrama = CtrlInput.Tables[0].Rows[0]["CtrlTrama"].ToString();
                if(empresa.Codigo == "0")
                {
            strCodigoTrama = "7053";
                }
                else if(empresa.Codigo == "1")
                {
            strCodigoTrama = "7026";
                }
                else
                {
            strCodigoTrama = "7053";
                }
                        switch (strCodigoTrama)
                        {
                            case "7026"://MOVISTAR
                                strLocalidad = "00 "; //(strLocalidad.ToString() == "00") ? "00 " : strLocalidad.ToString().PadRight(3, ' ');
                                strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' '); break;
                            case "7053": //CLARO
                                strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' ');
                                strLocalidad = "00 "; // strLocalidad.Trim().PadLeft(3, ' ');
                                break;
                            default:
                                strLocalidad = "00 "; //(strLocalidad.ToString() == "00") ? "00 " : strLocalidad.ToString().PadRight(3, ' ');
                                strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' ');
                                break;
                        }
                //    }
                //}
                using (DataSet dsData = ObjPagoServicios.ObtenerConsultaTelefono(115, CodigoServicioTemporal, out _strError, strLocalidad, strNumero))
                {
                    if (_strError == "0000")
                    {
                        ReciboIBS = ReciboService.ObtenerRecibosCelular(Codigo);
                        //if (dsData != null && dsData.Tables[0].Rows.Count > 0)
                        //{
                            //using (GridView gvDetalle = (GridView)Step2.ContentTemplateContainer.FindControl("gvPagos"))
                            //{
                            //    if (gvDetalle != null)
                            //    {
                            //        DataSet dstmp_data = new DataSet();
                            //        if (strCodigoTrama == BE.ListaTransacciones.ConsultaClaroCelular.ToString())
                            //        {
                            //            dstmp_data = EmularData7026(dsData, strNumero.Trim().TrimEnd());
                            //            gvDetalle.Columns[1].HeaderText = "Fecha de vencimiento";
                            //        }
                            //        else
                            //        {
                            //            gvDetalle.Columns[1].HeaderText = "Fecha de emisión";
                            //            dstmp_data = dsData;
                            //        }

                            //        DataColumn dcoview = new DataColumn("Oview", typeof(string));
                            //        dstmp_data.Tables[0].Columns.Add(dcoview);
                            //        int intIndice = -1;
                            //        foreach (DataRow dr in dstmp_data.Tables[0].Rows)
                            //        {
                            //            NombreCliente = dr["Odnombr"].ToString();
                            //            intIndice += 1;
                            //            if (intIndice == 0)
                            //                dr["Oview"] = "true";
                            //            else
                            //                dr["Oview"] = "false";
                            //        }
                            //        //using (Label CtrlNumpero = (Label)Step2.ContentTemplateContainer.FindControl("lblCliente")) CtrlNumpero.Text = NombreCliente + " - " + strNumero;
                            //        dstmp_data.Tables[0].AcceptChanges();
                            //        //ViewState["ViewDataPago"] = dsData;
                            //        gvDetalle.DataSource = dstmp_data;
                            //        gvDetalle.DataBind();
                            //    }
                            //}
                        //}
                    }
                    else
                    {
                        msjRpta = "El número de teléfono ingresado no posee deuda pendiente. Código IBS: " + _strError;
                        //OcultarInformacionStep2(Step2, false);
                        //MostrarMensaje((HBC.Mensaje)Step2.ContentTemplateContainer.FindControl("Mensaje3"), "Estimado Cliente.", _strError, HBC.ListImages.AdvertenciaCuidado);
                    }
                }
                //LlenarCuentasAsociadasHome();
            }
            catch (Exception ex)
            {
                msjRpta = "5";
                //GeneraEntrada(ex.Message, string.Concat("Exception ObtenerDatosStep2Home", HttpContext.Current.Session["strPad"].ToString().Trim()));
                //OcultarInformacionStep2(Step2, false);
                //MostrarMensaje((HBC.Mensaje)Step2.ContentTemplateContainer.FindControl("Mensaje3"), "Estimado Cliente", ex.Message, HBC.ListImages.AdvertenciaCuidado);

            }
            
            return msjRpta;
        }

        public string ValidarTelefonoFijo()
        {
            string msjRpta = "";
            //proveedor es empresa
            string strLocalidad = "";
            string strNumero = "";
            Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;
            try
            {
                //using (Label CtrlProveedor = (Label)Step2.ContentTemplateContainer.FindControl("lblProveedor"))
                //{
                //    using (DropDownList CtrlddlProveedor = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlProveedor"))
                string strProveedor = empresa.Nombre;// CtrlddlProveedor.SelectedItem.Text.ToString();
                //}
                //using (Label CtrlNumpero = (Label)Step2.ContentTemplateContainer.FindControl("lblCliente"))
                //{
                //    using (HBC.ComboBoxText CtrlddlProveedor = (HBC.ComboBoxText)Step1.ContentTemplateContainer.FindControl("ComboBoxText1"))
                //    {
                        //CtrlNumpero.Text = CtrlddlProveedor.Text;
                strNumero = Codigo ;// CtrlddlProveedor.Text;
                                    //    }
                                    //}
                                    //using (Label CtrlServicio = (Label)Step2.ContentTemplateContainer.FindControl("lblServicio"))
                                    //{
                                    //    using (DropDownList CtrlddlServicio = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlServicio"))
                                    //    {
                                    //        CtrlServicio.Text = CtrlddlServicio.SelectedItem.Text;
                                    //    }
                                    //}
                                    //using (DropDownList CtrlddlLocalidad = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlLocalidad"))
                                    //{
                strLocalidad = "01";// CtrlddlLocalidad.SelectedValue.ToString();
                //}
                BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();
                strLocalidad = strLocalidad.PadRight(3, ' ');
                string strTelefono = strNumero.PadRight(10, ' ');
                CodigoServicioTemporal = (int.Parse(CodigoServicio) == 0) ? CodigoServicioTemporal : CodigoServicio;
                CodigoServicioTemporal = empresa.Codigo + ServicioSelected.Codigo;//aniadido temporalmente
                using (DataSet dsData = ObjPagoServicios.ObtenerConsultaTelefono(152, CodigoServicioTemporal, out _strError, strLocalidad, strTelefono))
                {
                    if (_strError == "0000")
                    {
                        ReciboIBS = ReciboService.ObtenerRecibosTelfFijo(Codigo);
                        //if (dsData != null && dsData.Tables[0].Rows.Count > 0)
                        //{
                        //    using (GridView gvPagosTelefonia = (GridView)Step2.ContentTemplateContainer.FindControl("gvPendientes"))
                        //    {
                        //        gvPagosTelefonia.Columns[1].HeaderText = "Fecha de emisión";
                        //        DataSet dstmp_data = new DataSet();
                        //        dstmp_data = dsData;//DataGrid(); 
                        //        DataColumn dcoview = new DataColumn("Oview", typeof(string));
                        //        dstmp_data.Tables[0].Columns.Add(dcoview);
                        //        int intIndice = -1;
                        //        foreach (DataRow dr in dstmp_data.Tables[0].Rows)
                        //        {
                        //            NombreCliente = dr["Odnombr"].ToString();
                        //            intIndice += 1;
                        //            if (intIndice == 0)
                        //                dr["Oview"] = "true";
                        //            else
                        //                dr["Oview"] = "false";
                        //        }

                        //        dstmp_data.Tables[0].AcceptChanges();
                        //        ViewState["ViewDataPago"] = dstmp_data;
                        //        gvPagosTelefonia.DataSource = (DataSet)ViewState["ViewDataPago"];
                        //        gvPagosTelefonia.DataBind();

                        //    }
                        //    using (Label CtrlNumpero = (Label)Step2.ContentTemplateContainer.FindControl("lblCliente"))
                        //    {
                        //        CtrlNumpero.Text = NombreCliente + " - " + strNumero; //;CtrlddlProveedor.Text;
                        //    }
                        //}
                    }
                    else
                    {
                        msjRpta = "El número de teléfono ingresado no posee deuda pendiente. Código IBS: " + _strError;
                        //OcultarInformacionStep2(Step2, false);
                        //MostrarMensaje((HBC.Mensaje)Step2.ContentTemplateContainer.FindControl("Mensaje3"), "Estimado Cliente", _strError, HBC.ListImages.AdvertenciaPeligro);
                    }
                }
            }
            catch (Exception ex)
            {
                //OcultarInformacionStep2(Step2, false);
                //MostrarMensaje((HBC.Mensaje)Step2.ContentTemplateContainer.FindControl("Mensaje3"), "Estimado Cliente.", ex.Message, HBC.ListImages.AdvertenciaPeligro);
            }

            return msjRpta;
        }

        public string ValidarAgua()
        {
            string msjRpta = "";
            CodigoServicioTemporal = "34345";
            Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;
            //
            double douImporte = 0.0;
            string strCuenta = "";
            string strSuministro = "";
            string strNombreProveedor = "";
            string strFechaVencimiento = "";
            string strReferencia = "";
            string strImproteSoles = "";
            string strDescripcion = "";
            string strGastos = "".PadRight(14, '0');
            string strNombreCliente = "JUAN PEREZ";// Session["strNombreClienteIBS"].ToString().PadRight(31, ' ').Substring(0, 30);
            //using (DropDownList CtrlText = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlcuentas"))
                strCuenta = "".PadLeft(12, '0');//;CtrlText.SelectedValue.ToString().PadLeft(12, '0');
            //using (HBC.ComboBoxText CtrlText = (HBC.ComboBoxText)Step1.ContentTemplateContainer.FindControl("ComboBoxText1"))
                strSuministro = Codigo;// CtrlText.Text.PadLeft(8, ' ');
            //using (TextBox CtrlText = (TextBox)Step1.ContentTemplateContainer.FindControl("txtFechaVencimiento"))
                strFechaVencimiento = "210418";//CtrlText.Text.Replace("/", "");
                                               //using (TextBox CtrlText = (TextBox)Step1.ContentTemplateContainer.FindControl("txtReferencia"))
            strReferencia = "".PadLeft(15, '0');// CtrlText.Text.Trim().PadLeft(15, '0');
            //using (TextBox CtrlText = (TextBox)Step1.ContentTemplateContainer.FindControl("txtImporte"))
            //{
                strImproteSoles = Convert.ToString(System.Math.Round(double.Parse("0"), 2) * 100).PadLeft(14, '0'); //Convert.ToString(System.Math.Round(decimal.Parse(CtrlText.Text.ToString()), 2) * 100).PadLeft(14, '0'); //string.Format("{0:N}", double.Parse(CtrlText.Text.ToString())).Replace(".", "").Replace(",","").PadLeft(14, '0'); 
                douImporte = double.Parse("0");
            //};
            //using (TextBox CtrlText = (TextBox)Step1.ContentTemplateContainer.FindControl("txtDescripcion"))
            strDescripcion = "";// CtrlText.Text.PadRight(30, ' ');
            //if (!ValidarCuentaConSuficienteMonto("604", douImporte, strCuenta.PadLeft(12, '0')))
            //{
            //    MensajeAlerta("La cuenta de origen, no cuenta con saldo suficiente para concluir la operación");
            //    wzContenedor.MoveTo(Step1);
            //    return;
            //}
            BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();
            CodigoServicioTemporal = (int.Parse(CodigoServicio) == 0) ? CodigoServicioTemporal : CodigoServicio;
            CodigoServicioTemporal = empresa.Codigo + ServicioSelected.Codigo;//aniadido temporalmente
            if (CodigoServicioTemporal != "0")
            {
                using (DataSet dsData = ObjPagoServicios.ConsultarTransaccion(250, CodigoServicioTemporal, out _strError, strSuministro, strImproteSoles, strNombreCliente, strFechaVencimiento, strReferencia, strCuenta, strDescripcion, "1"))
                {
                    if (_strError == "0000")
                    {
                        if (dsData != null && dsData.Tables[0].Rows.Count > 0)
                        {
                            ReciboIBS = ReciboService.ObtenerRecibos(Codigo);
                            //Moneda a Cargar 
                            //string strPrefixMonedaCargado = string.Empty;
                            //string strCodigoInternoMonedaCargado = string.Empty;
                            //DataView dvMoneda = ListaMonedas.DefaultView;
                            //DataTable dtCuentas = (DataTable)ViewState["dtCuentas"];
                            //DataRow[] drCuentas = dtCuentas.Select("ODcodct='" + strCuenta.PadLeft(12, '0') + "'");
                            //dvMoneda.RowFilter = string.Empty;
                            //dvMoneda.RowFilter = "strCodigoInterno='" + drCuentas[0]["ODDmoned"].ToString() + "'";
                            //strPrefixMonedaCargado = dvMoneda[0]["strPrefijo"].ToString();
                            //strCodigoInternoMonedaCargado = drCuentas[0]["ODDmoned"].ToString();
                            //MonedaCuenta = drCuentas[0]["ODDmoned"].ToString();
                            //foreach (DataRow item in dsData.Tables[0].Rows)
                            //{
                            //    ITF = double.Parse(item["ODImpItf"].ToString());
                            //    Comision = double.Parse(item["ODComis"].ToString());
                            //    TotalPagar = double.Parse(item["OutGrs"].ToString());
                            //    Moneda = "604";
                            //    NombreProveedor = item["OutNme"].ToString();
                            //    ConceptoPago = item["OutDs1"].ToString();
                            //    NumeroRecibo = item["OutCuo"].ToString();
                            //    break;
                            //}

                            //using (Label CtrlTipoNumeroCuenta = (Label)Step2.ContentTemplateContainer.FindControl("lblTipoNumeroCuenta")) CtrlTipoNumeroCuenta.Text = ((DropDownList)Step1.ContentTemplateContainer.FindControl("ddlcuentas")).SelectedItem.Text.Split('(').GetValue(0).ToString();
                            //using (Label CtrlMontoPagar = (Label)Step2.ContentTemplateContainer.FindControl("lblMontoPagar")) CtrlMontoPagar.Text = strPrefixMonedaCargado + " " + string.Format("{0:N}", TotalPagar);
                            //using (Label CtrllblITF = (Label)Step2.ContentTemplateContainer.FindControl("lblITF")) CtrllblITF.Text = strPrefixMonedaCargado + " " + string.Format("{0:N}", ITF);
                            //using (Label CtrllblComisiones = (Label)Step2.ContentTemplateContainer.FindControl("lblComisiones")) CtrllblComisiones.Text = strPrefixMonedaCargado + " " + string.Format("{0:N}", Comision);
                            //using (Label CtrllblTotalDebitar = (Label)Step2.ContentTemplateContainer.FindControl("lblTotalDebitar")) CtrllblTotalDebitar.Text = strPrefixMonedaCargado + " " + string.Format("{0:N}", (Comision + ITF + TotalPagar));
                            //using (Label CtrllblProveedor = (Label)Step2.ContentTemplateContainer.FindControl("lblproveedor")) CtrllblProveedor.Text = NombreProveedor;
                            //using (Label Ctrllblservicio = (Label)Step2.ContentTemplateContainer.FindControl("lblservicio")) Ctrllblservicio.Text = ConceptoPago;
                            //using (Label CtrllblclienteNumero = (Label)Step2.ContentTemplateContainer.FindControl("lblclienteNumero")) CtrllblclienteNumero.Text = strNombreCliente + " - " + strSuministro;
                            //using (Label CtrllblRecibo = (Label)Step2.ContentTemplateContainer.FindControl("lblRecibo")) CtrllblRecibo.Text = NumeroRecibo;//((Label)Step2.ContentTemplateContainer.FindControl("txtDescripcion")).Text;
                            //using (Label CtrllblMontoPagarTelefonia = (Label)Step2.ContentTemplateContainer.FindControl("lblMontoPagarSedapal")) CtrllblMontoPagarTelefonia.Text = ObtenerImporte(Moneda, TotalPagar.ToString());
                            //using (Label CtrllblTotalPagarTelefonia = (Label)Step2.ContentTemplateContainer.FindControl("lblTotalPagarSedapal")) CtrllblTotalPagarTelefonia.Text = ObtenerImporte(Moneda, TotalPagar.ToString());
                            //using (Label CtrllblDescripicion = (Label)Step2.ContentTemplateContainer.FindControl("lblDescripcion")) CtrllblDescripicion.Text = ((TextBox)Step1.ContentTemplateContainer.FindControl("txtDescripcion")).Text;

                        }
                    }
                    else
                    {
                        msjRpta = "El número de suministro ingresado no posee deuda pendiente. Código IBS: " + _strError;
                        //OcultarInformacionStep2(Step2, false);
                        //MostrarMensaje((HBC.Mensaje)Step2.ContentTemplateContainer.FindControl("Mensaje3"), "Estimado Cliente", _strError, HBC.ListImages.Error);
                    }
                }
            }
            else
            {
                //OcultarInformacionStep2(Step2, false);
                //MostrarMensaje((HBC.Mensaje)Step2.ContentTemplateContainer.FindControl("Mensaje3"), "Estimado Cliente", "no ha seleccionado un servicio", HBC.ListImages.Error);
            }

            return msjRpta;
        }

        public string ValidarLuz()
        {
            string msjRpta = "";
            CodigoServicioTemporal = "34345";
            Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;
            //
            //BL.General objGeneral = new BL.General();
            //DropDownList ddlProveedor = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlProveedor");
            //DropDownList ddlServicio = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddServicios1");
            //DropDownList ddlSector = (DropDownList)Step1.ContentTemplateContainer.FindControl("ddlSector");
            ////TextBox txtCodigo = (TextBox)Step1.ContentTemplateContainer.FindControl("txtCodigo");
            //HBC.ComboBoxText txtCodigo = (HBC.ComboBoxText)Step1.ContentTemplateContainer.FindControl("ComboBoxText1");
            //Label lblImporte = ((Label)Step1.ContentTemplateContainer.FindControl("lblImporte"));
            //Label lblProveedor = ((Label)Step2.ContentTemplateContainer.FindControl("lblProveedor"));
            //Label lblServicio = ((Label)Step2.ContentTemplateContainer.FindControl("lblServicio"));
            //Label lblCodigo = ((Label)Step2.ContentTemplateContainer.FindControl("lblCodigo"));
            //DropDownList ddlCuenta = (DropDownList)Step2.ContentTemplateContainer.FindControl("ddlCuentas");
            //GridView gvPendientes = ((GridView)Step2.ContentTemplateContainer.FindControl("gvPendientes"));
            string strMensaje = "00 ".PadRight(3, ' ') + Codigo.PadLeft(7, '0').PadLeft(7, '0').PadRight(10, ' ');
            BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();
            CodigoServicioTemporal = (int.Parse(CodigoServicio) == 0) ? CodigoServicioTemporal : CodigoServicio;
            CodigoServicioTemporal = empresa.Codigo + ServicioSelected.Codigo;//aniadido temporalmente
            using (DataSet dsData = ObjPagoServicios.ConsultarTransaccion(115, CodigoServicioTemporal, out _strError, strMensaje))
            {
                if (_strError == "0000")
                {
                    ReciboIBS = ReciboService.ObtenerRecibos(Codigo);
                    //DataSet dstmp_data = new DataSet();
                    //dstmp_data = dsData;//DataGrid(); 
                    //DataColumn dcoview = new DataColumn("Oview", typeof(string));
                    //dstmp_data.Tables[0].Columns.Add(dcoview);
                    //int intIndice = -1;
                    //string strNombreCliente = string.Empty;
                    //foreach (DataRow dr in dstmp_data.Tables[0].Rows)
                    //{
                    //    //strNombreCliente = dr["Odnombr"].ToString();
                    //    intIndice += 1;
                    //    if (intIndice == 0)
                    //        dr["Oview"] = "true";
                    //    else
                    //        dr["Oview"] = "false";
                    //}
                    //dstmp_data.Tables[0].AcceptChanges();
                    //((Button)Step2.CustomNavigationTemplateContainer.FindControl("btnStep2Siguiente")).Enabled = true;
                    //Step2.ContentTemplateContainer.FindControl("TablaStep2Datos1").Visible = true;
                    //if (Int32.Parse(dstmp_data.Tables["OData"].Rows[0]["ODcantd"].ToString()) > 0)
                    //    ViewState["txtSeccion"] = dstmp_data.Tables["OData"].Rows[0]["ODcsecc"].ToString().PadLeft(2, '0');
                    //lblProveedor.Text = ddlProveedor.SelectedItem.Text;
                    //lblServicio.Text = ddlServicio.SelectedItem.Text;
                    //lblCodigo.Text = txtCodigo.Text;
                    //ViewState["ODrcbos"] = dstmp_data.Tables["ODrcbos"];
                    //ViewState["ViewDataPago"] = dsData;
                    //gvPendientes.DataSource = dstmp_data.Tables["ODrcbos"].DefaultView;
                    //gvPendientes.DataBind();
                }
                else
                {
                    msjRpta = "El número ingresado no posee deudas pendientes. Codigo IBS: " + _strError;
                    //OcultarInformacionStep2(Step2, false);
                    //try
                    //{
                    //    MostrarMensaje((HBC.Mensaje)Step2.ContentTemplateContainer.FindControl("Mensaje3"), "Estimado Cliente", _strError, HBC.ListImages.AdvertenciaPeligro);
                    //}
                    //catch (Exception ex)
                    //{
                    //    MostrarMensaje((HBC.Mensaje)Step2.ContentTemplateContainer.FindControl("Mensaje3"), "Estimado Cliente", ex.Message, HBC.ListImages.AdvertenciaPeligro);
                    //}
                }

            }
            //
            return msjRpta;
        }

            private DataSet EmularData7026(DataSet dsData, string strNumeroTelefono)
        {
            BL.PagoServicios ObjPagoServicios = new BL.PagoServicios();

            //using (DataSet dsGenerada = null)// ObjPagoServicios.EmularData("OutDat", BE.ListaTransacciones.ConsultaRecargaVirtualMovistar.ToString(), 0, 0))
            //{
            //    foreach (DataColumn item in dsGenerada.Tables[0].Columns)
            //        item.DataType = System.Type.GetType("System.String");
            //    foreach (DataRow item in dsData.Tables[0].Rows)
            //    {
            //        dsGenerada.Tables[0].Rows.Add();
            //        dsGenerada.Tables[0].Rows[dsGenerada.Tables[0].Rows.Count - 1]["Odrecib"] = item["CNroDoc"];
            //        dsGenerada.Tables[0].Rows[dsGenerada.Tables[0].Rows.Count - 1]["Odmoned"] = item["CMonPro"];
            //        foreach (DataRow item2 in dsData.Tables[1].Rows)
            //        {
            //            if (item2["OCAbon"].ToString().TrimEnd().Trim() == strNumeroTelefono)
            //                dsGenerada.Tables[0].Rows[dsGenerada.Tables[0].Rows.Count - 1]["Odnombr"] = dsData.Tables[1].Rows[0]["OCNombre"];
            //        }
            //        dsGenerada.Tables[0].Rows[dsGenerada.Tables[0].Rows.Count - 1]["Odfecem"] = item["CFecVen"];
            //        dsGenerada.Tables[0].Rows[dsGenerada.Tables[0].Rows.Count - 1]["OdTotal"] = item["CTotPag"];
            //    }
            //    return dsGenerada;
            //}
            return null;

        }

    }
}
