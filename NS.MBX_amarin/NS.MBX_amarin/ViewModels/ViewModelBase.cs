using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NS.MBX_amarin.Six;
using NS.MBX_amarin.Common;
using System.Collections.Specialized;
using System.Data;
using Xamarin.Forms;
using NS.MBX_amarin.BusinessLogic;

namespace NS.MBX_amarin.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService DialogService { get; private set; }
        protected IUserDialogs UserDialogs { get; private set; }
        protected NavigationParameters RefNavParameters { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            DialogService = pageDialogService;
        }

        public ViewModelBase(INavigationService navigationService, IUserDialogs userDialogs)
        {
            NavigationService = navigationService;
            UserDialogs = userDialogs;
        }

        public ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService, IUserDialogs userDialogs)
        {
            NavigationService = navigationService;
            DialogService = pageDialogService;
            UserDialogs = userDialogs;
        }

        //cuando se sale de esta pagina
        public virtual void OnNavigatedFrom(NavigationParameters parametros)
        {
            
        }

        //cuando se navega hacia aqui incluyendo backbutton
        public virtual void OnNavigatedTo(NavigationParameters parametros)
        {
            
        }

        //cuando se navega hacia aqui de ida, no de regreso con el back button
        public virtual void OnNavigatingTo(NavigationParameters parametros)
        {
            
        }

        public virtual void Destroy()
        {
            
        }

        //metodo que permite crear un nuevo navigation parameters sin tener que apuntar al mismo anterior
        //por defecto no incluye la pagina de origen, a menos que sea una pagina muy frecuentada
        protected NavigationParameters GetNavigationParameters()
        {
            return GetNavigationParameters(false, null);
        }

        protected NavigationParameters GetNavigationParameters(string listaParametrosNoIncluir)
        {
            return GetNavigationParameters(false, listaParametrosNoIncluir);
        }

        //si la lista tiene parametros, estos no se colocaran en el nuevo navigation parameters
        protected NavigationParameters GetNavigationParameters(bool incluirPageOrigen, string parametrosNoIncluir)
        {
            NavigationParameters navParameters = new NavigationParameters();

            if (RefNavParameters != null)
            {
                List<string> listaParametrosNoIncluye = null;
                if(parametrosNoIncluir != null)
                {
                    listaParametrosNoIncluye = parametrosNoIncluir.Split(',').ToList();
                }

                foreach (KeyValuePair<string, object> navigationParameter in RefNavParameters)
                {
                    //la pagina origen debe ser colocada en cada pagina individual
                    if (incluirPageOrigen == true || (incluirPageOrigen == false && navigationParameter.Key != Constantes.pageOrigen))
                    {
                        bool encontroEnLista = false;

                        if (listaParametrosNoIncluye != null)
                        {
                            foreach(string item in listaParametrosNoIncluye)
                            {
                                if(item == navigationParameter.Key)
                                {
                                    encontroEnLista = true;
                                    break;
                                }
                            }
                        }

                        if(!encontroEnLista)
                        {
                            navParameters.Add(navigationParameter.Key, navigationParameter.Value);
                        }
                    }
                }
            }

            return navParameters;
        }

        public int ValidarTarjetaCoordenadas(string strNumeroCoordenada, int intNumeroCaracteresObligatorios, string strClave, out string strError)
        {
            string strNumeroTarjeta = string.Empty;
            //BFP.HomeBanking.BusinessLogic.SIXP2.Service objSixtoken = null;
            TransaccionesMBX Transaccion = null;
            try
            {
                //if (Session["strTC"] != null)
                //{
                //objSixtoken = new BFP.HomeBanking.BusinessLogic.SIXP2.Service { Url = System.Configuration.ConfigurationManager.AppSettings.Get("URLHOMEBANKINGWS") };
                Transaccion = new TransaccionesMBX();
                strNumeroTarjeta = "23232323232323";// Session["strTC"].ToString().Substring(0, 14);
                string strTarjetaCoordenada = strNumeroTarjeta.ToString() + strNumeroCoordenada.ToString();
                string strClaveDigital = strClave.ToString();
                if (strClaveDigital.ToString() == "")
                {
                    strError = "Debe Ingresar su Clave Digital";
                    return 1;
                }
                if (strClaveDigital.ToString().Length < int.Parse(intNumeroCaracteresObligatorios.ToString()))
                {
                    strError = "La clave Digital debe ser de " + intNumeroCaracteresObligatorios.ToString();
                    return 1;
                }
                string strToken = new SixService().SetToken(strTarjetaCoordenada, strClaveDigital);
                strToken = strToken.Substring(21, 16);
                string strMensajeTrama = strTarjetaCoordenada.ToString() + "   " + strToken.ToString() + "    ";
                strError = Transaccion.EjecutarTransaccion(ListaTransacciones.VALIDAR_COORDENADAS_HB(), 121, strMensajeTrama.ToString());
                if (strError == "0000")
                {
                    //Session["ds_TramaCoordenadas"] = null;//soluciona problema TCO repetido en cada pago en CHROME 26-DIC-2013 FDJ
                    return 0;
                }
                else
                {
                    strError = "4008";
                    return 1;
                }
                //}
                //else
                //{
                //    strError = "4002";
                //    return -1;
                //}

            }
            catch (Exception ex)
            {
                //GenerarLogError(string.Concat(ex.Message, ex.HelpLink, ex.StackTrace), " Exception ValidarTarjetaCoordenadas4");
                strError = "6666";
                return -1;
            }
        }

        protected StringDictionary ObtenerTipoCambio()
        {
            TransaccionesMBX tx = new TransaccionesMBX();
            StringDictionary strTipoCambio = new StringDictionary();
            //DataSet dsSalida = null;
            string _strError = string.Empty;

            try
            {
                if (Application.Current.Properties["TipoCambio"] == null)
                {
                    using (DataSet dsSalida = tx.EjecutarTransaccion(ListaTransacciones.TIPO_CAMBIO(), ListaTransacciones.LongitudCabecera(), "", ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError))
                    {
                        if (string.Compare(_strError, "0000") == 0)
                        {
                            strTipoCambio.Add("compra", decimal.Parse(dsSalida.Tables[0].Rows[0]["ODtipc1"].ToString()).ToString());
                            strTipoCambio.Add("venta", decimal.Parse(dsSalida.Tables[0].Rows[0]["ODtipc2"].ToString()).ToString());
                            Application.Current.Properties["TipoCambio"] = strTipoCambio;
                        }
                        else
                        {
                            //Session["TipoCambio"] = null;
                            Application.Current.Properties["TipoCambio"] = null;
                            strTipoCambio = null;
                        }
                    }

                }
                else
                {
                    strTipoCambio = (StringDictionary)Application.Current.Properties["TipoCambio"];
                }
            }
            catch (Exception ex)
            {
                //GenerarLogError(string.Concat(ex.Message, ex.HelpLink, ex.StackTrace), " Exception ObtenerTipoCambio");
                return strTipoCambio;
            }
            finally
            {
                //dsSalida = null;
            }


            return strTipoCambio;

        }

        protected StringDictionary ObtenerTipoCambioPreferencial(string strCuentaDestino, string strCuentaOrigen, string strMonedaTransaccion, double douImporteTransaccion)
        {
            TransaccionesMBX tx = new TransaccionesMBX();
            StringDictionary strTipoCambio = new StringDictionary();
            DataSet dsSalida = null;
            string _strError = string.Empty;
            try
            {
                string strImporte = Convert.ToString(System.Math.Round(douImporteTransaccion, 2) * 100).PadLeft(14, '0');
                string strTrama = string.Concat(strCuentaDestino.PadRight(20, ' '), strCuentaOrigen.PadRight(20, ' '), strImporte, strMonedaTransaccion.PadLeft(3, '0'));
                dsSalida = tx.EjecutarTransaccion(ListaTransacciones.TipoCambioPreferencia.ToString(), 150, strTrama, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
                if (string.Compare(_strError, "0000") == 0)
                {
                    strTipoCambio.Add("compra", dsSalida.Tables[0].Rows[0]["ODRatFpr"].ToString());
                    strTipoCambio.Add("venta", dsSalida.Tables[0].Rows[0]["ODRatFsr"].ToString());
                    Application.Current.Properties["TipoCambioPreferencia"] = strTipoCambio;
                }
                else
                {
                    strTipoCambio.Add("compra", "1.0000");
                    strTipoCambio.Add("venta", "1.0000");
                    Application.Current.Properties["TipoCambioPreferencia"] = strTipoCambio;
                }
            }
            catch (Exception ex)
            {
                //GenerarLogError(string.Concat(ex.Message, ex.HelpLink, ex.StackTrace), " Exception ObtenerTipoCambioPreferencial");
                strTipoCambio.Add("compra", "1.0000");
                strTipoCambio.Add("venta", "1.0000");
                return strTipoCambio;
            }
            finally
            {
                dsSalida = null;
                tx = null;
            }
            return strTipoCambio;

        }
    }
}
