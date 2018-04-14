using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public interface ITarjetaService
    {
        ObservableCollection<Tarjeta> ListarTarjetasCredito(string codCliente);
    }
}
