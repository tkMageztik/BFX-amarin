using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services.Impl
{
    public class TarjetaService
    {
        public ObservableCollection<Tarjeta> ListarTarjetasCredito(string codCliente)
        {
            List<Tarjeta> lista = new List<Tarjeta>
            {
                new Tarjeta { NroTarjeta = "4323414356765434", NombreCliente = "Juan Perez Mendez", IsTarjetaCredito = true,  IdEstado = 1 }
            };

            return new ObservableCollection<Tarjeta>(lista);
        }
    }
}
