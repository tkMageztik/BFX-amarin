using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public interface ICuentaService
    {
        string efectuarMovimiento(Cuenta cuenta, decimal monto, string monedaMonto, bool incremento);
    }
}
