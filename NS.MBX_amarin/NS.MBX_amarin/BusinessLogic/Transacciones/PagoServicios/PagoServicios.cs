using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BP = NS.MBX_amarin.Parser;
using DA = NS.MBX_amarin.DataAccess;
using BC = NS.MBX_amarin.Common;

namespace NS.MBX_amarin.BusinessLogic
{
    public class PagoServicios
    {
        public DataSet ObtenerControlesInput(string intServicio, int TipoOperacion)
        {
            TransaccionesMBX tx = new TransaccionesMBX();
            DataSet dsData = new DataSet();
            string Trama = "";
            BP.DataLoader xml = new BP.DataLoader();
            DA.PagoServicios ObjPagoServicio = new DA.PagoServicios();
            try
            {
                //using (DataSet dsServicioTransaccion = ObjPagoServicio.ObtenerTransaccionesServicio(intServicio, TipoOperacion))
                //{
                //    if (dsServicioTransaccion != null && dsServicioTransaccion.Tables[0].Rows.Count == 1)
                //    {
                string strDefaultValuesNombre = ""; //dsServicioTransaccion.Tables[0].Rows[0]["strNombreTransaccion"].ToString();
                string strDefaultValuesNemonico = "";// dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
                if(TipoOperacion == 2)
                {
                    MapearServicioEjecutar(intServicio.Substring(0, 1), intServicio.Substring(1, 1), out strDefaultValuesNemonico, out strDefaultValuesNombre);
                }
                else
                {
                    MapearServicioConsulta(intServicio.Substring(0, 1), intServicio.Substring(1, 1), out strDefaultValuesNemonico, out strDefaultValuesNombre);
                }
                
                using (DataSet dsSalida = xml.ObtenerControlsInput(Trama, BC.ListaTransacciones.NombreMensajeIn(), strDefaultValuesNombre, 0))
                {
                    if (dsSalida != null)
                    {
                        dsData.Tables.Add(TablaInformacionControlesInput());
                        dsData.Tables[0].Rows.Add();
                        dsData.Tables[0].Rows[dsData.Tables[0].Rows.Count - 1]["CtrlTrama"] = strDefaultValuesNemonico;
                        dsData.Tables[0].Rows[dsData.Tables[0].Rows.Count - 1]["CtrlCantidad"] = dsSalida.Tables[0].Columns.Count;
                        string strColumnas = "";
                        foreach (DataColumn item in dsSalida.Tables[0].Columns)
                        {
                            strColumnas += item + ";";
                        }
                        dsData.Tables[0].Rows[dsData.Tables[0].Rows.Count - 1]["CtrlListaCampos"] = strColumnas;
                        dsData.Tables[0].Rows[dsData.Tables[0].Rows.Count - 1]["CtrlDelimitadorCampos"] = ";";
                    }
                    
                }
                //    }
                //}
                return dsData;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                tx = null;
                dsData = null;
            }
        }
        private DataTable TablaInformacionControlesInput()
        {
            DataTable dtData = new DataTable();
            dtData.Columns.Add("CtrlTrama", System.Type.GetType("System.String"));
            dtData.Columns.Add("CtrlCantidad", System.Type.GetType("System.String"));
            dtData.Columns.Add("CtrlListaCampos", System.Type.GetType("System.String"));
            dtData.Columns.Add("CtrlDelimitadorCampos", System.Type.GetType("System.String"));
            return dtData;

        }

        private void MapearServicioConsulta(string codEmpresa, string codServicio, out string strDefaultValuesNemonico, out string strDefaultValuesNombre)
        {
            strDefaultValuesNemonico = "";
            strDefaultValuesNombre = "";
            if (codEmpresa == "0")//claro
            {
                strDefaultValuesNemonico = "7053";
                strDefaultValuesNombre = "7053ICTelefCelClaro1";
            }
            else if(codEmpresa == "1")
            {
                if(codServicio == "0")//celular
                {
                    strDefaultValuesNemonico = "7026";
                    strDefaultValuesNombre = "7026ICTelefCel";
                }
                else
                {
                    strDefaultValuesNemonico = "7026";
                    strDefaultValuesNombre = "7026ICTelFija";
                }
            }
            else if(codEmpresa == "2")
            {
                strDefaultValuesNemonico = "7053";
                strDefaultValuesNombre = "7053ICTelefCelClaro1";
            }
            else if (codEmpresa == "3")//edelnor
            {
                strDefaultValuesNemonico = "7026";
                strDefaultValuesNombre = "7026ICEDELNOR";
            }
            else if (codEmpresa == "4")//luz del sur
            {
                strDefaultValuesNemonico = "7026";
                strDefaultValuesNombre = "7026ICLUZSUR";
            }
            else if (codEmpresa == "5")//sedapal
            {
                strDefaultValuesNemonico = "7051";
                strDefaultValuesNombre = "7051ICSEDAPAL";
            }
        }

        private void MapearServicioEjecutar(string codEmpresa, string codServicio, out string strDefaultValuesNemonico, out string strDefaultValuesNombre)
        {
            strDefaultValuesNemonico = "";
            strDefaultValuesNombre = "";
            if (codEmpresa == "0")//claro
            {
                strDefaultValuesNemonico = "7054";
                strDefaultValuesNombre = "7054IPTelefClaro";
            }
            else if (codEmpresa == "1")
            {
                if (codServicio == "0")//celular
                {
                    strDefaultValuesNemonico = "7031";
                    strDefaultValuesNombre = "7031ICTELFCEL";
                }
                else
                {
                    strDefaultValuesNemonico = "7031";
                    strDefaultValuesNombre = "7031ICTelefFija";
                }
            }
            else if (codEmpresa == "2")//entel
            {
                strDefaultValuesNemonico = "7054";
                strDefaultValuesNombre = "7054IPTelefClaro";
            }
            else if (codEmpresa == "3")//edelnor
            {
                strDefaultValuesNemonico = "7031";
                strDefaultValuesNombre = "7031ICEDELNOR";
            }
            else if (codEmpresa == "4")//luz del sur
            {
                strDefaultValuesNemonico = "7031";
                strDefaultValuesNombre = "7031ICLUZ";
            }
            else if (codEmpresa == "5")//sedapal
            {
                strDefaultValuesNemonico = "7051";
                strDefaultValuesNombre = "7051IPSEDAPAL";
            }
        }

        public DataSet ObtenerConsultaTelefono(short intLongitudTrama, string intServicio, out string _strError, params object[] parameterValues)
        {

            DataSet dsData = new DataSet();
            string strMensajeError = "";
            string Trama = "";
            try
            {
                for (int i = 0; i < parameterValues.Length; i++)
                    Trama += parameterValues[i].ToString();
                BP.DataLoader xml = new BP.DataLoader();
                DA.PagoServicios ObjPagoServicio = new DA.PagoServicios();
                string strTramaGenerada = "";
                //using (DataSet dsServicioTransaccion = ObjPagoServicio.ObtenerTransaccionesServicio(intServicio, 1))
                //{
                //    if (dsServicioTransaccion != null && dsServicioTransaccion.Tables[0].Rows.Count == 1)
                //    {
                string strDefaultValuesNemonico = "";
                string strDefaultValuesNombre = "";
                MapearServicioConsulta(intServicio.Substring(0, 1), intServicio.Substring(1, 1), out strDefaultValuesNemonico, out strDefaultValuesNombre);
                //string strDefaultValuesNemonico = ""; dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
                //string strDefaultValuesNombre = ""; dsServicioTransaccion.Tables[0].Rows[0]["strNombreTransaccion"].ToString();
                string strProgramaEjecutador = "HomeBanking";// fdsServicioTransaccion.Tables[0].Rows[0]["strProgramaEjecutador"].ToString();
                string strNombreEmpresa = "";// dsServicioTransaccion.Tables[0].Rows[0]["strNombreEmpresa"].ToString();
                        using (DataSet dsSalida = xml.ObtenerInputTrama(Trama, BC.ListaTransacciones.NombreMensajeIn(), strDefaultValuesNombre, 0))
                        {
                            if (dsSalida != null && dsSalida.Tables[0].Rows.Count == 1)
                            {
                                foreach (DataRow item in dsSalida.Tables[0].Rows)
                                {
                                    foreach (DataColumn item1 in dsSalida.Tables[0].Columns)
                                        strTramaGenerada += item[item1.ColumnName].ToString();
                                }
                                if (strProgramaEjecutador == "HomeBanking")
                                {
                                    TransaccionesMBX tx = new TransaccionesMBX();
                                    using (DataSet dsDataTrama = tx.EjecutarTransaccion(strDefaultValuesNemonico, intLongitudTrama, strTramaGenerada, BC.ListaTransacciones.NombreMensajeOut(), BC.ListaTransacciones.PosicionInicialCorte(), out strMensajeError))
                                    {
                                        if (dsDataTrama != null && strMensajeError == "0000")
                                        {
                                            dsData = dsDataTrama;
                                        }
                                        else
                                        {
                                            dsData = null;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                strMensajeError = "Aun no ha sido creado el XML  de entrada";
                                dsData = null;
                            }
                        }
                //    }
                //    else
                //    {
                //        strMensajeError = "Aun no ha registrado el XML de entrada";
                //        dsData = null;
                //    }
                //}
                _strError = strMensajeError;

                return dsData;
            }
            catch (Exception ex)
            {
                _strError = ex.Message;
                return null;
            }
            finally
            {
                dsData = null;
            }
        }

        public DataSet ObtenerProgramaEjecutorServicio(int intServicio)
        {
            DataSet dsData = new DataSet();
            DA.PagoServicios ObjPagoServicioDA = new DA.PagoServicios();
            try
            {
                using (DataSet ds = ObjPagoServicioDA.ObtenerProgramaEjecutorServicio(intServicio))
                {
                    dsData = ds;
                }
                return dsData;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dsData = null;
            }

        }

        public DataSet ObtenerProgramaEjecutorServicio()
        {
            DataSet dsData = new DataSet();
            DA.PagoServicios ObjPagoServicioDA = new DA.PagoServicios();
            try
            {
                using (DataSet ds = ObjPagoServicioDA.ObtenerProgramaEjecutorServicio())
                {
                    dsData = ds;
                }
                return dsData;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dsData = null;
            }

        }

        private DataTable GetDtDetalle()
        {
            DataTable dtDetalle = new DataTable();
            dtDetalle.Columns.Add("IdItem", System.Type.GetType("System.String"));
            dtDetalle.Columns.Add("NumRec", System.Type.GetType("System.String"));
            dtDetalle.Columns.Add("Moneda", System.Type.GetType("System.String"));
            dtDetalle.Columns.Add("ValPag", System.Type.GetType("System.Decimal"));
            dtDetalle.Columns.Add("ValCom", System.Type.GetType("System.Decimal"));
            dtDetalle.Columns.Add("FecEmi", System.Type.GetType("System.String"));
            dtDetalle.Columns.Add("FecLim", System.Type.GetType("System.String"));
            dtDetalle.Columns.Add("NomAbo_i", System.Type.GetType("System.String"));
            dtDetalle.Columns.Add("ContraPartida", System.Type.GetType("System.String"));
            dtDetalle.Columns.Add("IdSobre", System.Type.GetType("System.String"));
            dtDetalle.Columns.Add("Secuencial", System.Type.GetType("System.String"));
            dtDetalle.Columns.Add("NomContrapartida", System.Type.GetType("System.String"));

            return dtDetalle;

        }

        private DataTable GetDtSubDetalle()
        {
            DataTable dtSubDetalle = new DataTable();
            dtSubDetalle.Columns.Add("IdSubItem", System.Type.GetType("System.String"));
            dtSubDetalle.Columns.Add("IdSobre", System.Type.GetType("System.String"));
            dtSubDetalle.Columns.Add("CodSubItem", System.Type.GetType("System.String"));
            dtSubDetalle.Columns.Add("TipSubItem", System.Type.GetType("System.String"));
            dtSubDetalle.Columns.Add("Referencia", System.Type.GetType("System.String"));
            dtSubDetalle.Columns.Add("Valor", System.Type.GetType("System.String"));
            dtSubDetalle.Columns.Add("Porcentaje", System.Type.GetType("System.String"));
            dtSubDetalle.Columns.Add("ValNeto", System.Type.GetType("System.String"));
            dtSubDetalle.Columns.Add("FechaRefer", System.Type.GetType("System.String"));

            return dtSubDetalle;
        }

        public DataSet EjecutarTransaccion(short intLongitudTrama, string intServicio, out string _strError, out string _strTrama, out DataSet _dsHeader, params object[] parameterValues)
        {

            DataSet dsData = new DataSet();
            DataSet dsHeader = new DataSet();
            string strMensajeError = "";
            string Trama = "";
            string strTramaGenerada = "";
            try
            {
                for (int i = 0; i < parameterValues.Length; i++)
                    Trama += parameterValues[i].ToString();
                BP.DataLoader xml = new BP.DataLoader();
                DA.PagoServicios ObjPagoServicio = new DA.PagoServicios();
                //using (DataSet dsServicioTransaccion = ObjPagoServicio.ObtenerTransaccionesServicio(intServicio, 2))
                //{
                //    if (dsServicioTransaccion != null && dsServicioTransaccion.Tables[0].Rows.Count == 1)
                //    {
                string strDefaultValuesNemonico = "";
                string strDefaultValuesNombre = "";
                MapearServicioEjecutar(intServicio.Substring(0, 1), intServicio.Substring(1, 1), out strDefaultValuesNemonico, out strDefaultValuesNombre);
                //string strDefaultValuesNemonico = ""; dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
                //string strDefaultValuesNombre = ""; dsServicioTransaccion.Tables[0].Rows[0]["strNombreTransaccion"].ToString();
                string strProgramaEjecutador = "HomeBanking";// fdsServicioTransaccion.Tables[0].Rows[0]["strProgramaEjecutador"].ToString();
                string strNombreEmpresa = "";
                string strDefaultValues = strDefaultValuesNemonico;// dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
                strTramaGenerada = "";
                //string strDefaultValuesNemonico = dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
                        //string strDefaultValuesNombre = dsServicioTransaccion.Tables[0].Rows[0]["strNombreTransaccion"].ToString();
                        //string strProgramaEjecutador = dsServicioTransaccion.Tables[0].Rows[0]["strProgramaEjecutador"].ToString();
                        //string strNombreEmpresa = dsServicioTransaccion.Tables[0].Rows[0]["strNombreEmpresa"].ToString();
                        //using (DataSet dsSalida = xml.ObtenerInputTrama(Trama, BC.ListaTransacciones.NombreMensajeIn(), strDefaultValuesNombre, 0))
                        //{
                        //    if (dsSalida != null && dsSalida.Tables[0].Rows.Count == 1)
                        //    {
                        //        foreach (DataRow item in dsSalida.Tables[0].Rows)
                        //        {
                        //            foreach (DataColumn item1 in dsSalida.Tables[0].Columns)
                        //                strTramaGenerada += item[item1.ColumnName].ToString();
                        //        }
                                if (strProgramaEjecutador == "HomeBanking")
                                {
                                    TransaccionesMBX tx = new TransaccionesMBX();
                                    using (DataSet dsDataTrama = tx.EjecutarTransaccion(strDefaultValues, intLongitudTrama, strTramaGenerada, BC.ListaTransacciones.NombreMensajeOut(), BC.ListaTransacciones.PosicionInicialCorte(), out strMensajeError))
                                    {
                                        if (dsDataTrama != null && strMensajeError == "0000")
                                        {
                                            dsHeader = tx.ObtenerCabecera(strDefaultValues, BC.ListaTransacciones.NombreMensajeOut(), 0);
                                            dsData = dsDataTrama;
                                        }
                                        else
                                            dsData = null;
                                    }
                                }
                                else
                                {
                                    if (strProgramaEjecutador == "Cash")
                                    {
                                        //Cash tx = new Cash();
                                        //string strSalida = tx.EjecutarTransaccion(BCC.DefaultValues.CM_pago(), out _strError, strTramaGenerada.Split('*') );
                                        //if (_strError == "0001")
                                        //{
                                        //    string[] strValores = strSalida.Split(';');  // strSalida
                                        //    //for (int i = 0; i < length; i++)
                                        //    //{

                                        //    //}
                                        //}
                                    }
                                }

                        //    }
                        //    else
                        //    {
                        //        strMensajeError = "Aun no ha sido creado el XML  de entrada";
                        //        dsData = null;
                        //    }
                        //}
                    //}
                    //else
                    //{
                    //    strMensajeError = "Aun no ha registrado el XML de entrada";
                    //    dsData = null;
                    //}
                //}
                _dsHeader = dsHeader;
                _strError = strMensajeError;
                _strTrama = strTramaGenerada;
                return dsData;
            }
            catch (Exception ex)
            {
                _dsHeader = null;
                _strTrama = Trama;
                _strError = ex.Message;
                return null;
            }
            finally
            {
                dsData = null;
                dsHeader = null;
            }
        }

        public DataSet EmularData(string strNombreMensajeOut, string strNombreTransaccion, int intPosicionInicialLecturaOut, int intCantidadData)
        {
            TransaccionesMBX tx = new TransaccionesMBX();
            using (DataSet dsData = tx.EmularData(strNombreMensajeOut, strNombreTransaccion, intPosicionInicialLecturaOut, intCantidadData))
            {
                return dsData;
            }
        }

        public DataSet ConsultarTransaccion(short intLongitudTrama, string intServicio, out string _strError, params object[] parameterValues)
        {

            DataSet dsData = new DataSet();
            string strMensajeError = "";
            string Trama = "";
            try
            {
                for (int i = 0; i < parameterValues.Length; i++)
                    Trama += parameterValues[i].ToString();
                BP.DataLoader xml = new BP.DataLoader();
                DA.PagoServicios ObjPagoServicio = new DA.PagoServicios();
                string strTramaGenerada = "";
                //using (DataSet dsServicioTransaccion = ObjPagoServicio.ObtenerTransaccionesServicio(intServicio, 1))
                //{
                //    if (dsServicioTransaccion != null && dsServicioTransaccion.Tables[0].Rows.Count == 1)
                //    {
                string strDefaultValuesNemonico = "";
                string strDefaultValuesNombre = "";
                MapearServicioConsulta(intServicio.Substring(0, 1), intServicio.Substring(1, 1), out strDefaultValuesNemonico, out strDefaultValuesNombre);
                //string strDefaultValuesNemonico = ""; dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
                //string strDefaultValuesNombre = ""; dsServicioTransaccion.Tables[0].Rows[0]["strNombreTransaccion"].ToString();
                string strProgramaEjecutador = "HomeBanking";// fdsServicioTransaccion.Tables[0].Rows[0]["strProgramaEjecutador"].ToString();
                string strNombreEmpresa = "";
                string strDefaultValues = strDefaultValuesNemonico;

                //string strDefaultValuesNemonico = dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
                //        string strDefaultValuesNombre = dsServicioTransaccion.Tables[0].Rows[0]["strNombreTransaccion"].ToString();
                //        string strProgramaEjecutador = dsServicioTransaccion.Tables[0].Rows[0]["strProgramaEjecutador"].ToString();
                //        string strNombreEmpresa = dsServicioTransaccion.Tables[0].Rows[0]["strNombreEmpresa"].ToString();

                        //using (DataSet dsSalida = xml.ObtenerInputTrama(Trama, BC.ListaTransacciones.NombreMensajeIn(), strDefaultValuesNombre, 0))
                        //{
                        //    if (dsSalida != null && dsSalida.Tables[0].Rows.Count == 1)
                        //    {
                        //        foreach (DataRow item in dsSalida.Tables[0].Rows)
                        //        {
                        //            foreach (DataColumn item1 in dsSalida.Tables[0].Columns)
                        //                strTramaGenerada += item[item1.ColumnName].ToString();
                        //        }
                                if (strProgramaEjecutador == "HomeBanking")
                                {
                                    TransaccionesMBX tx = new TransaccionesMBX();
                                    using (DataSet dsDataTrama = tx.EjecutarTransaccion(strDefaultValuesNemonico, intLongitudTrama, strTramaGenerada, BC.ListaTransacciones.NombreMensajeOut(), BC.ListaTransacciones.PosicionInicialCorte(), out strMensajeError))
                                    {
                                        if (dsDataTrama != null && strMensajeError == "0000")
                                            dsData = dsDataTrama;
                                        else
                                            dsData = null;
                                    }
                                }
                                //else
                                //{
                                //    if (strProgramaEjecutador == "Cash")
                                //    {
                                //        Cash tx = new Cash();
                                //        DataTable dtDetalle;
                                //        DataTable dtSubDetalle;
                                //        string strDataTrama = tx.EjecutarTransaccion(BCC.DefaultValues.CM_Consulta(), out strMensajeError, strTramaGenerada.Split('*'));
                                //        if (strDataTrama != null && strMensajeError == "0000")
                                //        {
                                //            string[] strElementos = strDataTrama.Split(';');
                                //            string strCanal = strElementos[0].Trim();
                                //            string strEmpresa = strElementos[1].Trim();
                                //            string strCodigoServ = strElementos[2].Trim();
                                //            string strCodigoret = strElementos[3].Trim();
                                //            string strNombreAbonado = strElementos[4].Trim();
                                //            int intCantidad = int.Parse(strElementos[5].Trim());
                                //            if (intCantidad > 10) intCantidad = 10;
                                //            int intLastPosition = 6;
                                //            DataRow drDetalle;
                                //            dtDetalle = GetDtDetalle();
                                //            for (int i = 0; i < intCantidad; i++)
                                //            {
                                //                drDetalle = dtDetalle.NewRow();
                                //                drDetalle["IdItem"] = strElementos[intLastPosition++].Trim();
                                //                drDetalle["NumRec"] = strElementos[intLastPosition++].Trim();
                                //                drDetalle["Moneda"] = strElementos[intLastPosition++].Trim();
                                //                drDetalle["ValPag"] = decimal.Parse(strElementos[intLastPosition++].Trim());
                                //                drDetalle["ValCom"] = decimal.Parse(strElementos[intLastPosition++].Trim());
                                //                drDetalle["FecEmi"] = strElementos[intLastPosition++].Trim();
                                //                drDetalle["FecLim"] = strElementos[intLastPosition++].Trim();
                                //                drDetalle["NomAbo_i"] = strElementos[intLastPosition++].Trim();
                                //                drDetalle["ContraPartida"] = strElementos[intLastPosition++].Trim();
                                //                drDetalle["IdSobre"] = strElementos[intLastPosition++].Trim();
                                //                drDetalle["Secuencial"] = strElementos[intLastPosition++].Trim();
                                //                drDetalle["NomContrapartida"] = strElementos[intLastPosition++].Trim();
                                //                dtDetalle.Rows.Add(drDetalle);
                                //            }
                                //            int intCantidadSubItems = int.Parse(strElementos[intLastPosition++].Trim());
                                //            if (intCantidadSubItems > 30)
                                //            {
                                //                intCantidadSubItems = 30;
                                //            }
                                //            DataRow drSubDetalle;
                                //            dtSubDetalle = GetDtSubDetalle();
                                //            for (int i = 0; i < intCantidadSubItems; i++)
                                //            {
                                //                drSubDetalle = dtSubDetalle.NewRow();
                                //                drSubDetalle["IdSubItem"] = strElementos[intLastPosition++].Trim();
                                //                drSubDetalle["IdSobre"] = strElementos[intLastPosition++].Trim();
                                //                drSubDetalle["CodSubItem"] = strElementos[intLastPosition++].Trim();
                                //                drSubDetalle["TipSubItem"] = strElementos[intLastPosition++].Trim();
                                //                drSubDetalle["Referencia"] = strElementos[intLastPosition++].Trim();
                                //                drSubDetalle["Valor"] = strElementos[intLastPosition++].Trim();
                                //                drSubDetalle["Porcentaje"] = strElementos[intLastPosition++].Trim();
                                //                drSubDetalle["ValNeto"] = strElementos[intLastPosition++].Trim();
                                //                drSubDetalle["FechaRefer"] = strElementos[intLastPosition].Trim().Length >= 10 ? strElementos[intLastPosition++].Trim().Substring(0, 10) : strElementos[intLastPosition++].Trim();
                                //                dtSubDetalle.Rows.Add(drSubDetalle);
                                //            }
                                //            dsData.Tables.Add(dtDetalle);
                                //            dsData.Tables.Add(dtSubDetalle);
                                //        }
                                //        else
                                //        {
                                //            dsData = null;
                                //        }
                                //    }
                                //}

                            //}
                            //else
                            //{
                            //    strMensajeError = "Aun no ha sido creado el XML  de entrada";
                            //    dsData = null;
                            //}
                    //    }
                    //}
                    //else
                    //{
                    //    strMensajeError = "Aun no ha registrado el XML de entrada";
                    //    dsData = null;
                    //}
                //}
                _strError = strMensajeError;
                return dsData;
            }
            catch (Exception ex)
            {
                _strError = ex.Message;
                return null;
            }
            finally
            {
                dsData = null;
            }
        }

        //public string ConsultarTransaccionCash(short intLongitudTrama, int intServicio, out string _strError, params object[] parameterValues)
        //{

        //    string strData = string.Empty;
        //    string strMensajeError = "";
        //    string Trama = "";
        //    string strTramaGenerada = "";
        //    try
        //    {
        //        for (int i = 0; i < parameterValues.Length; i++)
        //            Trama += parameterValues[i].ToString();
        //        BP.DataLoader xml = new BP.DataLoader();
        //        DA.PagoServicios ObjPagoServicio = new DA.PagoServicios();
        //        using (DataSet dsServicioTransaccion = ObjPagoServicio.ObtenerTransaccionesServicio(intServicio, 2))
        //        {
        //            if (dsServicioTransaccion != null && dsServicioTransaccion.Tables[0].Rows.Count == 1)
        //            {
        //                string strDefaultValues = dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
        //                string strDefaultValuesNemonico = dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
        //                string strDefaultValuesNombre = dsServicioTransaccion.Tables[0].Rows[0]["strNombreTransaccion"].ToString();
        //                string strProgramaEjecutador = dsServicioTransaccion.Tables[0].Rows[0]["strProgramaEjecutador"].ToString();
        //                string strNombreEmpresa = dsServicioTransaccion.Tables[0].Rows[0]["strNombreEmpresa"].ToString();

        //                using (DataSet dsSalida = xml.ObtenerInputTrama(Trama, BC.DefaultValues.NombreMensajeIn(), strDefaultValues, 0))
        //                {
        //                    if (dsSalida != null && dsSalida.Tables[0].Rows.Count == 1)
        //                    {
        //                        foreach (DataRow item in dsSalida.Tables[0].Rows)
        //                        {
        //                            foreach (DataColumn item1 in dsSalida.Tables[0].Columns)
        //                                strTramaGenerada += item[item1.ColumnName].ToString();
        //                        }

        //                        if (strProgramaEjecutador == "Cash")
        //                        {
        //                            Cash tx = new Cash();
        //                            string strDataTrama = tx.EjecutarTransaccion(BCC.DefaultValues.CM_Consulta(), out strMensajeError, strTramaGenerada);
        //                            if (strDataTrama != null && strMensajeError == "0000")
        //                            {
        //                                strData = strDataTrama;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        strMensajeError = "Aun no ha sido creado el XML  de entrada";
        //                        strData = string.Empty;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                strMensajeError = "Aun no ha registrado el XML de entrada";
        //                strData = string.Empty;
        //            }
        //        }
        //        _strError = strMensajeError;
        //        return strData;
        //    }
        //    catch (Exception ex)
        //    {
        //        _strError = ex.Message;
        //        return strData;
        //    }
        //}

        //public string EjecutarTransaccionCash(short intLongitudTrama, int intServicio, out string _strError, out string _strTrama, params object[] parameterValues)
        //{
        //    Cash tx = new Cash();
        //    string strData = string.Empty;
        //    string strMensajeError = "";
        //    string Trama = "";
        //    string strTramaGenerada = "";
        //    try
        //    {
        //        for (int i = 0; i < parameterValues.Length; i++)
        //            Trama += parameterValues[i].ToString();
        //        BP.DataLoader xml = new BP.DataLoader();
        //        DA.PagoServicios ObjPagoServicio = new DA.PagoServicios();
        //        using (DataSet dsServicioTransaccion = ObjPagoServicio.ObtenerTransaccionesServicio(intServicio, 2))
        //        {
        //            if (dsServicioTransaccion != null && dsServicioTransaccion.Tables[0].Rows.Count == 1)
        //            {
        //                string strDefaultValues = dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
        //                string strDefaultValuesNemonico = dsServicioTransaccion.Tables[0].Rows[0]["strNemonicoTransaccion"].ToString();
        //                string strDefaultValuesNombre = dsServicioTransaccion.Tables[0].Rows[0]["strNombreTransaccion"].ToString();
        //                string strProgramaEjecutador = dsServicioTransaccion.Tables[0].Rows[0]["strProgramaEjecutador"].ToString();
        //                string strNombreEmpresa = dsServicioTransaccion.Tables[0].Rows[0]["strNombreEmpresa"].ToString();

        //                using (DataSet dsSalida = xml.ObtenerInputTrama(Trama, BC.DefaultValues.NombreMensajeIn(), strDefaultValues, 0))
        //                {
        //                    if (dsSalida != null && dsSalida.Tables[0].Rows.Count == 1)
        //                    {
        //                        foreach (DataRow item in dsSalida.Tables[0].Rows)
        //                        {
        //                            foreach (DataColumn item1 in dsSalida.Tables[0].Columns)
        //                                strTramaGenerada += item[item1.ColumnName].ToString();
        //                        }
        //                        string dsDataTrama = tx.EjecutarTransaccion(BCC.DefaultValues.CM_pago(), out strMensajeError, strTramaGenerada);
        //                        if (dsDataTrama != null && strMensajeError == "0000")
        //                        {
        //                            strData = dsDataTrama;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        strMensajeError = "Aun no ha sido creado el XML  de entrada";
        //                        strData = string.Empty;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                strMensajeError = "Aun no ha registrado el XML de entrada";
        //                strData = string.Empty;
        //            }
        //        }
        //        _strError = strMensajeError;
        //        _strTrama = strTramaGenerada;
        //        return strData;
        //    }
        //    catch (Exception ex)
        //    {
        //        _strTrama = Trama;
        //        _strError = ex.Message;
        //        return string.Empty;
        //    }
        //}
        

    }
}
