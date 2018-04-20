using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Model
{
    public class OperacionFrecuente
    {
        public string IdOperacionFrecuente { get; set; }
        public string NombreFrecuente { get; set; }
        public Operacion Operacion { get; set; }
        public SubOperacion SubOperacion { get; set; }
        public DateTime FechaOperacion { get; set; }
        public Catalogo Picker1 { get; set; } //servicio, tipo tarjeta, etc
        public Catalogo Picker2 { get; set; } //moneda, etc
        public string parametro1 { get; set; } //num tarjeta, num telefono, etc
        public Cuenta CtaOrigen { get; set; }
        public Cuenta CtaDestino { get; set; }
        
        public Catalogo Moneda { get; set; }

        public Servicio Servicio { get; set; }

        public Tarjeta TcDestino { get; set; }
        public Catalogo OrigenTarjeta { get; set; } //es propio banco o de otro banco
        public Catalogo TipoPropTarjeta { get; set; } //es tarjeta propia o de un tercero

    }
}
