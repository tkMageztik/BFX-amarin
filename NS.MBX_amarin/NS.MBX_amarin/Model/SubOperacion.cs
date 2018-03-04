using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Model
{
    public class SubOperacion
    {
        public string Id { get; set; }
        public string IdOperacion { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaOperacion { get; set; }
        public Servicio ServicioFrecuente { get; set; }
        public string NombreFrecuente { get; set; }
    }
}
