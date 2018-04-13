using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NS.MBX_amarin.Services.Impl
{
    public class OperacionService : IOperacionService
    {
        private ObservableCollection<OperacionFrecuente> listaOperacionesFrecuentes = new ObservableCollection<OperacionFrecuente>();

        public ObservableCollection<OperacionFrecuente> ListaOperacionesFrecuentes { get => listaOperacionesFrecuentes; set => listaOperacionesFrecuentes = value; }

        public string GenerarCodigoOperacion()
        {
            return "231230";
        }
        
        public ObservableCollection<Operacion> ListarOperaciones()
        {
            List<Operacion> lista = new List<Operacion>
            {
                new Operacion { Id= "1", Nombre = "Pagos" },
                new Operacion { Id= "2", Nombre = "Recargas" },
                new Operacion { Id= "3", Nombre = "Transferencias" }
            };

            return new ObservableCollection<Operacion>(lista);
        }

        public Operacion BuscarOperacion(string id)
        {
            return ListarOperaciones().Where(p => p.Id == id).First();
        }

        //public Operacion BuscarOperacion(string id)
        //{
        //    ObservableCollection<Operacion> lista = ListarOperaciones();

        //    foreach(Operacion ope in lista)
        //    {
        //        if(ope.Id == id)
        //        {
        //            return ope;
        //        }
        //    }

        //    return null;
        //}

        public ObservableCollection<SubOperacion> ListarSubOperaciones(string id)
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
        public ObservableCollection<OperacionFrecuente> ListarOperacionesFrecuentes()
        {
            List<OperacionFrecuente> listaOrdenada = ListaOperacionesFrecuentes.OrderBy(x => x.FechaOperacion).ToList() ;

            return new ObservableCollection<OperacionFrecuente>(listaOrdenada);
        }

        public void AgregarOperacionFrecuente(OperacionFrecuente opeFrec)
        {
            bool encontro = false;
            OperacionFrecuente existente = null;
            //buscamos si ya existe
            foreach(OperacionFrecuente frec in ListaOperacionesFrecuentes)
            {
                if(opeFrec.SubOperacion.Id == frec.SubOperacion.Id && opeFrec.Operacion.Id == frec.Operacion.Id )
                {
                    //condicionales dependiendo de la operacion
                    if(opeFrec.Operacion.Id == "1")
                    {
                        if ((opeFrec.SubOperacion.Id == "0" || opeFrec.SubOperacion.Id == "1") && frec.Servicio != null && opeFrec.Servicio.IdEmpresa == frec.Servicio.IdEmpresa)
                        {
                            encontro = true;
                        }else if (opeFrec.SubOperacion.Id == "2" && frec.Picker1 != null && opeFrec.Picker1.Codigo == frec.Picker1.Codigo)
                        {
                            encontro = true;
                        }
                    }
                    else if (opeFrec.Operacion.Id == "2")
                    {
                        if (opeFrec.SubOperacion.Id == "0" || opeFrec.SubOperacion.Id == "1")
                        {
                            encontro = true;
                        }
                    }
                }
                if (encontro)
                {
                    existente = frec;
                    break;
                }
            }

            if (!encontro)
            {
                ListaOperacionesFrecuentes.Add(opeFrec);
            }
            else
            {
                var index = listaOperacionesFrecuentes.IndexOf(existente);
                if(index != -1)
                {
                    listaOperacionesFrecuentes[index] = opeFrec;
                }
            }
        }
    }
}
