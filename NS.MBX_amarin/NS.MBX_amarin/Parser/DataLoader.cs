using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace NS.MBX_amarin.Parser
{
    public class DataLoader
    {
        int _intPosicion = 0;
        public string rutaXml = "NS.MBX_amarin.Xml.";

        public DataSet ObtenerData(string strTrama, string strNombreMensaje, string strNombreTransaccion, int intPosicionInicial)
        {
            DataSet dsResult = null;
            XmlDocument xmlDom = null;
            XmlNode xmlNode = null;
            try
            {
                _intPosicion = intPosicionInicial;
                dsResult = new DataSet(strNombreTransaccion);
                xmlDom = LeerXML(strNombreTransaccion, strNombreMensaje);
                xmlNode = xmlDom.GetElementsByTagName("OData")[0];
                GenerateDatatable(ref dsResult, xmlNode, "OData");
                FillDataTable(ref dsResult, xmlNode, "OData", strTrama);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xmlDom = null;
                xmlNode = null;
            }
        }

        public DataSet EmularData(string strNombreMensaje, string strNombreTransaccion, int intPosicionInicial)
        {
            _intPosicion = intPosicionInicial;
            DataSet dsResult = new DataSet(strNombreTransaccion);
            XmlDocument xmlDom = LeerXML(strNombreTransaccion, strNombreMensaje);
            XmlNode xmlNode = xmlDom.GetElementsByTagName("OData")[0];
            GenerateDatatable(ref dsResult, xmlNode, "OData");
            return dsResult;
        }

        public DataSet ObtenerCabecera(string strTrama, string strNombreMensaje, string strNombreTransaccion, int intPosicionInicial)
        {
            _intPosicion = intPosicionInicial;
            DataSet dsResult = new DataSet(strNombreTransaccion);
            XmlDocument xmlDom = LeerXML(strNombreTransaccion, strNombreMensaje);
            XmlNode xmlNode = xmlDom.GetElementsByTagName("OHead")[0];
            GenerateDatatable(ref dsResult, xmlNode, "OHead");
            FillDataTable(ref dsResult, xmlNode, "OHead", strTrama);
            return dsResult;
        }

        public DataSet ObtenerInputTrama(string strTrama, string strNombreMensaje, string strNombreTransaccion, int intPosicionInicial)
        {
            _intPosicion = intPosicionInicial;
            DataSet dsResult = new DataSet(strNombreTransaccion);
            XmlDocument xmlDom = LeerXML(strNombreTransaccion, strNombreMensaje);
            XmlNode xmlNode = xmlDom.GetElementsByTagName("IData")[0];
            GenerateDatatable(ref dsResult, xmlNode, "IData");
            FillDataTableIn(ref dsResult, xmlNode, "IData", strTrama);
            return dsResult;
        }

        public DataSet ObtenerControlsInput(string strTrama, string strNombreMensaje, string strNombreTransaccion, int intPosicionInicial)
        {
            _intPosicion = intPosicionInicial;
            DataSet dsResult = new DataSet(strNombreTransaccion);
            XmlDocument xmlDom = LeerXMLControlsInput(strNombreTransaccion, strNombreMensaje);
            XmlNode xmlNode = xmlDom.GetElementsByTagName("IData")[0];
            GenerateDatatable(ref dsResult, xmlNode, "IData");
            FillDataTableIn(ref dsResult, xmlNode, "IData", strTrama);
            return dsResult;
        }

        private XmlDocument LeerXMLControlsInput(string strNombreTransaccion, string strNombreMensaje)
        {
            XmlDocument xmlDom = new XmlDocument();
            string fileName = string.Format("{0}MBXCtrl{1}{2}.xml", rutaXml, strNombreTransaccion, strNombreMensaje);

            //if (System.IO.File.Exists(System.Configuration.ConfigurationSettings.AppSettings.Get("XMLPATH") + "BFPCtrl" + strNombreTransaccion + strNombreMensaje + ".xml"))
            if (!System.IO.File.Exists(string.Format("{0}MBXCtrl{1}{2}.xml", rutaXml, strNombreTransaccion, strNombreMensaje)))
            {
                //xmlDom.Load(System.Configuration.ConfigurationSettings.AppSettings.Get("XMLPATH") + "BFPCtrl" + strNombreTransaccion + strNombreMensaje + ".xml");
                //xmlDom.Load(string.Format("{0}MBXCtrl{1}{2}.xml", rutaXml, strNombreTransaccion, strNombreMensaje));
                Assembly asm = Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream(fileName))
                {
                    xmlDom.Load(stream);
                }
            }
            else
            {
                XmlGenerator xmlGen = new XmlGenerator();
                xmlDom = xmlGen.ObtenerXmlControlsInput(strNombreTransaccion, strNombreMensaje);
            }

            return xmlDom;
        }

        private XmlDocument LeerXML(string strNombreTransaccion, string strNombreMensaje)
        {
            XmlDocument xmlDom = new XmlDocument();
            string fileName = string.Format("{0}MBX{1}{2}.xml", rutaXml, strNombreTransaccion, strNombreMensaje);

            //if (System.IO.File.Exists(System.Configuration.ConfigurationSettings.AppSettings.Get("XMLPATH") + "BFP" +  strNombreTransaccion + strNombreMensaje + ".xml"))
            //System.IO.FileStream file = null;
            //try
            //{
            //    Assembly asm = Assembly.GetExecutingAssembly();
            //    string[] test = asm.GetManifestResourceNames();
            //    using (Stream stream = asm.GetManifestResourceStream("NS.MBX_amarin." + fileName))
            //    {
            //        xmlDom.Load(stream);
            //    }
            //}
            //catch(Exception ex)
            //{
            //    if (file != null)
            //        file.Flush();
            //    Console.WriteLine(ex.ToString());
            //}
            if (!System.IO.File.Exists(string.Format("{0}MBX{1}{2}.xml", rutaXml, strNombreTransaccion, strNombreMensaje)))
            {
                //xmlDom.Load(System.Configuration.ConfigurationSettings.AppSettings.Get("XMLPATH") + "BFP" + strNombreTransaccion + strNombreMensaje + ".xml");
                //xmlDom.Load(string.Format("{0}MBX{1}{2}.xml", rutaXml, strNombreTransaccion, strNombreMensaje));
                Assembly asm = Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream(fileName))
                {
                    xmlDom.Load(stream);
                }
            }
            else
            {
                XmlGenerator xmlGen = new XmlGenerator();
                xmlDom = xmlGen.ObtenerXml(strNombreTransaccion, strNombreMensaje);
            }

            return xmlDom;
        }

        void FillDataTable(ref DataSet dsPadre, XmlNode xmlNode, string strName, string strTrama)
        {
            DataTable dtOut = dsPadre.Tables[strName];
            DataRow drData = dtOut.NewRow();

            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.HasChildNodes)
                {
                    drData[node.Name + "ID"] = "__ID";
                    for (int i = 0; i < int.Parse(node.Attributes["Size"].Value); i++)
                    {
                        if (strTrama.Substring(_intPosicion, int.Parse(node.Attributes["Length"].Value)).Trim().Length == 0)
                        {
                            int intNewPos = int.Parse(node.Attributes["Size"].Value) - i;
                            int intLongitud = int.Parse(node.Attributes["Length"].Value);
                            _intPosicion = _intPosicion + intNewPos * intLongitud;
                            break;
                        }

                        FillDataTable(ref dsPadre, node, node.Name, strTrama);
                    }
                }
                else
                {
                    switch (dtOut.Columns[node.Name].DataType.Name)
                    {
                        case "Double":
                            string strEntero = strTrama.Substring(_intPosicion, int.Parse(node.Attributes["Length"].Value) - int.Parse(node.Attributes["Decimal"].Value));
                            string strDecimal = strTrama.Substring(_intPosicion + strEntero.Length, int.Parse(node.Attributes["Decimal"].Value));

                            if (strEntero.Trim().Length == 0)
                            {
                                strEntero = "0";
                            }

                            if (strDecimal.Trim().StartsWith("-"))
                            {
                                strEntero = "-" + strEntero;
                                strDecimal = strDecimal.Substring(1);
                                strDecimal = strDecimal.Trim().PadLeft(int.Parse(node.Attributes["Decimal"].Value), '0');

                                if (strDecimal.Trim().Length == 0)
                                {
                                    strDecimal = "0";
                                    strDecimal = strDecimal.Trim().PadLeft(int.Parse(node.Attributes["Decimal"].Value), '0');
                                }

                            }
                            else
                            {
                                strDecimal = strDecimal.Trim().PadLeft(int.Parse(node.Attributes["Decimal"].Value), '0');
                            }


                            if (strEntero.Trim().Length == 0 && strDecimal.Trim().Length == 0)
                            {
                                drData[node.Name] = "0.0";
                            }
                            else
                            {
                                drData[node.Name] = strEntero.Trim() + "." + strDecimal.Trim();
                            }

                            break;
                        case "DateTime":
                            if (int.Parse(node.Attributes["Length"].Value) == 8)
                            {
                                string strDia = strTrama.Substring(_intPosicion, 2);
                                string strMes = strTrama.Substring(_intPosicion + 2, 2);
                                string strAnio = strTrama.Substring(_intPosicion + 4, 4);

                                drData[node.Name] = DateTime.Parse(strDia + "/" + strMes + "/" + strAnio);
                            }
                            else
                            {
                                throw new Exception("La fecha tiene un tamaño inválido");
                            }

                            break;

                        default:
                            if (strTrama.Length >= int.Parse(node.Attributes["Length"].Value))
                            {
                                drData[node.Name] = strTrama.Substring(_intPosicion, int.Parse(node.Attributes["Length"].Value));
                            }
                            else
                            {
                                drData[node.Name] = strTrama;
                            }
                            break;
                    }

                    _intPosicion = _intPosicion + int.Parse(node.Attributes["Length"].Value);
                }
            }

            dtOut.Rows.Add(drData);
        }

        void FillDataTableIn(ref DataSet dsPadre, XmlNode xmlNode, string strName, string strTrama)
        {
            DataTable dtOut = dsPadre.Tables[strName];
            DataRow drData = dtOut.NewRow();

            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.HasChildNodes)
                {
                    drData[node.Name + "ID"] = "__ID";
                    for (int i = 0; i < int.Parse(node.Attributes["Size"].Value); i++)
                    {
                        if (strTrama.Substring(_intPosicion, int.Parse(node.Attributes["Length"].Value)).Trim().Length == 0)
                        {
                            int intNewPos = int.Parse(node.Attributes["Size"].Value) - i;
                            int intLongitud = int.Parse(node.Attributes["Length"].Value);
                            _intPosicion = _intPosicion + intNewPos * intLongitud;
                            break;
                        }

                        FillDataTable(ref dsPadre, node, node.Name, strTrama);
                    }
                }
                else
                {
                    switch (dtOut.Columns[node.Name].DataType.Name)
                    {
                        case "Double":
                            string strEntero = strTrama.Substring(_intPosicion, int.Parse(node.Attributes["Length"].Value) - int.Parse(node.Attributes["Decimal"].Value));
                            string strDecimal = strTrama.Substring(_intPosicion + strEntero.Length, int.Parse(node.Attributes["Decimal"].Value));

                            if (strEntero.Trim().Length == 0)
                            {
                                strEntero = "0";
                            }

                            if (strDecimal.Trim().StartsWith("-"))
                            {
                                strEntero = "-" + strEntero;
                                strDecimal = strDecimal.Substring(1);
                                strDecimal = strDecimal.Trim().PadLeft(int.Parse(node.Attributes["Decimal"].Value), '0');

                                if (strDecimal.Trim().Length == 0)
                                {
                                    strDecimal = "0";
                                    strDecimal = strDecimal.Trim().PadLeft(int.Parse(node.Attributes["Decimal"].Value), '0');
                                }

                            }
                            else
                            {
                                strDecimal = strDecimal.Trim().PadLeft(int.Parse(node.Attributes["Decimal"].Value), '0');
                            }


                            if (strEntero.Trim().Length == 0 && strDecimal.Trim().Length == 0)
                            {
                                drData[node.Name] = "0.0";
                            }
                            else
                            {
                                drData[node.Name] = strEntero.Trim() + "." + strDecimal.Trim();
                            }
                            _intPosicion = _intPosicion + int.Parse(node.Attributes["Length"].Value);
                            break;
                        case "DateTime":
                            if (int.Parse(node.Attributes["Length"].Value) == 8)
                            {
                                string strDia = strTrama.Substring(_intPosicion, 2);
                                string strMes = strTrama.Substring(_intPosicion + 2, 2);
                                string strAnio = strTrama.Substring(_intPosicion + 4, 4);

                                drData[node.Name] = DateTime.Parse(strDia + "/" + strMes + "/" + strAnio);
                            }
                            else
                            {
                                throw new Exception("La fecha tiene un tamaño inválido");
                            }
                            _intPosicion = _intPosicion + int.Parse(node.Attributes["Length"].Value);
                            break;

                        default:
                            if (node.Attributes["Default"].Value.ToString() == "")
                            {
                                if (strTrama.Length >= int.Parse(node.Attributes["Length"].Value))
                                {
                                    drData[node.Name] = strTrama.Substring(_intPosicion, int.Parse(node.Attributes["Length"].Value));
                                }
                                else
                                {
                                    drData[node.Name] = strTrama;
                                }
                                _intPosicion = _intPosicion + int.Parse(node.Attributes["Length"].Value);
                            }
                            else
                            {
                                drData[node.Name] = node.Attributes["Default"].Value.ToString();
                            }

                            break;
                    }

                    //Aqui estuvo la posicion
                }
            }

            dtOut.Rows.Add(drData);
        }

        void GenerateDatatable(ref DataSet dsPadre, XmlNode xmlNode, string strName)
        {
            DataTable dtOut = new DataTable(strName);
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.HasChildNodes)
                {
                    DataColumn dtCol = new DataColumn(node.Name + "ID");
                    dtOut.Columns.Add(dtCol);
                    GenerateDatatable(ref dsPadre, node, node.Name);
                }
                else
                {
                    DataColumn dtCol = new DataColumn(node.Name, System.Type.GetType(node.Attributes["Type"].Value.Trim()));
                    dtOut.Columns.Add(dtCol);
                }
            }

            dsPadre.Tables.Add(dtOut);
        }
    }
}
