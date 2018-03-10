using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public interface IOperacionService
    {
        ObservableCollection<SubOperacion> ListaSuboperacionesFrecuentes { get; set; }
        ObservableCollection<Operacion> ListarOperaciones();
        ObservableCollection<SubOperacion> ListarSubOperaciones(string id);
        ObservableCollection<SubOperacion> ListarSuboperacionesFrecuentes();
        void AgregarSuboperacionFrecuente(SubOperacion suboperacion);
    }
}
