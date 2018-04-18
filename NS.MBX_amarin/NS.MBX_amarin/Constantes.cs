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

        //google places api
        public static readonly string placesApiKey = "AIzaSyBJwIS9G1hf8u3KSCIKyRQUZUMnglnWOtQ";//por ahora es publica

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
        public static readonly string pageSubOperaciones = "SubOperaciones";
        public static readonly string pageMiPerfil = "MiPerfil";
        public static readonly string pageTransfCtaPropia = "TransfCtaPropia";
        public static readonly string pageTransfResumen = "TransfResumen";
        public static readonly string pageTransfCtaPropiaDestino = "TransfCtaPropiaDestino";
        public static readonly string pageTransfCtaPropiaDatos = "TransfCtaPropiaDatos";
        public static readonly string pageTransfConfirmacion = "TransfConfirmacion";
        public static readonly string pageTransfCuentaTercero = "TransfCuentatercero";
        public static readonly string pageTransfCtaTerceroDestino = "TransfCtaTerceroDestino";
        public static readonly string pageTransfCuentaOtroBanco = "TransfCuentaOtroBanco";
        public static readonly string pageTransfCtaOtroBancoDestino = "TransfCtaOtroBancoDestino";
        public static readonly string pagePagoTCTipo = "PagoTCTipo";
        public static readonly string pagePagoTCDatos = "PagoTCDatos";
        public static readonly string pagePagoTCPropTipo = "PagoTCPropTipo";
        public static readonly string pageTCPropia = "TCPropia";

        //constantes para mensajes dialog
        public static readonly string MSJ_VALIDACION = "Validación";
        public static readonly string MSJ_INFO = "Información";
        public static readonly string MSJ_BOTON_OK = "Ok";
        public static readonly string MSJ_BOTON_ACEPTAR = "Aceptar";
        public static readonly string MSJ_EXITO = "La operación fue realizada con éxito";
    }
}
