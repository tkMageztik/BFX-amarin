﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Model
{
    public class Tarjeta
    {
        public string NroTarjeta { get; set; }
        public string NombreCliente { get; set; }
        public string Email { get; set; }
        public bool IsTarjetaCredito { get; set; }
        public int IdEstado { get; set; }
    }
}
