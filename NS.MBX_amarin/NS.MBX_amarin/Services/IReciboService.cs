using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public interface IReciboService
    {
        Recibo ObtenerRecibosCelular(string numTelefono);
        Recibo ObtenerRecibosTelfFijo(string numTelefono);
        Recibo ObtenerRecibos(string codigo);
    }
}
