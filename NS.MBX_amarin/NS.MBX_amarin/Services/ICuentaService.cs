using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public interface ICuentaService
    {
        ObservableCollection<Cuenta> listaCuentas { get; set; }
        string efectuarMovimiento(Cuenta cuenta, decimal monto, string monedaMonto, bool incremento);
        bool ValidarSaldoOperacion(Cuenta cuenta, decimal monto, string monedaMonto);
    }
}
