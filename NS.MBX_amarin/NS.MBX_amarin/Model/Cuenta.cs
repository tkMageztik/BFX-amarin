using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Model
{
    public class Cuenta
    {
        public string idCta { get; set; }
        public string NombreCta { get; set; }
        public decimal SaldoDisponible { get; set; }
        public string Moneda { get; set; }
        public string idMoneda { get; set; } //PEN, USD
    }
}
