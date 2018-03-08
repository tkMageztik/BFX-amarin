using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Model
{
    public class User
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        [MaxLength(16)]
        public string NroTarjeta { get; set; }

        public string TipDoc { get; set; }
        public string NroDoc { get; set; }

    }
}
