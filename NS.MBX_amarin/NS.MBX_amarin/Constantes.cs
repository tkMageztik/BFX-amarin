using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin
{
    public static class Constantes
    {
        public static string SoapUrl = "http://todoasmxservicembx.azurewebsites.net/TodoService.asmx";
        public static readonly string msjExito = "La operación fue realizada con éxito";
        public static readonly string msjNoSaldo = "No hay suficiente saldo en la cuenta de origen";

        //constantes para navegacion
        public static readonly string pageOrigen = "pageOrigen";
        public static readonly string pageTipoTarjeta = "TipoTarjeta";
        public static readonly string pageCtaCargo = "CtaCargo";
        public static readonly string pageDatosPagoTarjeta = "DatosPagoTarjeta";
        public static readonly string pageConfPagoServicioEmpresa = "ConfPagoServicioEmpresa";
        public static readonly string pageConfDatosPago = "ConfDatosPago";
        public static readonly string pageRecargaCelular = "RecargaCelular";
        public static readonly string pageRecargaBim = "RecargaBim";
        public static readonly string pageEmpresa = "Empresa";
        public static readonly string pageBuscadorEmpresa = "BuscadorEmpresa";
        public static readonly string pageOperaciones = "Operaciones";
        public static readonly string pageMiPerfil = "MiPerfil";

        //constantes para mensajes dialog
        public static readonly string MSJ_VALIDACION = "Validación";
        public static readonly string MSJ_INFO = "Información";
        public static readonly string MSJ_BOTON_OK = "Ok";
        public static readonly string MSJ_BOTON_ACEPTAR = "Aceptar";
        public static readonly string MSJ_EXITO = "La operación fue realizada con éxito";
    }
}
