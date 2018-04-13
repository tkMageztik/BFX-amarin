using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using DA = NS.MBX_amarin.DataAccess;

namespace NS.MBX_amarin.Parser
{
    public class XmlGenerator
    {
        private DataTable _dtMensajes;
        private DataTable _dtCampos;
        public string rutaXml = @".\Xml\";
        XmlDocument xmlDom = new XmlDocument();

        public XmlGenerator()
        {

        }

        private DataTable ObtenerMensajesPorFormulario(string strNombreFormulario)
        {
            DA.Parser objParser = new DA.Parser();
            DataTable dtMensajes = new DataTable();
            using (IDataReader reader = objParser.ObtenerMensajesPorFormulario(strNombreFormulario))
            {
                //dtMensajes.Load(objParser.ObtenerMensajesPorFormulario(strNombreFormulario));
                dtMensajes.Load(reader);
            }


            return dtMensajes;
        }

        private DataTable ObtenerMensajesPorNombre(string strNombreMensaje, string strNombreTransaccion)
        {
            DA.Parser objParser = new DA.Parser();
            DataTable dtMensajes = new DataTable();
            using (IDataReader reader = objParser.ObtenerMensajePorNombre(strNombreMensaje, strNombreTransaccion))
            {
                //dtMensajes.Load(objParser.ObtenerMensajePorNombre(strNombreMensaje, strNombreTransaccion));
                dtMensajes.Load(reader);
            }
            return dtMensajes;
        }

        private DataTable ObtenerCampos(int intMensajeID)
        {
            DA.Parser objParser = new DA.Parser();
            DataTable dtCampos = new DataTable();
            using (IDataReader reader = objParser.ObtenerCamposPorMensaje(intMensajeID))
            {
                //dtCampos.Load(objParser.ObtenerCamposPorMensaje(intMensajeID));
                dtCampos.Load(reader);
            }


            return dtCampos;
        }

        public XmlDocument ObtenerXml(string strNombreTransaccion, string strNombreMensaje)
        {
            XmlNode xmlNodoCabecera;
            XmlNode xmlNodoDetalle;

            xmlNodoCabecera = xmlDom.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmlDom.AppendChild(xmlNodoCabecera);

            xmlNodoDetalle = xmlDom.CreateElement("", "MBX" + strNombreTransaccion, "");
            xmlDom.AppendChild(xmlNodoDetalle);

            _dtMensajes = ObtenerMensajesPorNombre(strNombreMensaje, strNombreTransaccion);
            foreach (DataRow drMensaje in _dtMensajes.Rows)
            {
                _dtCampos = ObtenerCampos((int)drMensaje["intMensajeID"]);

                XmlNode xmlNodoMensaje;
                xmlNodoMensaje = xmlDom.CreateElement("", drMensaje["strNombre"].ToString(), "");

                xmlNodoDetalle.AppendChild(xmlNodoMensaje);

                foreach (DataRow drCampo in _dtCampos.Rows)
                {
                    if (string.Compare((string)drCampo["strNombrePadre"], (string)drCampo["strNombre"]) == 0)
                    //if (drCampo["strNombrePadre"].ToString().Equals(drCampo["strNombre"].ToString()))
                    {
                        XmlElement xmlNodoPadre;

                        xmlNodoPadre = xmlDom.CreateElement("", drCampo["strNombre"].ToString(), "");

                        xmlNodoMensaje.AppendChild(xmlNodoPadre);
                        AgregarNodo(ref xmlNodoPadre, _dtCampos);
                    }

                }
            }


            //xmlDom.Save(System.Configuration.ConfigurationSettings.AppSettings.Get("XMLPATH") + "BFP" + strNombreTransaccion + strNombreMensaje + ".xml");
            xmlDom.Save(string.Format("{0}MBX{1}{2}.xml", rutaXml, strNombreTransaccion, strNombreMensaje));

            return xmlDom;
        }

        public XmlDocument ObtenerXmlControlsInput(string strNombreTransaccion, string strNombreMensaje)
        {
            XmlNode xmlNodoCabecera;
            XmlNode xmlNodoDetalle;

            xmlNodoCabecera = xmlDom.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmlDom.AppendChild(xmlNodoCabecera);

            xmlNodoDetalle = xmlDom.CreateElement("", "MBX" + strNombreTransaccion, "");
            xmlDom.AppendChild(xmlNodoDetalle);

            _dtMensajes = ObtenerMensajesPorNombre(strNombreMensaje, strNombreTransaccion);
            foreach (DataRow drMensaje in _dtMensajes.Rows)
            {
                _dtCampos = ObtenerCampos((int)drMensaje["intMensajeID"]);

                XmlNode xmlNodoMensaje;
                xmlNodoMensaje = xmlDom.CreateElement("", drMensaje["strNombre"].ToString(), "");

                xmlNodoDetalle.AppendChild(xmlNodoMensaje);

                foreach (DataRow drCampo in _dtCampos.Rows)
                {
                    if (string.Compare((string)drCampo["strNombrePadre"], (string)drCampo["strNombre"]) == 0)
                    //if (drCampo["strNombrePadre"].ToString().Equals(drCampo["strNombre"].ToString()))
                    {
                        XmlElement xmlNodoPadre;
                        xmlNodoPadre = xmlDom.CreateElement("", drCampo["strNombre"].ToString(), "");
                        xmlNodoMensaje.AppendChild(xmlNodoPadre);
                        AgregarNodoControlsInput(ref xmlNodoPadre, _dtCampos);
                    }

                }
            }
            //xmlDom.Save(System.Configuration.ConfigurationSettings.AppSettings.Get("XMLPATH") + "BFPCtrl" + strNombreTransaccion + strNombreMensaje + ".xml");
            xmlDom.Save(string.Format("{0}MBXCtrl{1}{2}.xml", rutaXml, strNombreTransaccion, strNombreMensaje));

            return xmlDom;
        }

        void AgregarNodoControlsInput(ref XmlElement xmlNode, DataTable dtCampos)
        {
            foreach (DataRow drCampo in dtCampos.Rows)
            {
                if (drCampo["strDefaultValue"].ToString() == "")
                {
                    if (string.Compare((string)drCampo["strNombrePadre"], xmlNode.Name) == 0 && string.Compare((string)drCampo["strNombrePadre"], (string)drCampo["strNombre"]) != 0)
                    //if (drCampo["strNombrePadre"].ToString().Equals(xmlNode.Name) && (!drCampo["strNombrePadre"].ToString().Equals(drCampo["strNombre"].ToString())))
                    {
                        XmlElement xmlNodoHijo;

                        xmlNodoHijo = xmlDom.CreateElement("", drCampo["strNombre"].ToString(), "");
                        if ((bool)drCampo["blnEsHoja"])
                        {
                            xmlNodoHijo.SetAttribute("Type", drCampo["strTipo"].ToString());
                            xmlNodoHijo.SetAttribute("Length", drCampo["intLongitud"].ToString());
                            xmlNodoHijo.SetAttribute("Default", drCampo["strDefaultValue"].ToString());
                            xmlNodoHijo.SetAttribute("Decimal", drCampo["intDecimales"].ToString());
                            xmlNodoHijo.SetAttribute("Description", drCampo["strDescripcion"].ToString());
                            xmlNodoHijo.SetAttribute("Values", "");
                            xmlNodoHijo.SetAttribute("Control", "TextBox");
                        }
                        else
                        {
                            xmlNodoHijo.SetAttribute("Length", drCampo["intLongitud"].ToString());
                            xmlNodoHijo.SetAttribute("Size", drCampo["intTamano"].ToString());
                        }

                        xmlNode.AppendChild(xmlNodoHijo);
                        AgregarNodo(ref xmlNodoHijo, dtCampos);
                    }
                }
            }
        }

        void AgregarNodo(ref XmlElement xmlNode, DataTable dtCampos)
        {
            foreach (DataRow drCampo in dtCampos.Rows)
            {
                if (string.Compare((string)drCampo["strNombrePadre"], xmlNode.Name) == 0 && string.Compare((string)drCampo["strNombrePadre"], (string)drCampo["strNombre"]) != 0)
                //if (drCampo["strNombrePadre"].ToString().Equals(xmlNode.Name) && (!drCampo["strNombrePadre"].ToString().Equals(drCampo["strNombre"].ToString())))
                {
                    XmlElement xmlNodoHijo;

                    xmlNodoHijo = xmlDom.CreateElement("", drCampo["strNombre"].ToString(), "");
                    if ((bool)drCampo["blnEsHoja"])
                    {
                        xmlNodoHijo.SetAttribute("Type", drCampo["strTipo"].ToString());
                        xmlNodoHijo.SetAttribute("Length", drCampo["intLongitud"].ToString());
                        xmlNodoHijo.SetAttribute("Default", drCampo["strDefaultValue"].ToString());
                        xmlNodoHijo.SetAttribute("Decimal", drCampo["intDecimales"].ToString());
                    }
                    else
                    {
                        xmlNodoHijo.SetAttribute("Length", drCampo["intLongitud"].ToString());
                        xmlNodoHijo.SetAttribute("Size", drCampo["intTamano"].ToString());
                    }

                    xmlNode.AppendChild(xmlNodoHijo);
                    AgregarNodo(ref xmlNodoHijo, dtCampos);
                }
            }
        }
    }
}
