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
        private string _strError = "";
        private string CodigoServicioTemporal;
        private string NombreServicio;
        private string CodigoServicio;

        public ServicioEmpresaViewModel(ICatalogoService catalogoService, INavigationService navigationService, IUserDialogs userDialogs)
            : base(navigationService, userDialogs)
        {
            CatalogoService = catalogoService;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;
            string pageOrigen = RefNavParameters[Constantes.pageOrigen] as string;
            Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;

            NomEmpresa = empresa.Nombre;

            ListaServicios = CatalogoService.ListarServiciosxEmpresa(empresa.Codigo);//numero de suministro

            //dependiendo de la empresa coloca el label
            if(empresa.Codigo == "0" || empresa.Codigo == "1" || empresa.Codigo == "2")
            {
                LblCodigo = "Número de Teléfono";
            }else if(empresa.Codigo == "3" || empresa.Codigo == "4" || empresa.Codigo == "5")
            {
                LblCodigo = "Número de Suministro";
            }

            if (pageOrigen == Constantes.pageOperaciones)
            {
                Servicio servicio = RefNavParameters["Servicio"] as Servicio;
                ServicioSelected = ListaServicios.Where(p => p.Codigo == servicio.Codigo).First();
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

                NavigationParameters parametros = GetNavigationParameters("Servicio");//indicamos que no considere servicio, porque recien sera añadido

                Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;

                //filtro telefonia celular
                if (empresa.Codigo == "0" || (empresa.Codigo == "1" && ServicioSelected.Codigo == "0") || empresa.Codigo == "2")
                {
                    string msj2 = ValidarTelefonoCelular();

                    if(msj2 != "")
                    {
                        await DialogService.DisplayAlertAsync("Validación", msj2, "OK");
                        return;
                    }
                }
                    
                parametros.Add("Servicio", ServicioSelected);
                parametros.Add("stringEmpresa", NomEmpresa);
                parametros.Add("stringPicServicio", ServicioSelected.Nombre);
                parametros.Add("stringCodigo", Codigo);

                await NavigationService.NavigateAsync("PagoServicioEmpresa", parametros);

                ServicioSelected = null;
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

            return msj;
        }

        //valida si existen pagos pendientes
        public string ValidarTelefonoCelular()
        {
            string strLocalidad = "";
            string strNumero = "";
            string msjRpta = "";/*
            try
            {
                Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;

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
                string strCodigoTrama = string.Empty;
                using (DataSet CtrlInput = ObjPagoServicios.ObtenerControlesInput(int.Parse(CodigoServicioTemporal), 1))
                {
                    if (CtrlInput != null && CtrlInput.Tables[0].Rows.Count > 0)
                    {
                        strCodigoTrama = CtrlInput.Tables[0].Rows[0]["CtrlTrama"].ToString();
                        switch (strCodigoTrama)
                        {
                            case "7026":
                                strLocalidad = "   "; //(strLocalidad.ToString() == "00") ? "00 " : strLocalidad.ToString().PadRight(3, ' ');
                                strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' '); break;
                            case "7053":
                                strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' ');
                                strLocalidad = "   "; // strLocalidad.Trim().PadLeft(3, ' ');
                                break;
                            default:
                                strLocalidad = "   "; //(strLocalidad.ToString() == "00") ? "00 " : strLocalidad.ToString().PadRight(3, ' ');
                                strNumero = strNumero.ToString().Trim().TrimEnd().PadRight(10, ' ');
                                break;
                        }
                    }
                }
                using (DataSet dsData = ObjPagoServicios.ObtenerConsultaTelefono(115, int.Parse(CodigoServicioTemporal), out _strError, strLocalidad, strNumero))
                {
                    if (_strError == "0000")
                    {
                        if (dsData != null && dsData.Tables[0].Rows.Count > 0)
                        {
                            using (GridView gvDetalle = (GridView)Step2.ContentTemplateContainer.FindControl("gvPagos"))
                            {
                                if (gvDetalle != null)
                                {
                                    DataSet dstmp_data = new DataSet();
                                    if (strCodigoTrama == BE.DefaultValues.ConsultaClaroCelular.ToString())
                                    {
                                        dstmp_data = EmularData7026(dsData, strNumero.Trim().TrimEnd());
                                        gvDetalle.Columns[1].HeaderText = "Fecha de vencimiento";
                                    }
                                    else
                                    {
                                        gvDetalle.Columns[1].HeaderText = "Fecha de emisión";
                                        dstmp_data = dsData;
                                    }

                                    DataColumn dcoview = new DataColumn("Oview", typeof(string));
                                    dstmp_data.Tables[0].Columns.Add(dcoview);
                                    int intIndice = -1;
                                    foreach (DataRow dr in dstmp_data.Tables[0].Rows)
                                    {
                                        NombreCliente = dr["Odnombr"].ToString();
                                        intIndice += 1;
                                        if (intIndice == 0)
                                            dr["Oview"] = "true";
                                        else
                                            dr["Oview"] = "false";
                                    }
                                    using (Label CtrlNumpero = (Label)Step2.ContentTemplateContainer.FindControl("lblCliente")) CtrlNumpero.Text = NombreCliente + " - " + strNumero;
                                    dstmp_data.Tables[0].AcceptChanges();
                                    ViewState["ViewDataPago"] = dsData;
                                    gvDetalle.DataSource = dstmp_data;
                                    gvDetalle.DataBind();
                                }
                            }
                        }
                    }
                    else
                    {
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
            */
            return msjRpta;
        }

    }
}
