using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public static class CatalogoService
    {
        public static ObservableCollection<Catalogo> ListarEmpresas()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Direct TV", IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Edelnor", IdEstado = 1 },
                new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Sedapal", IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public static Catalogo BuscarEmpresa(string codigo)
        {
            ObservableCollection<Catalogo> lista = ListarEmpresas();

            Catalogo empresa = null;

            foreach (Catalogo cat in lista)
            {
                if (cat.Codigo == codigo)
                {
                    empresa = cat;
                    break;
                }
            }

            return empresa;
        }

        public static ObservableCollection<Servicio> ListarServiciosxEmpresa(string id)
        {
            List<Servicio> lista = null;
            if (id == "0")
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = "0", Codigo = "0", Nombre = "Instalación Post Pago" },
                    new Servicio { IdEmpresa = "0", Codigo = "1", Nombre = "Mensual Post" },
                    new Servicio { IdEmpresa = "0", Codigo = "2", Nombre = "Recarga" }
                };
            }
            else if (id == "1")
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = "1", Codigo = "0", Nombre = "Luz" }
                };
            }
            else if (id == "2")
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = "2", Codigo = "0", Nombre = "Agua" }
                };
            }

            return new ObservableCollection<Servicio>(lista);
        }

        public static ObservableCollection<Servicio> ListarServiciosxEmpresa(int id)
        {
            return ListarServiciosxEmpresa(id.ToString());
        }
    }
}
