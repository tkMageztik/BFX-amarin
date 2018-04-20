using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services.Impl
{
    public class TarjetaService : ITarjetaService
    {
        private Tarjeta Tarjeta = new Tarjeta { NroTarjeta = "4323414356765434", NombreCliente = "Juan Perez Mendez", MarcaTarjeta = "Tarjeta de Crédito Visa", IsTarjetaCredito = true, DeudaMinMes = 50.00M, DeudaTotMes = 150M, IdEstado = 1 };

        public ObservableCollection<Tarjeta> ListarTarjetasCredito(string codCliente)
        {
            List<Tarjeta> lista = new List<Tarjeta>
            {
                Tarjeta
            };

            return new ObservableCollection<Tarjeta>(lista);
        }
    }
}
