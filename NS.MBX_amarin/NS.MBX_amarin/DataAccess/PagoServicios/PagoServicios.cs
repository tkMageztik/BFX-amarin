using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NS.MBX_amarin.DataAccess
{
    public class PagoServicios
    {
        public DataSet ObtenerTransaccionesServicio(int intServicio, int intOperacion, string strNumeroTrama)
        {
            string sp_nombre = "proc_ObtenerTransaccionesServicio";
            DataSet dsData = new DataSet();
            try
            {
                //using (SqlConnection conn = new SqlConnection(this.dbHomeBanking))
                //{
                //    SqlCommand cmd = new SqlCommand();
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandText = sp_nombre;
                //    cmd.Connection = conn;
                //    cmd.Parameters.Add("@intServicio", SqlDbType.Int);
                //    cmd.Parameters["@intServicio"].Value = intServicio;

                //    cmd.Parameters.Add("@intOperacion", SqlDbType.Int);
                //    cmd.Parameters["@intOperacion"].Value = intOperacion;

                //    cmd.Parameters.Add("@strNemonicoTransaccion", SqlDbType.VarChar, 10);
                //    cmd.Parameters["@strNemonicoTransaccion"].Value = strNumeroTrama;

                //    try
                //    {
                //        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                //        {
                //            da.Fill(dsData);
                //            return dsData;
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        return null;
                //    }
                //    finally
                //    {
                //        cmd = null;
                //        dsData = null;
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
                dsData = null;
            }
            //return base.dbHomeBanking.ExecuteDataSet("proc_ObtenerTransaccionesServicio", intServicio, intOperacion, strNumeroTrama);
        }

        public DataSet ObtenerTransaccionesServicio(int intServicio, int intOperacion)
        {

            string sp_nombre = "proc_ObtenerTransaccionesServicio";
            DataSet dsData = new DataSet();
            try
            {
                //using (SqlConnection conn = new SqlConnection(this.dbHomeBanking))
                //{
                //    SqlCommand cmd = new SqlCommand();
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandText = sp_nombre;
                //    cmd.Connection = conn;
                //    cmd.Parameters.Add("@intServicio", SqlDbType.Int);
                //    cmd.Parameters["@intServicio"].Value = intServicio;

                //    cmd.Parameters.Add("@intOperacion", SqlDbType.Int);
                //    cmd.Parameters["@intOperacion"].Value = intOperacion;

                //    cmd.Parameters.Add("@strNemonicoTransaccion", SqlDbType.VarChar, 10);
                //    cmd.Parameters["@strNemonicoTransaccion"].Value = System.DBNull.Value;

                //    try
                //    {
                //        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                //        {
                //            da.Fill(dsData);
                //            return dsData;
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        return null;
                //    }
                //    finally
                //    {
                //        cmd = null;
                //        dsData = null;
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
                dsData = null;
            }
            //return base.dbHomeBanking.ExecuteDataSet("proc_ObtenerTransaccionesServicio", intServicio, intOperacion, null);
        }

        public DataSet ObtenerProgramaEjecutorServicio(int intServicio)
        {
            string sp_nombre = "proc_ProgramaEjecutadorTramaServicio";
            DataSet dsData = new DataSet();
            try
            {
                //using (SqlConnection conn = new SqlConnection(this.dbHomeBanking))
                //{
                //    SqlCommand cmd = new SqlCommand();
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandText = sp_nombre;
                //    cmd.Connection = conn;
                //    cmd.Parameters.Add("@CodigoServicio", SqlDbType.Int);
                //    cmd.Parameters["@CodigoServicio"].Value = intServicio;

                //    try
                //    {
                //        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                //        {
                //            da.Fill(dsData);
                //            return dsData;
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        return null;
                //    }
                //    finally
                //    {
                //        cmd = null;
                //        dsData = null;
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
                dsData = null;
            }

            //return base.dbHomeBanking.ExecuteDataSet("proc_ProgramaEjecutadorTramaServicio", intServicio);
        }

        public DataSet ObtenerProgramaEjecutorServicio()
        {

            string sp_nombre = "proc_ProgramaEjecutadorTramaServicio";
            DataSet dsData = new DataSet();
            try
            {
                //using (SqlConnection conn = new SqlConnection(this.dbHomeBanking))
                //{
                //    SqlCommand cmd = new SqlCommand();
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandText = sp_nombre;
                //    cmd.Connection = conn;
                //    try
                //    {
                //        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                //        {
                //            da.Fill(dsData);
                //            return dsData;
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        return null;
                //    }
                //    finally
                //    {
                //        cmd = null;
                //        dsData = null;
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
                dsData = null;
            }

            //return base.dbHomeBanking.ExecuteDataSet("proc_ProgramaEjecutadorTramaServicio");
        }
    }
}
