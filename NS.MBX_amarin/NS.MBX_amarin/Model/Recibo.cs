using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Model
{
    public class Recibo
    {
        public string Codigo { get; set; } //el codigo, num telefono o numero de suministro
        public string Nombre { get; set; }
        public string NombreCliente { get; set; }
        public ObservableCollection<DetalleRecibo> ListaDetalle { get; set; }
}
}
