using NS.MBX_amarin.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NS.MBX_amarin.BusinessLogic.Transacciones
{
    public class Comunes
    {
        public DataSet ObtenerCuentasAsociadas(short intLongitud, out string _strError, params object[] ParmeterValues)
        {
            TransaccionesMBX tx = new TransaccionesMBX();
            string strTrama = "";
            try
            {

                for (int i = 0; i < ParmeterValues.Length; i++)
                {
                    strTrama += ParmeterValues[i].ToString();
                }

                using (DataSet dsData = tx.EjecutarTransaccion(ListaTransacciones.ConsultaCuentasAsociadas, intLongitud, strTrama, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError))
                {
                    if (dsData != null && _strError == "0000")
                    {
                        using (DataSet dsDataTotal = ObtenerCuentasPersonales(118, out _strError, dsData.Tables[1].Rows[0]["ODDcodcl"].ToString().PadLeft(10, '0'), dsData.Tables[1].Rows[0]["ODNrTarj"].ToString().PadRight(19, ' ')))
                        {
                            DataSet dstmp_return = new DataSet();
                            dstmp_return = dsDataTotal.Clone();

                            foreach (DataRow item2 in dsDataTotal.Tables[0].Rows)
                            {
                                foreach (DataRow item1 in dsData.Tables[0].Rows)
                                {
                                    if (item1["ODDcodct"].ToString().PadLeft(19, '0') == item2["ODcodct"].ToString().PadLeft(19, '0'))
                                    {
                                        dstmp_return.Tables[0].ImportRow(item2);
                                    }
                                }
                            }

                            dstmp_return.Tables[0].Columns.Add("ODDmoned");
                            foreach (DataRow item2 in dstmp_return.Tables[0].Rows)
                            {
                                foreach (DataRow item1 in dsData.Tables[0].Rows)
                                {
                                    if (item1["ODDcodct"].ToString().PadLeft(19, '0') == item2["ODcodct"].ToString().PadLeft(19, '0'))
                                    {
                                        item2["ODDmoned"] = item1["ODDmoned"];
                                    }
                                }
                            }

                            DataColumn dcSaldoDisp = new DataColumn("SaldoDisponible", typeof(string));
                            dstmp_return.Tables["ODDctas"].Columns.Add(dcSaldoDisp);
                            foreach (DataRow dr in dstmp_return.Tables["ODDctas"].Rows)
                            {
                                dr["SaldoDisponible"] = double.Parse(dr["ODslddi"].ToString()).ToString("N2");
                            }
                            dstmp_return.Tables["ODDctas"].AcceptChanges();
                            DataColumn dcDescCuenta = new DataColumn("DescCuenta", typeof(string));
                            dcDescCuenta.Expression = "TRIM(ODdescr) + ' - ' + TRIM(ODcodct) + ' (Saldo disp.: ' + TRIM(ODmoned) + ' ' + TRIM(SaldoDisponible) + ')'";
                            dstmp_return.Tables["ODDctas"].Columns.Add(dcDescCuenta);
                            return dstmp_return;
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _strError = ex.Message;
                return null;
            }

        }

        public DataSet ObtenerCuentasPersonales(short intLongitud, out string _strError, params object[] ParmeterValues)
        {
            TransaccionesMBX tx = new TransaccionesMBX();
            DataSet dsData = new DataSet();
            string strTrama = "";
            string __strError = string.Empty;
            int intNumeroRegistro = 24;
            try
            {
                for (int f = 0; f < 100; f++)
                {
                    strTrama = intNumeroRegistro.ToString();
                    for (int i = 0; i < ParmeterValues.Length; i++)
                    {
                        strTrama += ParmeterValues[i].ToString();
                    }
                    strTrama += string.Concat((intNumeroRegistro * f).ToString().PadLeft(4, '0'), "%");
                    //strMensaje = string.Format("99{0}{1}0000%", strCodigoCliente.Trim().PadLeft(10, '0'), strTarjeta.PadRight(19, ' '));
                    using (DataSet dsOut = tx.EjecutarTransaccion(ListaTransacciones.ConsultasCuentasAhorroCCPLZ, intLongitud, strTrama, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError))
                    {
                        if (string.Compare(_strError, "0000") == 0)
                        {
                            __strError = _strError;
                            if (f == 0)
                            {
                                dsData = dsOut.Clone();
                                if (dsOut.Tables.Count > 0)
                                {
                                    foreach (DataRow row in dsOut.Tables[1].Rows)
                                    {
                                        dsData.Tables[1].ImportRow(row);
                                    }
                                }
                            }
                            if (dsOut.Tables[0].Rows.Count == 0)
                            {
                                break;
                            }
                            else
                            {
                                foreach (DataRow row in dsOut.Tables[0].Rows)
                                {
                                    dsData.Tables[0].ImportRow(row);
                                }

                            }

                        }
                        else
                        {
                            __strError = _strError;
                            dsData = null;
                            break;
                        }
                        //return dsOut;
                    }
                }
                if (__strError.ToString().Trim().Length == 4)
                {
                    _strError = __strError;
                    return dsData;
                }
                else { _strError = "6666"; return null; }

            }
            catch (Exception ex)
            {
                _strError = "6666";
                return null;
            }
            finally
            {
                tx = null;
                dsData = null;
                strTrama = string.Empty;
                __strError = string.Empty;
            }
        }
    }
}
