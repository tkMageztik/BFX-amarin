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

        public static string TrasferenciaValidaCuentas = "7015";
        public static string TrasferenciaConsultaGastos = "7016";
        public static string TrasferenciaEjecuta = "7025";
    }
}
