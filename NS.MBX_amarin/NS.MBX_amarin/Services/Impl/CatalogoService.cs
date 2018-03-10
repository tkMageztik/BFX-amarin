using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NS.MBX_amarin.Services.Impl
{
    public class CatalogoService : ICatalogoService
    {

        public ObservableCollection<string> ListarOperadoresMovilesString()
        {
            ObservableCollection<Catalogo> listaCat = ListarOperadoresMoviles();

            List<string> lista = new List<string>();

            foreach (Catalogo cat in listaCat)
            {
                lista.Add(cat.Nombre);
            }

            return new ObservableCollection<string>(lista);
        }

        public ObservableCollection<Catalogo> ListarOperadoresMoviles()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Claro",  IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Movistar", IdEstado = 1 },
                 new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Entel",  IdEstado = 1 },
                  new Catalogo { IdTabla = 3, Codigo = "3", Nombre = "Bitel",  IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public Catalogo BuscarMonedaPorNombre(string nombre)
        {
            ObservableCollection<Catalogo> listaCat = ListarMonedas();
            
            foreach (Catalogo cat in listaCat)
            {
                if(cat.Nombre == nombre)
                {
                    return cat;
                }
            }

            return null;
        }

        public ObservableCollection<string> ListarMonedasString()
        {
            ObservableCollection<Catalogo> listaCat = ListarMonedas();

            List<string> lista = new List<string>();

            foreach(Catalogo cat in listaCat)
            {
                lista.Add(cat.Nombre);
            }

            return new ObservableCollection<string>(lista);
        }

        public ObservableCollection<Catalogo> ListarMonedas()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "PEN", Nombre = "Soles", Descripcion = "S/", IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "USD", Nombre = "Dólares",Descripcion = "$",  IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public ObservableCollection<Catalogo> ListarTiposTarjetaCredito()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Otra tarjeta Financiero", Descripcion = "Tarjeta de Crédito", IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Tarjeta de otro banco", Descripcion = "Tarjeta de Crédito", IdEstado = 1 },
                new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Tarjeta Diners", Descripcion = "Tarjeta de Crédito Diners", IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public Catalogo ObtenerTipoTarjetaCredito(string codigo)
        {
            ObservableCollection<Catalogo> lista = ListarTiposTarjetaCredito();

            foreach (Catalogo cat in lista)
            {
                if (cat.Codigo == codigo)
                {
                    return cat;
                }
            }

            return null;
        }

        public ObservableCollection<Catalogo> ListarEmpresasConServicios()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Direct TV", IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Edelnor", IdEstado = 1 },
                new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Sedapal", IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public Catalogo BuscarEmpresaConServicios(string codigo)
        {
            ObservableCollection<Catalogo> lista1 = ListarEmpresasConServicios();

            ObservableCollection<Catalogo> lista2 = ListarEmpresas();
            
            //unir las listas
            foreach(Catalogo cat in lista2)
            {
                lista1.Add(cat);
            }

            Catalogo empresa = null;

            //recorrer
            foreach (Catalogo cat in lista1)
            {
                if (cat.Codigo == codigo)
                {
                    empresa = cat;
                    break;
                }
            }


            return empresa;
        }

        public ObservableCollection<Servicio> ListarServiciosxEmpresa(string id)
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

        public ObservableCollection<Servicio> ListarServiciosxEmpresa(int id)
        {
            return ListarServiciosxEmpresa(id.ToString());
        }

        public ObservableCollection<Catalogo> ListarEmpresas()
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
        public ObservableCollection<Catalogo> BuscarEmpresa(string nombre)
        {
            ObservableCollection<Catalogo> listaTotal = ListarEmpresas();
            List<Catalogo> listaFiltro = listaTotal.Where(c => c.Nombre.ToUpper().Contains(nombre.ToUpper())).ToList();

            return new ObservableCollection<Catalogo>(listaFiltro);
        }

    }
}
