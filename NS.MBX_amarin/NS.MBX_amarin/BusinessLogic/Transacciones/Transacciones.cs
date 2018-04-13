using NS.MBX_amarin.Parser;
using NS.MBX_amarin.Six;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NS.MBX_amarin.BusinessLogic.Transacciones
{
    public class Transacciones : SEBase
    {
        private DataSet _Cabecera;
        public string _strTrama = string.Empty;

        public DataSet Cabecera
        {
            get { return _Cabecera; }
            set { _Cabecera = value; }
        }

        public DataSet EjecutarTransaccion(string strNombreTransaccion, short shrLongitudCabecera, string strMensajeIn, string strNombreMensajeOut, int intPosicionInicialLecturaOut, out string strError, string strCodigoRecibidorPagador)
        {
            //objSixCommunication = new DC.Service(); // Web Services
           // var objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"], Timeout = Timeout };
            DataSet dsSalida = null;

            string strResultado = string.Empty;

            //try
            //{
            //    //3 opciones
            //    strResultado = objSixCommunication.SendMessageCash(strNombreTransaccion, shrLongitudCabecera, strMensajeIn, strCodigoRecibidorPagador);

            //    //Six con 5 parametros            
            //    //strResultado = objSixCommunication.SendMessageCash(ConfigurationSettings.AppSettings.Get("_CodigoEstacion"), ConfigurationSettings.AppSettings.Get("_CodigoSIAFUsuario"), strNombreTransaccion, shrLongitudCabecera, strMensajeIn, strCodigoRecibidorPagador);
            //    strError = strResultado.Substring(17, 4);
            //    _strTrama = strResultado;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    objSixCommunication = null;
            //}
            //if (strError == "0000")
            //{
            //    DataLoader xml = new DataLoader();
            //    Transacciones ch = new Transacciones();
            //    dsSalida = xml.ObtenerData(strResultado, strNombreMensajeOut, strNombreTransaccion, intPosicionInicialLecturaOut);
            //}
            strError = "0001";
            return dsSalida;
        }

        public DataSet EjecutarTransaccion(string strNombreTransaccion, short shrLongitudCabecera, string strMensajeIn, string strNombreMensajeOut, int intPosicionInicialLecturaOut, out string strError)
        {
            //objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"]  };
            //objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"] };
            DataSet dsSalida = null;
            string strResultado = string.Empty;
            try
            {
                //using (var objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"], Timeout = Timeout })
                //using (var objSixCommunication = new SixService())
                //{
                    strResultado = new SixService().SendMessage(strNombreTransaccion, shrLongitudCabecera, System.Web.HttpUtility.HtmlDecode(strMensajeIn));
               // }
                strError = strResultado.Substring(17, 4);

                _strTrama = strResultado;
                if (strError == "0000" || strError == "1000")//CPH - Bloqueo Consulta Cronogramas 18/02/2016- Se agrega error 1000
                {
                    DataLoader xml = new DataLoader();
                    dsSalida = xml.ObtenerData(strResultado, strNombreMensajeOut, strNombreTransaccion, intPosicionInicialLecturaOut);
                    if (dsSalida == null)
                    {
                        GenerarLogError(string.Concat(strResultado.Trim(), strError, strNombreMensajeOut, strNombreTransaccion, intPosicionInicialLecturaOut.ToString(), strError, "-->", "6666"), "error ObtenerData6");
                        strError = "6666";
                    }
                }
                return dsSalida;
            }
            catch (Exception ex)
            {
                strError = "6666";
                GenerarLogError(string.Concat(strNombreTransaccion, strError, shrLongitudCabecera.ToString(), strMensajeIn, strNombreMensajeOut, intPosicionInicialLecturaOut.ToString(), strResultado.Trim(), "--->", ex.Message, ex.StackTrace, ex.HelpLink), "Exception EjecutarTransaccion6");
                string _____error = ex.Message;
                return null;
            }
            finally
            {
                //objSixCommunication = null;
                strResultado = string.Empty;
                dsSalida = null;
            }


        }

        public string EjecutarTransaccion(string strNombreTransaccion, short shrLongitudCabecera, string strMensajeIn)
        {
            //objSixCommunication = new DC.Service(); // Web Services        
            //objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"] };
            string strError = string.Empty;
            string strResultado = string.Empty;

            try
            {
                //using (var objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"], Timeout = Timeout })
                //{
                    strResultado = new SixService().SendMessage(strNombreTransaccion, shrLongitudCabecera, strMensajeIn);
                //}
                strError = strResultado.Substring(17, 4);
                return strError;
            }
            catch (Exception ex)
            {
                strError = string.Empty;
                string ___Error = ex.Message;
                GenerarLogError(string.Concat(strNombreTransaccion, "6666", shrLongitudCabecera.ToString(), strMensajeIn, strResultado.Trim(), "--->", ex.Message, ex.StackTrace, ex.HelpLink), "Exception EjecutarTransaccion3");
                return strError;
            }
            finally
            {
                //objSixCommunication = null;
                strResultado = null;
            }


        }

        public string EjecutarTransaccion(string strNombreTransaccion, short shrLongitudCabecera, string strMensajeIn, out string __strError)
        {
            //objSixCommunication = new DC.Service(); // Web Services
            //objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"] };
            string strError = string.Empty;
            string strResultado = string.Empty;

            try
            {
               // using (var objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"], Timeout = Timeout })
               // {
                    strResultado = new SixService().SendMessage(strNombreTransaccion, shrLongitudCabecera, strMensajeIn);
              //  }
                __strError = strResultado.Substring(17, 4);
                if (__strError.Trim().Length < 4) __strError = strResultado.Substring(26, 4);
                return strResultado;
            }
            catch (Exception ex)
            {
                __strError = "6666";
                GenerarLogError(string.Concat(strNombreTransaccion, __strError, shrLongitudCabecera.ToString(), strMensajeIn, strResultado.Trim(), "--->", ex.Message, ex.StackTrace, ex.HelpLink), "Exception EjecutarTransaccion4");
                strError = ex.Message;
                return strResultado;

            }
            finally
            {
                //objSixCommunication = null;
                strResultado = null;
            }

        }

        public DataSet ObtenerCabecera(string strNombreTransaccion, string strNombreMensajeOut, int intPosicionInicialLecturaOut)
        {
            DataSet dsSalida = null;
            DataLoader xml = new DataLoader();
            try
            {
                if (_strTrama.Length > 0)
                {

                    dsSalida = xml.ObtenerCabecera(_strTrama, strNombreMensajeOut, strNombreTransaccion, intPosicionInicialLecturaOut);
                }
                return dsSalida;
            }
            catch
            {
                return null;
            }
            finally { dsSalida = null; xml = null; }
        }

        public DataSet EmularData(string strNombreMensajeOut, string strNombreTransaccion, int intPosicionInicialLecturaOut, int CantidadData)
        {
            DataLoader xml = new DataLoader();
            Transacciones ch = new Transacciones();
            using (DataSet dsSalida = xml.EmularData(strNombreMensajeOut, strNombreTransaccion, intPosicionInicialLecturaOut))
            {
                foreach (DataTable dt in dsSalida.Tables)
                {
                    for (int i = 0; i < CantidadData; i++)
                    {
                        dt.Rows.Add();
                    }
                    foreach (DataRow item in dt.Rows)
                    {
                        foreach (DataColumn item1 in dt.Columns)
                        {
                            Random r2 = new Random();
                            switch (item1.DataType.FullName.ToString())
                            {
                                case "System.Double":
                                    r2 = new Random();
                                    r2.Next(1, 10);
                                    Random r1 = new Random();
                                    double nro = (r1.NextDouble() * 2) + r2.Next();
                                    item[item1] = double.Parse(string.Format("{0:N2}", nro.ToString()));
                                    break;
                                default:
                                    item[item1] = "AAAAAA";
                                    break;
                            }
                        }
                    }

                }
                return dsSalida; // = null;
            }
        }

        public string ObtenerTipoCambio()
        {
            //objSixCommunication = new DC.Service();
            string strResultado = string.Empty;
            string Error = string.Empty;
            try
            {
                //objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"] };
                //using (var objSixCommunication = new DC.Service { Url = UrlServicios["HomeBaking"], Timeout = Timeout })
                //{
                    strResultado = new SixService().SendMessage("3007", 82, string.Empty);
                //}

                return strResultado;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                GenerarLogError(string.Concat("3007", "6666", strResultado, ex.Message), "ObtenerTipoCambio()");
                return "6666";
            }
            finally
            {  //objSixCommunication = null; 
                strResultado = null;
            }


        }
        public void GenerarLogError(string strSalida, string strUbicacion)
        {
            string strRuta = string.Empty;
            string strArchivo = string.Empty;
            string strRutaArchivo = string.Empty;
            try
            {
                //if (string.Compare(System.Configuration.ConfigurationManager.AppSettings.Get("PATHLOGBANCAMOVIL"), "") != 0)
                //{
                //    strRuta = string.Concat(System.Configuration.ConfigurationManager.AppSettings.Get("PATHLOGBANCAMOVIL"));
                //    strArchivo = string.Concat("TransaccionesHB", DateTime.Now.Year.ToString().PadLeft(4, '0'), DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0'), ".txt");
                //    strRutaArchivo = string.Concat(strRuta, strArchivo);

                //    if (System.IO.File.Exists(strRutaArchivo))
                //    {
                //        using (System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(strRutaArchivo, true, System.Text.Encoding.Unicode))
                //        {
                //            objStreamWriter.WriteLine(string.Concat("[*", DateTime.Now, "*]", strUbicacion, ": ", strSalida));
                //        }
                //    }
                //    else
                //    {
                //        using (System.IO.FileStream fs = new System.IO.FileStream(string.Format("{0}\\{1}",
                //                                                    strRuta,
                //                                                    strArchivo),
                //                                                    System.IO.FileMode.Create))
                //        {
                //            //Archivo creado ;
                //        }
                //        using (System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(strRutaArchivo, true, System.Text.Encoding.Unicode))
                //        {
                //            objStreamWriter.WriteLine(string.Concat("[*", DateTime.Now, "*]", strUbicacion, ": ", strSalida));
                //        }

                //    }
                //    strRutaArchivo = string.Empty;
                //}
            }
            catch (Exception ex)
            {
                string strEx = ex.Message.ToString();
                strEx = string.Empty;
            }
        }

        ~Transacciones()
        {
            //objSixCommunication = null;
            _Cabecera = null;
        }
    }
}
