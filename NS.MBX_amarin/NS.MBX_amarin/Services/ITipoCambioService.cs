using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public interface ITipoCambioService
    {
        TipoCambio obtenerTipoCambio();
        String ObtenerDescTipoCambio();
    }
}
