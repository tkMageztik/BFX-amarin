using NS.MBX_amarin.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace NS.MBX_amarin.Six
{
    public class SixService : ISixService
    {
       // SixSendClass SIXConnection = new SixSendClass();

        //[System.Runtime.InteropServices.DllImport("C:\\sixclsec\\Sixpbl32.dll")]
        private static extern int SixGenPBlk(string Pin, string Pad, [In, Out]StringBuilder Token);

        private string SetSender()
        {
            string mvarWDirectory = "C:\\HBANKING";// ConfigurationManager.AppSettings.Get("mvarWDirectory"); //"C:\\HBANKING";
            string mvarLNivel = "D"; //ConfigurationManager.AppSettings.Get("mvarLNivel"); //"D";

            try
            {
                ASCIIEncoding AE = new ASCIIEncoding();
                byte[] Level;
                Level = AE.GetBytes(mvarLNivel);
                //SIXConnection.LNivel = mvarLNivel;
                //SIXConnection.WDirectory = mvarWDirectory;
                //return SIXConnection.SetSender().ToString();
                return "";
            }
            catch (Exception ex)
            {
                //LogManager.Log(ex, BFP.Library.Utils.Enums.LogType.Xml);
                throw ex;
            }
        }

        //[WebMethod]
        public string SetToken(string PAD, string PIN)
        {
            int iStatus;
            StringBuilder PinBlock = new StringBuilder(16);
            string _Retorno;
            try
            {
                if ((PAD.Length != 16) && (PIN.Length != 4)) throw new Exception("{ Error en el Ingreso de Parametros }");
                iStatus = SixGenPBlk(PIN, PAD, PinBlock);
                if (iStatus != 0) throw new Exception("{ Error en el Ingreso de Parametros }");
                _Retorno = PAD + "     " + PinBlock + String.Format("{0:ddMMyyyy}", DateTime.Now) + System.DateTime.Now.Hour.ToString().PadLeft(2, '0') + System.DateTime.Now.Minute.ToString().PadLeft(2, '0') + System.DateTime.Now.Second.ToString().PadLeft(2, '0');
                return _Retorno;
            }
            catch (Exception ex)
            {
                //LogManager.Log(ex, BFP.Library.Utils.Enums.LogType.Xml);
                throw ex;
            }
        }

        //[WebMethod]
        public string SendMessage(string Trx, short Length, string Message)
        {
            try
            {
                string SetSend = string.Empty;


                string _CodigoPlaza = "320";// ConfigurationManager.AppSettings.Get("_CodigoPlaza"); //"320";
                string _NroLogHost = "00000";//ConfigurationManager.AppSettings.Get("_NroLogHost"); //"00000";
                string _CodigoUsuario = "022"; //ConfigurationManager.AppSettings.Get("_CodigoUsuario"); //"022";
                string _CodigoSucursalUsuario = "22"; //ConfigurationManager.AppSettings.Get("_CodigoSucursalUsuario"); //"22";
                string _CodigoUnicoOperador = "622";// ConfigurationManager.AppSettings.Get("_CodigoUnicoOperador"); //"622";
                                              //string _CodigoRecibidorPagador = ConfigurationManager.AppSettings.Get("_CodigoRecibidorPagador"); //"WEBSERVER ";
                string _CodigoEstacion = "WEBSERVER ";// ConfigurationManager.AppSettings.Get("_CodigoEstacion"); //"WEBSERVER ";
                string _CodigoSIAFUsuario = "WEBSERVER "; //ConfigurationManager.AppSettings.Get("_CodigoSIAFUsuario"); //"WEBSERVER ";

                string FechaSiaf = string.Empty; //"250706";

                //if (ConfigurationManager.AppSettings.Get("blnFechaFija") == "1")
                //    FechaSiaf = ConfigurationManager.AppSettings.Get("strFechaFija"); //"250706";
                //else
                    FechaSiaf = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString().Substring(2, 2);

                StringBuilder _HeadMessage = new StringBuilder(4097);

                string OutMessage = string.Empty;
                string HResult = string.Empty;

                SetSend = SetSender();

                //SIXConnection.Host = "WEBUSR01"; //ConfigurationManager.AppSettings.Get("HOST"); //"WEBUSR01";
                //SIXConnection.User = "USER0001"; //ConfigurationManager.AppSettings.Get("USER"); //"USER0001";
                //SIXConnection.Password = "USER0001"; //ConfigurationManager.AppSettings.Get("PASSWORD"); //"USER0001";
                //SIXConnection.Server = "WDESA000"; //ConfigurationManager.AppSettings.Get("SERVER"); //"WDESA000";
                //SIXConnection.Application = "WDESA000"; //ConfigurationManager.AppSettings.Get("APPLICATION"); //"WDESA000";
                //SIXConnection.HostD = "scosysv"; //ConfigurationManager.AppSettings.Get("HOSTD"); //"scosysv";
                //SIXConnection.Port = "portscosixdesa";// ConfigurationManager.AppSettings.Get("PORT"); //"portscosixdesa";

                _HeadMessage.Append(_CodigoPlaza + _NroLogHost + _CodigoUsuario + _CodigoSucursalUsuario +
                                    _CodigoUnicoOperador + _CodigoEstacion + Trx +
                                    System.DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                                    System.DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                                    System.DateTime.Now.Second.ToString().PadLeft(2, '0') +
                                    _CodigoSIAFUsuario + "          " + "00000" + " " + "0" + "2" + "0" + "0" + "0" + "A" +
                                    FechaSiaf + "        " + Message);

                //SIXConnection.MMessage = 4096;
                //SIXConnection.LMessage = Length;
                ////SIXConnection.

                //SIXConnection.Message = _HeadMessage.ToString();

                //enviar mensaje
                //OutMessage = SIXConnection.SendMessage(_HeadMessage.ToString());

                //try
                //{
                //HResult = "";// SIXConnection.MessageOut;//string, osea encriptado
                if (Trx == ListaTransacciones.TrasferenciaValidaCuentas)
                {
                    //ERROR
                    //HResult = "**00000HB        203600000192412 * **                                                                                                                                                                                                           *";
                    //ok
                    HResult = "**00000HB        000000000192412 * *JUAN PEREZ OCHOA              S                                                                                                                                                                       01  02 ";

                }
                else if (Trx == ListaTransacciones.TrasferenciaConsultaGastos)
                {
                    //ERROR
                    // HResult = "**00000HB        203600000192412 * **                                                                                                                                                                                                           *";

                    //OK
                    HResult = "**00000HB        000000000192412 * *                                                                                                                                                                                                                                                                                                                                                                                          ";

                }
                else if (Trx == ListaTransacciones.TrasferenciaEjecuta)
                {
                    //ERROR
                    // HResult = "**08785HB        000000000152743*** GUEVARA OTERO JUAN JÞGUEVARA OTERO JUAN JþGUEVARA OTERO JUAN JOSE       µ0000000000000000000400 0000002830 2100100000000001132Ÿ¸00000000000000000000000000prueba1XXXXXXXXXXXXX          210Cuenta de Ahorros             Cuenta de Ahorros             000000000000400";

                    //OK
                    HResult = "**08785HB        000000000152743*** GUEVARA OTERO JUAN J%GUEVARA OTERO JUAN J%GUEVARA OTERO JUAN JOSE       %0000000000000000000400 0000002830 2100100000000001132%%00000000000000000000000000prueba1XXXXXXXXXXXXX          210Cuenta de Ahorros             Cuenta de Ahorros             000000000000400                                                                                                                    ";

                }
                else if (Trx == ListaTransacciones.VALIDAR_COORDENADAS_HB())
                {
                    //ERROR
                    // HResult = "**08785HB        000000000152743*** GUEVARA OTERO JUAN JÞGUEVARA OTERO JUAN JþGUEVARA OTERO JUAN JOSE       µ0000000000000000000400 0000002830 2100100000000001132Ÿ¸00000000000000000000000000prueba1XXXXXXXXXXXXX          210Cuenta de Ahorros             Cuenta de Ahorros             000000000000400";

                    //OK
                    HResult = "**08785HB        000000000152743***                                                                                                                ";

                }
                else if (Trx == ListaTransacciones.TIPO_CAMBIO())//faltatrama
                {
                    //ERROR
                    // HResult = "**08785HB        000000000152743*** GUEVARA OTERO JUAN JÞGUEVARA OTERO JUAN JþGUEVARA OTERO JUAN JOSE       µ0000000000000000000400 0000002830 2100100000000001132Ÿ¸00000000000000000000000000prueba1XXXXXXXXXXXXX          210Cuenta de Ahorros             Cuenta de Ahorros             000000000000400";

                    //OK
                    HResult = "**08785HB        000000000152743***                                                                                                                ";

                }
                else if (Trx == ListaTransacciones.TipoCambioPreferencia)//faltatrama
                {
                    //ERROR
                    // HResult = "**08785HB        000000000152743*** GUEVARA OTERO JUAN JÞGUEVARA OTERO JUAN JþGUEVARA OTERO JUAN JOSE       µ0000000000000000000400 0000002830 2100100000000001132Ÿ¸00000000000000000000000000prueba1XXXXXXXXXXXXX          210Cuenta de Ahorros             Cuenta de Ahorros             000000000000400";

                    //OK
                    HResult = "**08785HB        000000000152743***                                                                                                                ";

                }
                else if (Trx == ListaTransacciones.TrasferenciaOtroBancoConsultaGastos)//faltatrama
                {
                    //ERROR
                    // HResult = "**08785HB        000000000152743*** GUEVARA OTERO JUAN JÞGUEVARA OTERO JUAN JþGUEVARA OTERO JUAN JOSE       µ0000000000000000000400 0000002830 2100100000000001132Ÿ¸00000000000000000000000000prueba1XXXXXXXXXXXXX          210Cuenta de Ahorros             Cuenta de Ahorros             000000000000400";

                    //OK
                    HResult = "**08785HB        000000000152743*** GUEVARA OTERO JUAN J%GUEVARA OTERO JUAN J%GUEVARA OTERO JUAN JOSE       %0000000000000000000400 0000002830 2100100000000001132%%00000000000000000000000000prueba1XXXXXXXXXXXXX          210Cuenta de Ahorros             Cuenta de Ahorros             000000000000400                                                                                                                                                                                                                                                                                                                                                                             ";

                }
                else if (Trx == ListaTransacciones.TrasferenciaEjecutaOtroBanco)//faltatrama
                {
                    //ERROR
                    // HResult = "**08785HB        000000000152743*** GUEVARA OTERO JUAN JÞGUEVARA OTERO JUAN JþGUEVARA OTERO JUAN JOSE       µ0000000000000000000400 0000002830 2100100000000001132Ÿ¸00000000000000000000000000prueba1XXXXXXXXXXXXX          210Cuenta de Ahorros             Cuenta de Ahorros             000000000000400";

                    //OK
                    HResult = "**08785HB        000000000152743*** GUEVARA OTERO JUAN J%GUEVARA OTERO JUAN J%GUEVARA OTERO JUAN JOSE       %0000000000000000000400 0000002830 2100100000000001132%%00000000000000000000000000prueba1XXXXXXXXXXXXX          210Cuenta de Ahorros             Cuenta de Ahorros             000000000000400                                                                                                                                                                                                                                                                                                                                                                             ";

                }
                
                else if (Trx == ListaTransacciones.PagoTarjetaBancoFinancieroConsulta)
                {
                    HResult = "**00000HB        000000000191520*** NESTOR  ELIAS                 MAXIMA MC DORADA INT00000000 0000000000000 0000000211879 0000000242400 0000000262400 0000000252400 0000000272400 0000000009260 0000000282400 0000000004815* ";
                }
                else if (Trx == ListaTransacciones.PagoTarjetaBancoFinanciero)
                {
                    HResult = "**08785HB        000000000152743***                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ";
                }                
                else if (Trx == ListaTransacciones.PagoTarjetaBancoFinancieroEjecuta)
                {
                    HResult = "**08785HB        000000000152743***                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ";
                }
                else if (Trx == ListaTransacciones.PagoTarjetaOtroBanco)
                {
                    HResult = "**08785HB        000000000152743***                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ";
                }
                else if (Trx == ListaTransacciones.PagoTarjetaOtroBancoEjecuta)
                {
                    HResult = "**08785HB        000000000152743***                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ";
                }
                //servicios
                else if (Trx == "7026")//consulta: celular y fijo movistar, edelnor y luz del sur
                {
                    HResult = "**08785HB        000000000152743***                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      ";
                }
                else if (Trx == ListaTransacciones.ConsultaClaroCelular)//consulta: claro y entel
                {
                    HResult = "**08785HB        000000000152743***                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      ";
                }
                else if (Trx == "7051")//consulta y ejec: sedapal
                {
                    HResult = "**08785HB        000000000152743***                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      ";
                }
                else if (Trx == ListaTransacciones.PagoClaroCelular)//ejecucion
                {
                    HResult = "**08785HB        000000000152743***                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      ";
                }
                else if (Trx == "7031")//ejecucion: movistar cel y fijo, edelnor y luz del sur
                {
                    HResult = "**08785HB        000000000152743***                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      ";
                }
                //Stream s = new MemoryStream(ASCIIEncoding.Default.GetBytes(HResult));
                //Encoding encode = Encoding.GetEncoding(850);
                //StreamReader readStream = new StreamReader(s, encode);
                //HResult = readStream.ReadLine();
                //}

                //catch (Exception Ex)
                //{
                //    throw Ex;
                //}

                return HResult.ToString();
            }
            catch (Exception ex)
            {
                //LogManager.Log(ex, BFP.Library.Utils.Enums.LogType.Xml);
                throw ex;
            }
        }

        public string TransferenciaCtaPropia(string Trx, short Length, string Message)
        {
            return null;
        }

        //[WebMethod]
        //public string SendMessageAdd(string Trx, short Length, string Message, string progname)
        //{
        //    try
        //    {
        //        string SetSend = string.Empty;

        //        string _CodigoPlaza = ConfigurationManager.AppSettings.Get("_CodigoPlaza"); //"320";
        //        string _NroLogHost = ConfigurationManager.AppSettings.Get("_NroLogHost"); //"00000";
        //        string _CodigoUsuario = ConfigurationManager.AppSettings.Get("_CodigoUsuario"); //"022";
        //        string _CodigoSucursalUsuario = ConfigurationManager.AppSettings.Get("_CodigoSucursalUsuario"); //"22";
        //        string _CodigoUnicoOperador = ConfigurationManager.AppSettings.Get("_CodigoUnicoOperador"); //"622";
        //                                                                                                    //string _CodigoRecibidorPagador = ConfigurationManager.AppSettings.Get("_CodigoRecibidorPagador"); //"WEBSERVER ";
        //        string _CodigoEstacion = ConfigurationManager.AppSettings.Get("_CodigoEstacion"); //"WEBSERVER ";
        //        string _CodigoSIAFUsuario = ConfigurationManager.AppSettings.Get("_CodigoSIAFUsuario"); //"WEBSERVER ";

        //        string FechaSiaf = string.Empty; //"250706";

        //        if (ConfigurationManager.AppSettings.Get("blnFechaFija") == "1")
        //            FechaSiaf = ConfigurationManager.AppSettings.Get("strFechaFija"); //"250706";
        //        else
        //            FechaSiaf = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString().Substring(2, 2);

        //        StringBuilder _HeadMessage = new StringBuilder(4097);

        //        string OutMessage = string.Empty;
        //        string HResult = string.Empty;

        //        SetSend = SetSender();

        //        SIXConnection.Host = ConfigurationManager.AppSettings.Get("HOST"); //"WEBUSR01";
        //        SIXConnection.User = ConfigurationManager.AppSettings.Get("USER"); //"USER0001";
        //        SIXConnection.Password = ConfigurationManager.AppSettings.Get("PASSWORD"); //"USER0001";
        //        SIXConnection.Server = ConfigurationManager.AppSettings.Get("SERVER"); //"WDESA000";
        //        SIXConnection.Application = ConfigurationManager.AppSettings.Get("APPLICATION"); //"WDESA000";
        //        SIXConnection.HostD = ConfigurationManager.AppSettings.Get("HOSTD"); //"scosysv";
        //        SIXConnection.Port = ConfigurationManager.AppSettings.Get("PORT"); //"portscosixdesa";

        //        _HeadMessage.Append(_CodigoPlaza + _NroLogHost + _CodigoUsuario + _CodigoSucursalUsuario + _CodigoUnicoOperador + _CodigoEstacion + Trx + System.DateTime.Now.Hour.ToString().PadLeft(2, '0') + System.DateTime.Now.Minute.ToString().PadLeft(2, '0') + System.DateTime.Now.Second.ToString().PadLeft(2, '0') + _CodigoSIAFUsuario + "          " + "00000" + " " + "0" + "2" + "0" + "0" + "0" + "A" + FechaSiaf + progname.PadRight(8, ' ') + Message);

        //        SIXConnection.MMessage = 4096;
        //        SIXConnection.LMessage = Length;

        //        SIXConnection.Message = _HeadMessage.ToString();

        //        OutMessage = SIXConnection.SendMessage(_HeadMessage.ToString());

        //        HResult = SIXConnection.MessageOut;

        //        Stream s = new MemoryStream(ASCIIEncoding.Default.GetBytes(HResult));
        //        Encoding encode = Encoding.GetEncoding(850);
        //        StreamReader readStream = new StreamReader(s, encode);
        //        HResult = readStream.ReadLine();

        //        return HResult.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Log(ex, BFP.Library.Utils.Enums.LogType.Xml);
        //        throw ex;
        //    }
        //}

        //[WebMethod]
        //public string SendMessageCash(string Trx, short Length, string Message, string strCodigoRecibidorPagador)
        //{
        //    try
        //    {

        //        string l_Status = string.Empty;
        //        string SetSend = string.Empty;

        //        string _CodigoPlaza = ConfigurationSettings.AppSettings.Get("_CodigoPlaza"); //"320";
        //        string _NroLogHost = ConfigurationSettings.AppSettings.Get("_NroLogHost"); //"00000";
        //        string _CodigoUsuario = ConfigurationSettings.AppSettings.Get("_CodigoUsuario"); //"022";
        //        string _CodigoSucursalUsuario = ConfigurationSettings.AppSettings.Get("_CodigoSucursalUsuario"); //"22";
        //        string _CodigoUnicoOperador = ConfigurationSettings.AppSettings.Get("_CodigoUnicoOperador"); //"622";
        //        string _CodigoRecibidorPagador = strCodigoRecibidorPagador; //"WEBSERVER ";
        //        string FechaSiaf = string.Empty; //"250706";

        //        if (ConfigurationSettings.AppSettings.Get("blnFechaFija") == "1")
        //            FechaSiaf = ConfigurationSettings.AppSettings.Get("strFechaFija"); //"250706";
        //        else
        //            FechaSiaf = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString().Substring(2, 2);


        //        StringBuilder _HeadMessage = new StringBuilder(4097);

        //        string ChequeoCorrespondencia;
        //        string OutMessage = string.Empty;
        //        string HResult = string.Empty;

        //        SetSend = SetSender();

        //        SIXConnection.Host = ConfigurationSettings.AppSettings.Get("HOST"); //"WEBUSR01";
        //        SIXConnection.User = ConfigurationSettings.AppSettings.Get("USER"); //"USER0001";
        //        SIXConnection.Password = ConfigurationSettings.AppSettings.Get("PASSWORD"); //"USER0001";
        //        SIXConnection.Server = ConfigurationSettings.AppSettings.Get("SERVER"); //"WDESA000";
        //        SIXConnection.Application = ConfigurationSettings.AppSettings.Get("APPLICATION"); //"WDESA000";
        //        SIXConnection.HostD = ConfigurationSettings.AppSettings.Get("HOSTD"); //"scosysv";
        //        SIXConnection.Port = ConfigurationSettings.AppSettings.Get("PORT"); //"portscosixdesa";

        //        _HeadMessage.Append(_CodigoPlaza + _NroLogHost + _CodigoUsuario + _CodigoSucursalUsuario + _CodigoUnicoOperador + _CodigoRecibidorPagador + Trx + System.DateTime.Now.Hour.ToString().PadLeft(2, '0') + System.DateTime.Now.Minute.ToString().PadLeft(2, '0') + System.DateTime.Now.Second.ToString().PadLeft(2, '0') + _CodigoRecibidorPagador + "          " + "00000" + " " + "0" + "2" + "0" + "0" + "0" + "A" + FechaSiaf + "        " + Message);
        //        ChequeoCorrespondencia = _HeadMessage.ToString().Substring(57, 5) + _HeadMessage.ToString().Substring(31, 6);

        //        SIXConnection.MMessage = 4096;
        //        SIXConnection.LMessage = Length;

        //        SIXConnection.Message = _HeadMessage.ToString();

        //        OutMessage = SIXConnection.SendMessage(_HeadMessage.ToString());

        //        HResult = SIXConnection.MessageOut;
        //        //Byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(HResult);

        //        //string strEntrada = Encoding.ASCII.GetString(bytes);

        //        //StringBuilder sb = new StringBuilder();
        //        //for (int i = 0; i < strEntrada.Length; i++)
        //        //    if (char.IsLetterOrDigit(strEntrada[i]) || char.IsWhiteSpace(strEntrada[i]))
        //        //        sb.Append(strEntrada[i]);
        //        //	else
        //        //	    sb.Append(" ");

        //        //return sb.ToString();

        //        //return Encoding.ASCII.GetString(bytes);
        //        return HResult.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Log(ex, BFP.Library.Utils.Enums.LogType.Xml);
        //        throw ex;
        //    }
        //}

        public SixService()
        {
            //InitializeComponent(); 
        }
    }
}
