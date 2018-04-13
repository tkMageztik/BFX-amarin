using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public interface IOperacionService
    {
        ObservableCollection<OperacionFrecuente> ListaOperacionesFrecuentes { get; set; }
        ObservableCollection<Operacion> ListarOperaciones();
        Operacion BuscarOperacion(string id);
        ObservableCollection<SubOperacion> ListarSubOperaciones(string id);
        ObservableCollection<OperacionFrecuente> ListarOperacionesFrecuentes();
        void AgregarOperacionFrecuente(OperacionFrecuente opeFrec);
        string GenerarCodigoOperacion();
    }
}
