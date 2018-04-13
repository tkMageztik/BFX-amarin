using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NS.MBX_amarin.DataAccess
{
    public class Parser : MasterDataAccess
    {
        public IDataReader ObtenerMensajesPorFormulario(string strNombreFormulario)
        {
            //System.Data.IDataReader reader = null;
            //SqlConnection conn = new SqlConnection(this.dbHomeBanking);

            //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            //try
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "proc_Obtener_Mensajes_por_formulario";
            //    cmd.Parameters.Add("@strNombreFormulario", SqlDbType.VarChar, 200);
            //    cmd.Parameters["@strNombreFormulario"].Value = strNombreFormulario;
            //    cmd.Connection = conn;
            //    conn.Open();
            //    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            //    return reader;
            //}
            //catch
            //{
            //    return null;
            //}
            //finally
            //{
            //    cmd = null;
            //}
            return null;

            //return base.dbHomeBanking.ExecuteReader("proc_Obtener_Mensajes_por_formulario", strNombreFormulario);
        }

        public IDataReader ObtenerMensajePorNombre(string strNombreMensaje, string strNombreTransaccion)
        {
            //System.Data.IDataReader reader = null;
            //SqlConnection conn = new SqlConnection(this.dbHomeBanking);

            //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            //try
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "proc_Obtener_Mensajes_por_Nombre";
            //    cmd.Parameters.Add("@strNombreMensaje", SqlDbType.VarChar, 200);
            //    cmd.Parameters["@strNombreMensaje"].Value = strNombreMensaje;

            //    cmd.Parameters.Add("@strNombreTransaccion", SqlDbType.VarChar, 200);
            //    cmd.Parameters["@strNombreTransaccion"].Value = strNombreTransaccion;

            //    cmd.Connection = conn;
            //    conn.Open();
            //    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            //    return reader;
            //}
            //catch
            //{
            //    return null;
            //}
            //finally
            //{
            //    cmd = null;
            //}
            return null;
            //return base.dbHomeBanking.ExecuteReader("proc_Obtener_Mensajes_por_Nombre", strNombreMensaje, strNombreTransaccion);
        }

        public IDataReader ObtenerCamposPorMensaje(int intMensajeID)
        {
            //System.Data.IDataReader reader = null;
            //SqlConnection conn = new SqlConnection(this.dbHomeBanking);

            //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            //try
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "proc_Obtener_Campos_por_Mensaje";
            //    cmd.Parameters.Add("@intMensajeID", SqlDbType.VarChar, 200);
            //    cmd.Parameters["@intMensajeID"].Value = intMensajeID;

            //    cmd.Connection = conn;
            //    conn.Open();
            //    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            //    return reader;
            //}
            //catch
            //{
            //    return null;
            //}
            //finally
            //{
            //    cmd = null;
            //}
            return null;
            //            return base.dbHomeBanking.ExecuteReader("proc_Obtener_Campos_por_Mensaje", intMensajeID);
        }
    }
}
