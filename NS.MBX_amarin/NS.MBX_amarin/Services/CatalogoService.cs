using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public static class CatalogoService
    {
        public static ObservableCollection<Catalogo> ListarEmpresasConServicios()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Direct TV", IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Edelnor", IdEstado = 1 },
                new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Sedapal", IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public static Catalogo BuscarEmpresaConServicios(string codigo)
        {
            ObservableCollection<Catalogo> lista = ListarEmpresasConServicios();

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
            //san marcos
            else if (id == "15")
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = id, Codigo = "0", Nombre = "Pago de matrícula" },
                    new Servicio { IdEmpresa = id, Codigo = "1", Nombre = "Rectificación" }
                };
            }
            //instituto certus
            else if (id == "2")
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = id, Codigo = "0", Nombre = "Pago de Ciclo" },
                    new Servicio { IdEmpresa = id, Codigo = "1", Nombre = "Pago de Matrícula" }
                };
            }
            //ucv
            else if (id == "2")
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = id, Codigo = "0", Nombre = "Pago de matrícula" },
                    new Servicio { IdEmpresa = id, Codigo = "1", Nombre = "Rectificación" }
                };
            }

            return new ObservableCollection<Servicio>(lista);
        }

        public static ObservableCollection<Servicio> ListarServiciosxEmpresa(int id)
        {
            return ListarServiciosxEmpresa(id.ToString());
        }

        public static ObservableCollection<Catalogo> ListarEmpresas()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 15, Codigo = "15", Nombre = "Universidad San Marcos", IdEstado = 1 },
                new Catalogo { IdTabla = 16, Codigo = "16", Nombre = "Instituto Certus", IdEstado = 1 },
                new Catalogo { IdTabla = 17, Codigo = "17", Nombre = "Universidad UCV", IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        //insensitivo
        public static ObservableCollection<Catalogo> BuscarEmpresa(string nombre)
        {
            ObservableCollection<Catalogo> listaTotal = CatalogoService.ListarEmpresas();
            List<Catalogo> listaFiltro = listaTotal.Where(c => c.Nombre.ToUpper().Contains(nombre.ToUpper())).ToList();

            return new ObservableCollection<Catalogo>(listaFiltro);
        }

    }
}
