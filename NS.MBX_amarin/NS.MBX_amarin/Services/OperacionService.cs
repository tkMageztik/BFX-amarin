using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public static class OperacionService
    {
        private static ObservableCollection<SubOperacion> listaSuboperacionesFrecuentes = new ObservableCollection<SubOperacion>();

        public static ObservableCollection<SubOperacion> ListaSuboperacionesFrecuentes { get => listaSuboperacionesFrecuentes; set => listaSuboperacionesFrecuentes = value; }

        public static ObservableCollection<Operacion> ListarOperaciones()
        {
            List<Operacion> lista = new List<Operacion>
            {
                new Operacion { Id= "1", Nombre = "Pagos" },
                new Operacion { Id= "2", Nombre = "Recargas" },
                new Operacion { Id= "3", Nombre = "Transferencias" }
            };

            return new ObservableCollection<Operacion>(lista);
        }

        public static ObservableCollection<SubOperacion> ListarSubOperaciones(string id)
        {
            List<SubOperacion> lista = null;
            if (id == "1")
            {

                lista = new List<SubOperacion>
                {
                    new SubOperacion { Id= "0", IdOperacion="1", Nombre = "Pago de Servicios" },
                    new SubOperacion { Id= "1", IdOperacion="1", Nombre = "Pago a Institución o Empresa" },
                    new SubOperacion { Id= "2", IdOperacion="1", Nombre = "Pago de Tarjetas de Crédito" }
                };
            }
            else if (id == "2")
            {

                lista = new List<SubOperacion>
                {
                    new SubOperacion { Id= "0",IdOperacion="2", Nombre = "Recarga de Celular" },
                    new SubOperacion { Id= "1",IdOperacion="2", Nombre = "Recarga de Billetera Móvil" }
                };
            }
            else if (id == "3")
            {

                lista = new List<SubOperacion>
                {
                    new SubOperacion { Id= "0",IdOperacion="3", Nombre = "A otra cuenta Financiero" },
                    new SubOperacion { Id= "1",IdOperacion="3", Nombre = "A otro banco" },
                    new SubOperacion { Id= "2",IdOperacion="3", Nombre = "A cuenta propia" }
                };
            }

            return new ObservableCollection<SubOperacion>(lista);
        }

        //se ordena del mas reciente al menos
        public static ObservableCollection<SubOperacion> ListarSuboperacionesFrecuentes()
        {
            List<SubOperacion> listaOrdenada = ListaSuboperacionesFrecuentes.OrderBy(x => x.FechaOperacion).ToList() ;

            return new ObservableCollection<SubOperacion>(listaOrdenada);
        }

        public static void AgregarSuboperacionFrecuente(SubOperacion suboperacion)
        {
            bool encontro = false;
            //buscamos si ya existe
            foreach(SubOperacion sub in listaSuboperacionesFrecuentes)
            {
                if(sub.Id == suboperacion.Id && sub.IdOperacion == suboperacion.IdOperacion && suboperacion.ServicioFrecuente.IdEmpresa == sub.ServicioFrecuente.IdEmpresa)
                {
                    encontro = true;
                    break;
                }
            }

            if (!encontro)
            {
                ListaSuboperacionesFrecuentes.Add(suboperacion);
            }
        }
    }
}
