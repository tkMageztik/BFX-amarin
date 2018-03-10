using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services.Impl
{
    public class TipoCambioService : ITipoCambioService
    {
        public TipoCambio obtenerTipoCambio()
        {
            TipoCambio tipo = new TipoCambio { CompraDolares = 3.175M, VentaDolares = 3.345M };

            return tipo;
        }

        public String ObtenerDescTipoCambio()
        {
            TipoCambio tipo = obtenerTipoCambio();

            return "Tipo de cambio ref. Compra: " + tipo.CompraDolares.ToString() + " Venta: " + tipo.VentaDolares.ToString();
        }
    }
}
