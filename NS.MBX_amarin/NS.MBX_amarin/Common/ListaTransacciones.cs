using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Common
{
    public class ListaTransacciones
    {
        public static int PosicionInicialCorte() { return 36; }
        public static short LongitudCabecera() { return 82; }
        public static string NombreMensajeIn() { return "InDat"; }
        public static string NombreMensajeOut() { return "OutDat"; }

        public static string ConsultaCuentasAsociadas = "7002";
        public static string ConsultasCuentasAhorroCCPLZ = "7004";

        public static string VALIDAR_COORDENADAS_HB() { return "7100"; }
        public static string TIPO_CAMBIO() { return "3007"; }
        public static string TipoCambioPreferencia = "7022";

        public static string TrasferenciaValidaCuentas = "7015";
        public static string TrasferenciaConsultaGastos = "7016";
        public static string TrasferenciaOtroBancoConsultaGastos = "7017";
        public static string TrasferenciaEjecuta = "7025";
        public static string TrasferenciaEjecutaOtroBanco = "7005";
    }
}
