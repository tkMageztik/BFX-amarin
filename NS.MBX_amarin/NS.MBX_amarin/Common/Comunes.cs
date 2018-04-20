using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NS.MBX_amarin.Common
{
    public static class Comunes
    {
        public enum TipoSession
        {
            tsCash = 0,
            tsHomeBanking = 1
        }

        //public static void AlmacenaSession(string strKey, string strValor, string strFolder)
        //{
        //    HttpContext.Current.Session[strKey] = strValor;
        //    string strPath = HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings.Get("strRutaSessiones") + strFolder + "\\" + strKey + ".txt";
        //    string strPathFolder = HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings.Get("strRutaSessiones") + strFolder + "\\";

        //    if (!Directory.Exists(strPathFolder))
        //        Directory.CreateDirectory(strPathFolder);


        //    if (File.Exists(strPath))
        //        File.Delete(strPath);

        //    FileStream fs = File.Open(strPath, FileMode.OpenOrCreate);
        //    StreamWriter sw = new StreamWriter(fs);
        //    sw.Write(strValor);
        //    sw.Flush();
        //    fs.Flush();
        //    sw.Close();
        //    fs.Close();
        //}

        //public static void AppendLog(string strValor, string strFolder)
        //{
        //    string strPath = HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings.Get("strRutaSessiones") + strFolder + "\\appLog.txt";
        //    string strPathFolder = HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings.Get("strRutaSessiones") + strFolder + "\\";

        //    if (!Directory.Exists(strPathFolder))
        //        Directory.CreateDirectory(strPathFolder);

        //    FileStream fs = File.Open(strPath, FileMode.Append);
        //    StreamWriter sw = new StreamWriter(fs);
        //    sw.WriteLine('[' + DateTime.Now.ToShortDateString() + ' ' + DateTime.Today.ToShortTimeString() + ']' + strValor);
        //    sw.Flush();
        //    fs.Flush();
        //    sw.Close();
        //    fs.Close();
        //}

        //public static void AlmacenaValor(string strKey, string strValor, string strFolder)
        //{
        //    string strPath = HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings.Get("strRutaSessiones") + strFolder + "\\" + strKey + ".txt";
        //    string strPathFolder = HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings.Get("strRutaSessiones") + strFolder + "\\";

        //    if (!Directory.Exists(strPathFolder))
        //        Directory.CreateDirectory(strPathFolder);


        //    if (File.Exists(strPath))
        //        File.Delete(strPath);

        //    FileStream fs = File.Open(strPath, FileMode.OpenOrCreate);
        //    StreamWriter sw = new StreamWriter(fs);

        //    sw.Write(strValor);
        //    sw.Flush();
        //    fs.Flush();
        //    sw.Close();
        //    fs.Close();
        //}

        //public static void jsEnviarAlert(Page page, string strId, string strMessage)
        //{
        //    //page.ClientScript.RegisterStartupScript(page.GetType(), strId, "<script type='text/javascript'>alert('" + strMessage + "');</script>");
        //}

        //public static string Moneda_Descripcion(string strCodigoMoneda)
        //{
        //    General objGeneral = new General();
        //    DataTable dtMoneda = new DataTable();
        //    using (IDataReader reader = objGeneral.ObtenerTablaMaestra(TablasMaestras.Monedas()))
        //    {
        //        dtMoneda.Load(reader);
        //    }
        //    DataView dvMoneda = dtMoneda.DefaultView;

        //    dvMoneda.RowFilter = "strCodigoInterno = '" + strCodigoMoneda + "'";

        //    if (dvMoneda.Count > 0)
        //        return dvMoneda[0]["strDescripcion"].ToString();
        //    else
        //        return string.Empty;
        //}

        //public static string Moneda_Prefijo(string strCodigoMoneda)
        //{
        //    General objGeneral = new General();
        //    DataTable dtMoneda = new DataTable();
        //    using (IDataReader reader = objGeneral.ObtenerTablaMaestra(TablasMaestras.Monedas()))
        //    {
        //        dtMoneda.Load(reader);
        //    }
        //    DataView dvMoneda = dtMoneda.DefaultView;

        //    dvMoneda.RowFilter = "strCodigoInterno = '" + strCodigoMoneda + "'";

        //    if (dvMoneda.Count > 0)
        //        return dvMoneda[0]["strPrefijo"].ToString();
        //    else
        //        return string.Empty;
        //}

        public static string FormateaNumero(string strNumero)
        {
            string strFormato = "{0:#,##0.00}";

            try
            {
                double dbl = double.Parse(strNumero);
                return String.Format(strFormato, dbl);
            }
            catch (Exception ex)
            {
                return strNumero;
            }

        }

        public static string RemoverCaracteresEspeciales(string strEntrada)
        {
            strEntrada = strEntrada.Replace("á", "a");
            strEntrada = strEntrada.Replace("é", "e");
            strEntrada = strEntrada.Replace("í", "i");
            strEntrada = strEntrada.Replace("ó", "o");
            strEntrada = strEntrada.Replace("ú", "u");
            strEntrada = strEntrada.Replace("ü", "u");
            strEntrada = strEntrada.Replace("ñ", "n");

            strEntrada = strEntrada.Replace("Á", "A");
            strEntrada = strEntrada.Replace("É", "E");
            strEntrada = strEntrada.Replace("Í", "I");
            strEntrada = strEntrada.Replace("Ó", "O");
            strEntrada = strEntrada.Replace("Ú", "U");
            strEntrada = strEntrada.Replace("Ñ", "N");

            strEntrada = strEntrada.Replace("°", " ");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strEntrada.Length; i++)
                if (char.IsLetterOrDigit(strEntrada[i]) || char.IsWhiteSpace(strEntrada[i]))
                    sb.Append(strEntrada[i]);

            return sb.ToString();
        }

        //public static DataTable ObtieneDataMovimientos(DataSet dsEntrada, DataSet dsFuente, DataSet dsFuenteTar)
        //{
        //    General objGeneral = new General();
        //    DataTable dtEntrada = dsEntrada.Tables[0].Clone();
        //    DataTable dtFuente = dsFuente.Tables[1];
        //    bool blnAdicional = false;

        //    if (string.Compare(dsEntrada.Tables["OData"].Rows[0]["ODtipta"].ToString(), "A") == 0)
        //    {
        //        blnAdicional = true;
        //    }

        //    if (!dtEntrada.Columns.Contains("ODdescrip"))
        //        dtEntrada.Columns.Add("ODdescrip");
        //    if (!dtEntrada.Columns.Contains("Cuentas"))
        //        dtEntrada.Columns.Add("Cuentas");


        //    if (!blnAdicional)
        //    {
        //        DataTable dtMoneda = new DataTable();
        //        using (IDataReader reader = objGeneral.ObtenerTablaMaestra(TablasMaestras.Monedas()))
        //        {
        //            dtMoneda.Load(reader);
        //        }
        //        DataView dvMoneda = dtMoneda.DefaultView;

        //        HttpContext.Current.Session["dvMoneda"] = dvMoneda;
        //        if (dtFuente != null)
        //        {
        //            foreach (DataRow drDato in dtFuente.Rows)
        //            {
        //                if (drDato["ODtipct"].ToString().Trim() == "330" || drDato["ODtipct"].ToString().Trim() == "210" || drDato["ODtipct"].ToString().Trim() == "110")
        //                {
        //                    DataRow drEntrada = dtEntrada.NewRow();
        //                    drEntrada["ODtipct"] = drDato["ODtipct"].ToString();


        //                    dvMoneda.RowFilter = string.Format("strPrefijo='{0}'", drDato["ODmoned"].ToString().Trim());
        //                    //dvMoneda.RowFilter = "strPrefijo='" + drDato["ODmoned"].ToString().Trim() + "'";
        //                    string strCod = dvMoneda[0]["strCodigoInterno"].ToString();

        //                    drEntrada["ODmoned"] = strCod.ToString().Trim();
        //                    drEntrada["ODcodct"] = "0000" + drDato["ODcodct"].ToString().Trim();
        //                    drEntrada["ODmonto"] = drDato["ODslddi"].ToString();
        //                    drEntrada["ODdescrip"] = drDato["ODdescr"].ToString();
        //                    drEntrada["Cuentas"] = string.Format("{0} - {1}({2}{3})", drDato["ODdescr"].ToString().Trim().PadRight(30, ' '), drDato["ODcodct"].ToString(), drDato["ODmoned"].ToString().Trim(), drDato["ODslddi"].ToString());
        //                    //drEntrada["Cuentas"] = drDato["ODdescr"].ToString().Trim().PadRight(30, ' ') + " - " + drDato["ODcodct"].ToString() + "(" + drDato["ODmoned"].ToString().Trim() + drDato["ODslddi"].ToString() + ")";

        //                    dtEntrada.Rows.Add(drEntrada);
        //                }
        //            }

        //        }

        //        dtMoneda = null;
        //        dvMoneda = null;
        //    }
        //    if (dsFuenteTar != null)
        //    {
        //        foreach (DataRow drDato in dsFuenteTar.Tables[1].Rows)
        //        {
        //            if (!blnAdicional)
        //            {
        //                if (drDato["ODnumTj"].ToString().Trim() != "")
        //                {

        //                    DataRow dr;
        //                    dr = dtEntrada.NewRow();

        //                    dr["OdTipCt"] = (DefaultValues.CodigoTarjetasCredito()).ToString();
        //                    dr["ODCodCt"] = drDato["ODnumTj"].ToString().Trim();
        //                    dr["ODMonto"] = drDato["ODLinCr"].ToString();
        //                    dr["ODDescrip"] = drDato["ODDscTj"].ToString();
        //                    dr["Cuentas"] = string.Format("{0} - {1}", dr["ODDescrip"].ToString().Trim().PadRight(30, ' '), dr["ODCodCt"].ToString());
        //                    //dr["Cuentas"] = dr["ODDescrip"].ToString().Trim().PadRight(30, ' ') + " - " + dr["ODCodCt"].ToString().Substring(4, 12);
        //                    dtEntrada.Rows.Add(dr);

        //                }
        //            }
        //            else
        //            {
        //                if (drDato["ODnumTj"].ToString().Trim() == HttpContext.Current.Session["strPad"].ToString().Trim())
        //                {
        //                    DataRow dr;
        //                    dr = dtEntrada.NewRow();
        //                    dr["OdTipCt"] = (DefaultValues.CodigoTarjetasCredito()).ToString();
        //                    dr["ODCodCt"] = drDato["ODnumTj"].ToString().Trim();
        //                    dr["ODMonto"] = drDato["ODLinCr"].ToString();
        //                    dr["ODDescrip"] = drDato["ODDscTj"].ToString();
        //                    dr["Cuentas"] = string.Format("{0} - {1}", dr["ODDescrip"].ToString().Trim().PadRight(30, ' '), dr["ODCodCt"].ToString());
        //                    //dr["Cuentas"] = dr["ODDescrip"].ToString().Trim().PadRight(30, ' ') + " - " + dr["ODCodCt"].ToString().Substring(4, 12);
        //                    dtEntrada.Rows.Add(dr);
        //                }
        //            }
        //        }
        //    }


        //    return dtEntrada;
        //}

        //public static DataTable ObtieneDataMovimientosVISA(DataSet dsEntrada, DataSet dsFuenteTar)
        //{
        //    General objGeneral = new General();
        //    DataTable dtEntrada = dsEntrada.Tables[0].Clone();


        //    if (!dtEntrada.Columns.Contains("ODdescrip"))
        //        dtEntrada.Columns.Add("ODdescrip");
        //    if (!dtEntrada.Columns.Contains("Cuentas"))
        //        dtEntrada.Columns.Add("Cuentas");

        //    if (dsFuenteTar != null)
        //    {
        //        foreach (DataRow drDato in dsFuenteTar.Tables[1].Rows)
        //        {

        //            if (drDato["ODnumTj"].ToString().Trim() == HttpContext.Current.Session["strPad"].ToString().Trim())
        //            {
        //                DataRow dr;
        //                dr = dtEntrada.NewRow();
        //                dr["OdTipCt"] = (DefaultValues.CodigoTarjetasCredito()).ToString();
        //                dr["ODCodCt"] = drDato["ODnumTj"].ToString().Trim();
        //                dr["ODMonto"] = drDato["ODLinCr"].ToString();
        //                dr["ODDescrip"] = drDato["ODDscTj"].ToString();
        //                dr["Cuentas"] = string.Format("{0} - {1}", dr["ODDescrip"].ToString().Trim().PadRight(30, ' '), dr["ODCodCt"].ToString());
        //                //dr["Cuentas"] = dr["ODDescrip"].ToString().Trim().PadRight(30, ' ') + " - " + dr["ODCodCt"].ToString().Substring(4, 12);
        //                dtEntrada.Rows.Add(dr);
        //            }

        //        }
        //    }


        //    return dtEntrada;
        //}

        //public static bool ValidaModulo10(string strNumeroValidar)
        //{
        //    strNumeroValidar = Regex.Replace(strNumeroValidar, @"[^0-9]", "");

        //    int LongitudCadena = strNumeroValidar.Length;
        //    int intImpar = 0;
        //    int intUniforme = 0;

        //    char[] arrNumero = new char[LongitudCadena];
        //    arrNumero = strNumeroValidar.ToCharArray();
        //    Array.Reverse(arrNumero, 0, LongitudCadena);
        //    for (int i = 0; i < LongitudCadena; i++)
        //    {
        //        if (i % 2 == 0)
        //        {
        //            intImpar += (Convert.ToInt32(arrNumero.GetValue(i)) - 48);
        //        }
        //        else
        //        {
        //            int intTemp = (Convert.ToInt32(arrNumero[i]) - 48) * 2;
        //            if (intTemp > 9)
        //            {
        //                intTemp = intTemp - 9;
        //            }
        //            intUniforme += intTemp;
        //        }
        //    }
        //    if ((intImpar + intUniforme) % 10 == 0)
        //        return true;
        //    else
        //        return false;
        //}

        //public static DataSet ActualizaNombreCuentasLocal(DataSet dsEntrada, DataSet dsFuente, DataSet dsFuenteTar)
        //{
        //    General objGeneral = new General();
        //    DataTable dtEntrada = dsEntrada.Tables[0];
        //    bool blnAdicional = false;

        //    if (string.Compare(dsEntrada.Tables["OData"].Rows[0]["ODtipta"].ToString(), "A") == 0)
        //    //if (dsEntrada.Tables["OData"].Rows[0]["ODtipta"].ToString().Equals("A"))
        //    {
        //        blnAdicional = true;
        //    }
        //    if (!dtEntrada.Columns.Contains("ODdescrip"))
        //        dtEntrada.Columns.Add("ODdescrip");
        //    if (!dtEntrada.Columns.Contains("Cuentas"))
        //        dtEntrada.Columns.Add("Cuentas");

        //    DataTable dtMoneda = new DataTable();
        //    using (IDataReader reader = objGeneral.ObtenerTablaMaestra(TablasMaestras.Monedas()))
        //    {
        //        dtMoneda.Load(reader);
        //    }

        //    DataView dvMoneda = dtMoneda.DefaultView;

        //    HttpContext.Current.Session["dvMoneda"] = dvMoneda;

        //    if (!blnAdicional)
        //    {

        //        if (dtEntrada != null)
        //        {
        //            foreach (DataRow drDato in dtEntrada.Rows)
        //            {
        //                DataRow[] dr = dsFuente.Tables[1].Select("ODcodct='" + drDato["ODcodct"].ToString().Trim().Substring(4, 12) + "'");

        //                if (dr.Length == 1)
        //                {
        //                    drDato["ODdescrip"] = dr[0]["ODdescr"].ToString();

        //                    dvMoneda.RowFilter = "strCodigoInterno='" + drDato["ODmoned"].ToString() + "'";
        //                    string strPrefixMoneda = dvMoneda[0]["strPrefijo"].ToString();
        //                    drDato["Cuentas"] = drDato["ODdescrip"].ToString().Trim().PadRight(30, ' ') + " - " + drDato["ODcodct"].ToString().Substring(4, 12) + "(" + strPrefixMoneda + drDato["ODsigno"].ToString().Trim() + drDato["ODmonto"].ToString() + ")";
        //                }

        //            }
        //        }

        //    }

        //    if (dsFuenteTar != null)
        //    {
        //        foreach (DataRow drDato in dsFuenteTar.Tables[1].Rows)
        //        {
        //            if (!blnAdicional)
        //            {
        //                if (drDato["ODnumTj"].ToString().Trim() != "")
        //                {
        //                    DataRow[] drw = dsEntrada.Tables[0].Select("ODcodct='" + drDato["ODnumtj"].ToString() + "'");
        //                    if (drw.Length == 0)
        //                    {
        //                        DataRow dr;
        //                        dr = dtEntrada.NewRow();
        //                        //dvSalida.RowFilter = "ODtipct=" + drOpciones.GetString(1).Trim() + " AND ODmoned='S/.'";
        //                        dr["OdTipCt"] = (DefaultValues.CodigoTarjetasCredito()).ToString();
        //                        dr["ODCodCt"] = drDato["ODnumTj"].ToString();
        //                        //Midificado 05/03/2009
        //                        dr["ODmoned"] = drDato["ODmonsd"].ToString() == "S/. " ? "604" : "840"; //nueva linea
        //                                                                                                //dr["ODMonto"] = drDato["ODLinCr"].ToString();
        //                        dr["ODMonto"] = drDato["ODsldlc"].ToString();
        //                        dr["ODDescrip"] = drDato["ODDscTj"].ToString();
        //                        dr["Cuentas"] = dr["ODDescrip"].ToString().Trim().PadRight(30, ' ') + " - " + dr["ODCodCt"].ToString();
        //                        dtEntrada.Rows.Add(dr);
        //                    }

        //                }

        //            }
        //            else
        //            {
        //                if (drDato["ODnumTj"].ToString().Trim() != "")
        //                {
        //                    if (drDato["ODnumTj"].ToString().Trim() == HttpContext.Current.Session["strPad"].ToString().Trim())
        //                    {
        //                        DataRow[] drw = dsEntrada.Tables[0].Select("ODcodct='" + drDato["ODnumtj"].ToString() + "'");
        //                        if (drw.Length == 0)
        //                        {
        //                            DataRow dr;
        //                            dr = dtEntrada.NewRow();
        //                            dr["OdTipCt"] = (DefaultValues.CodigoTarjetasCredito()).ToString();
        //                            dr["ODCodCt"] = drDato["ODnumTj"].ToString();
        //                            dr["ODMonto"] = drDato["ODLinCr"].ToString();
        //                            dr["ODDescrip"] = drDato["ODDscTj"].ToString();
        //                            dr["Cuentas"] = dr["ODDescrip"].ToString().Trim().PadRight(30, ' ') + " - " + dr["ODCodCt"].ToString();
        //                            dtEntrada.Rows.Add(dr);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    return dsEntrada;
        //}

        public static bool IsDate(string fecha)
        {
            bool result = true;
            DateTime date;

            try
            {
                date = Convert.ToDateTime(fecha);
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
