using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace NS.MBX_amarin.BusinessLogic
{
    public abstract class SEBase
    {
        /// <summary>
        /// Timeout para la consulta del web service.
        /// </summary>
        protected static int Timeout { get; private set; }

        /// <summary>
        /// Hashtable para la lectura de las cadenas de conexion.
        /// </summary>
        protected static StringDictionary UrlServicios { get; private set; }

        /// <summary>
        /// Constructor por defecto, carga las cadenas de conexion.
        /// </summary>
        static SEBase()
        {
            if (UrlServicios == null)
            {
                UrlServicios = new StringDictionary();

                //UrlServicios["MoneyGram"] = SettingsManager.Group("Servicios")["MoneyGram"].ToString();
                //UrlServicios["DB2CashManagement"] = System.Configuration.ConfigurationManager.AppSettings.Get("URLDB2CASH");
                //UrlServicios["HomeBaking"] = System.Configuration.ConfigurationManager.AppSettings.Get("URLHOMEBANKINGWS");
                //UrlServicios["CashManagement"] = System.Configuration.ConfigurationManager.AppSettings.Get("URLCASHWS"); //SettingsManager.Group("Servicios")["CashManagement"].ToString();
                //UrlServicios["XeroxPDF"] = System.Configuration.ConfigurationManager.AppSettings.Get("URLSWXeroxPDF"); //SettingsManager.Group("Servicios")["CashManagement"].ToString();
                //UrlServicios["Encripta"] = System.Configuration.ConfigurationManager.AppSettings.Get("URLEncripta");
                //UrlServicios["ServicioEnLinea"] = System.Configuration.ConfigurationManager.AppSettings.Get("URLServicioEnLinea");
                ////UrlServicios["DataVoucher"] = SettingsManager.Group("Servicios")["DataVoucher"].ToString();
                //Timeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TIMEOUTSERVICIOWEBHB")) * 1000;
            }

        }



    }
}
