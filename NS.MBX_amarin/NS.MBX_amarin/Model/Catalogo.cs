using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Model
{
    public class Catalogo
    {
        public int IdTabla { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public int IdEstado { get; set; }
    }
}
