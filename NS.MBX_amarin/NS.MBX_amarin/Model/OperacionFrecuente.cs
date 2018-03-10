using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Model
{
    public class OperacionFrecuente
    {
        public string Id { get; set; }
        public string Operacion { get; set; }
        public string SubOperacion { get; set; }
        public DateTime FechaOperacion { get; set; }
        public Servicio Servicio { get; set; }
        public string NombreFrecuente { get; set; }
    }
}
