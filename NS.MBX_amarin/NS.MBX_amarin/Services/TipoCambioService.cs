using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public static class TipoCambioService
    {
        public static TipoCambio obtenerTipoCambio()
        {
            TipoCambio tipo = new TipoCambio { CompraDolares = 3.175M, VentaDolares = 3.345M };

            return tipo;
        }
    }
}
