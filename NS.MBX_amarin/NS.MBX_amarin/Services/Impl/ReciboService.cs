using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services.Impl
{
    public class ReciboService : IReciboService
    {
        public Recibo ObtenerRecibosCelular(string numTelefono)
        {
            return new Recibo
            {
                Codigo = numTelefono,
                NombreCliente = "MIGUEL VIVAR",
                ListaDetalle =
                    new ObservableCollection<DetalleRecibo>
                    {
                        new DetalleRecibo{IdDetalleRecibo = "55443", Monto = "59.90", FechaEmision = "05/02/2018", FechaVencimiento = "25/07/2018" }
                 ,   }
            };
        }

        public Recibo ObtenerRecibosTelfFijo(string numTelefono)
        {
            return new Recibo
            {
                Codigo = numTelefono,
                NombreCliente = "MIGUEL VIVAR",
                ListaDetalle =
                    new ObservableCollection<DetalleRecibo>
                    {
                        new DetalleRecibo{IdDetalleRecibo = "12333", Monto = "89.90", FechaEmision = "05/02/2018", FechaVencimiento = "25/09/2018" }
                 ,   }
            };            
            
        }

        public Recibo ObtenerRecibos(string codigo)
        {
            return new Recibo
            {
                Codigo = codigo,
                NombreCliente = "SAMUEL PEREZ",
                ListaDetalle =
                    new ObservableCollection<DetalleRecibo>
                    {
                        new DetalleRecibo{IdDetalleRecibo = "33224", Monto = "79.90", FechaEmision = "05/02/2018", FechaVencimiento = "28/09/2018" }
                 ,   }
            };

        }
    }
}
