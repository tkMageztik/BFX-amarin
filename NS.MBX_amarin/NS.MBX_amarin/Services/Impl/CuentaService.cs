using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Services.Impl
{
    public class CuentaService : ICuentaService
    {
        private ITipoCambioService TipoCambioService { get; set; }

        public CuentaService(ITipoCambioService tipoCambioService)
        {
            TipoCambioService = tipoCambioService;
        }

        //agregar = true es para incremento, false para decremento
        public string efectuarMovimiento(Cuenta cuenta, decimal monto, string monedaMonto, bool incremento)
        {
            string msj = "";
            if(cuenta != null)
            {
                decimal montoCambiado = monto;
                TipoCambio tipoCambio = TipoCambioService.obtenerTipoCambio();

                if (cuenta.idMoneda == "PEN" && monedaMonto == "USD")
                {
                    montoCambiado = decimal.Round(Decimal.Multiply(monto, tipoCambio.CompraDolares), 2, MidpointRounding.AwayFromZero);
                }
                else if (cuenta.idMoneda == "USD" && monedaMonto == "PEN")
                {
                    montoCambiado = decimal.Round(Decimal.Divide(monto, tipoCambio.VentaDolares), 2, MidpointRounding.AwayFromZero);
                }

                if (incremento)
                {
                    cuenta.SaldoDisponible = decimal.Round(Decimal.Add(montoCambiado, cuenta.SaldoDisponible), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    if(cuenta.SaldoDisponible >= montoCambiado)
                    {
                        cuenta.SaldoDisponible = decimal.Round(Decimal.Subtract(cuenta.SaldoDisponible, montoCambiado), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        msj = Constantes.msjNoSaldo;
                    }
                }
            }

            return msj;
        }
    }
}
