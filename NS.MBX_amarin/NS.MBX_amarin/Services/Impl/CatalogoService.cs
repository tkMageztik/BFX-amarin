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
        //constantes para los catalogos
        private readonly string _COD_OPE_MOVIL = "COD_OPE_MOVIL";
        public string COD_OPE_MOVIL { get => _COD_OPE_MOVIL; }

        private readonly string _COD_OPC_ADICIONALES = "COD_OPC_ADICIONALES";
        public string COD_OPC_ADICIONALES { get => _COD_OPC_ADICIONALES; }

        private readonly string _COD_UBICACIONES_MAPS = "COD_UBICACIONES_MAPS";
        public string COD_UBICACIONES_MAPS { get => _COD_UBICACIONES_MAPS; }

        private readonly string _COD_TIPOS_CTA = "COD_TIPOS_CTA";
        public string COD_TIPOS_CTA { get => _COD_TIPOS_CTA; }

        private readonly string _COD_OPC_SI_NO = "COD_OPC_SI_NO";
        public string COD_OPC_SI_NO { get => _COD_OPC_SI_NO; }

        private Cliente Cliente = new Cliente{ Nombre = "Jose", Celular = "98****567", Email = "emacliente@gmail.com"};

        public Cliente ObtenerCliente()
    {
            return Cliente;
    }
        
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

        public ObservableCollection<Catalogo> ListarOpcionesSiNo()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Si",  IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "No", IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
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

        public ObservableCollection<Catalogo> ListarTiposCuenta()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Cuenta Ahorros Soles", Descripcion = "PEN", IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Cuenta Ahorros Dólares", Descripcion = "USD",IdEstado = 1 },
                new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Cuenta Corriente Soles", Descripcion = "PEN", IdEstado = 1 },
                new Catalogo { IdTabla = 3, Codigo = "3", Nombre = "Cuenta Corriente Dólares", Descripcion = "USD", IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public ObservableCollection<Catalogo> ListarUbicacionesMaps()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Av. Ricardo Palma 232",  IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Jr. Casas 223", IdEstado = 1 },
                new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Av. Aranceles 555",  IdEstado = 1 },
                new Catalogo { IdTabla = 3, Codigo = "3", Nombre = "Calle Rosario 655",  IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public ObservableCollection<Catalogo> ListarOpcionesAdicionales()
        {
            List<Catalogo> lista = new List<Catalogo>
            {
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Configuraciones",  IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Mi perfil", IdEstado = 1 },
                new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Servicio al cliente",  IdEstado = 1 },
                new Catalogo { IdTabla = 3, Codigo = "3", Nombre = "Solicitudes",  IdEstado = 1 }
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public Catalogo BuscarPorCodigo(string codCatalogo, string codigo)
        {
            ObservableCollection<Catalogo> listaCat = ObtenerListaPorCodigo(codCatalogo);
            
            foreach (Catalogo cat in listaCat)
            {
                if (cat.Codigo == codigo)
                {
                    return cat;
                }
            }

            return null;
        }

        public Catalogo BuscarPorNombre(string codCatalogo, string nombre)
        {
            ObservableCollection<Catalogo> listaCat = ObtenerListaPorCodigo(codCatalogo);

            foreach (Catalogo cat in listaCat)
            {
                if (cat.Nombre == nombre)
                {
                    return cat;
                }
            }

            return null;
        }

        public ObservableCollection<Catalogo> BuscarCoincidePorNombre(string codCatalogo, string nombre)
        {
            if(!string.IsNullOrEmpty(nombre))
            {
                ObservableCollection<Catalogo> listaCat = ObtenerListaPorCodigo(codCatalogo);

                List<Catalogo> listaFiltro = listaCat.Where(c => c.Nombre.ToUpper().Contains(nombre.ToUpper())).ToList();

                return new ObservableCollection<Catalogo>(listaFiltro);
            }

            return null;
        }

        public ObservableCollection<Catalogo> ObtenerListaPorCodigo(string codCatalogo)
        {
            ObservableCollection<Catalogo> lista = null;

            if (codCatalogo == COD_OPE_MOVIL)
            {
                lista = ListarOperadoresMoviles();
            }else if(codCatalogo == COD_OPC_ADICIONALES)
            {
                lista = ListarOpcionesAdicionales();
            }else if(codCatalogo == COD_UBICACIONES_MAPS)
            {
                lista = ListarUbicacionesMaps();
            }
            else if (codCatalogo == COD_TIPOS_CTA)
            {
                lista = ListarTiposCuenta();
            }
            else if (codCatalogo == COD_OPC_SI_NO)
            {
                lista = ListarOpcionesSiNo();
            }

            return lista;
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

        public Catalogo BuscarMonedaPorCodigo(string codigo)
        {
            ObservableCollection<Catalogo> listaCat = ListarMonedas();

            foreach (Catalogo cat in listaCat)
            {
                if (cat.Codigo == codigo)
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
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Tarjeta propio banco", Descripcion = "Tarjeta de Crédito", IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Tarjeta de otro banco", Descripcion = "Tarjeta de Crédito", IdEstado = 1 }
                //,new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Tarjeta Diners", Descripcion = "Tarjeta de Crédito Diners", IdEstado = 1 }
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
                new Catalogo { IdTabla = 0, Codigo = "0", Nombre = "Claro", IdEstado = 1 },
                new Catalogo { IdTabla = 1, Codigo = "1", Nombre = "Movistar", IdEstado = 1 },//telefonia celular y fija
                new Catalogo { IdTabla = 2, Codigo = "2", Nombre = "Entel Perú", IdEstado = 1 },
                new Catalogo { IdTabla = 3, Codigo = "3", Nombre = "Edelnor", IdEstado = 1 },
                new Catalogo { IdTabla = 4, Codigo = "4", Nombre = "Luz del Sur", IdEstado = 1 },
                new Catalogo { IdTabla = 5, Codigo = "5", Nombre = "Sedapal", IdEstado = 1 },
            };

            return new ObservableCollection<Catalogo>(lista);
        }

        public Catalogo BuscarEmpresaConServicios(string codigo)
        {
            ObservableCollection<Catalogo> lista1 = ListarEmpresasConServicios();

            //ObservableCollection<Catalogo> lista2 = ListarEmpresas();
            
            ////unir las listas
            //foreach(Catalogo cat in lista2)
            //{
            //    lista1.Add(cat);
            //}

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
            if (id == "0")//claro
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = "0", Codigo = "0", Nombre = "Pago Celular" }
                };
            }
            else if (id == "1")//movistar
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = "1", Codigo = "0", Nombre = "Pago Celular" },
                    new Servicio { IdEmpresa = "1", Codigo = "1", Nombre = "Teléfono Fijo" }
                };
            }
            else if (id == "2")//entel perú
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = "2", Codigo = "0", Nombre = "Pago Celular" }
                };
            }
            else if (id == "3")//edelnor
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = "3", Codigo = "0", Nombre = "Luz" }
                };
            }
            else if (id == "4")//luz sur
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = "4", Codigo = "0", Nombre = "Luz" }
                };
            }
            else if (id == "5")//sedapal
            {

                lista = new List<Servicio>
                {
                    new Servicio { IdEmpresa = "5", Codigo = "0", Nombre = "Agua" }
                };
            }
            //san marcos
            //else if (id == "15")
            //{

            //    lista = new List<Servicio>
            //    {
            //        new Servicio { IdEmpresa = id, Codigo = "0", Nombre = "Pago de matrícula" },
            //        new Servicio { IdEmpresa = id, Codigo = "1", Nombre = "Rectificación" }
            //    };
            //}
            ////instituto certus
            //else if (id == "2")
            //{

            //    lista = new List<Servicio>
            //    {
            //        new Servicio { IdEmpresa = id, Codigo = "0", Nombre = "Pago de Ciclo" },
            //        new Servicio { IdEmpresa = id, Codigo = "1", Nombre = "Pago de Matrícula" }
            //    };
            //}
            ////ucv
            //else if (id == "2")
            //{

            //    lista = new List<Servicio>
            //    {
            //        new Servicio { IdEmpresa = id, Codigo = "0", Nombre = "Pago de matrícula" },
            //        new Servicio { IdEmpresa = id, Codigo = "1", Nombre = "Rectificación" }
            //    };
            //}

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
            ObservableCollection<Catalogo> listaTotal = ListarEmpresasConServicios();
            List<Catalogo> listaFiltro = listaTotal.Where(c => c.Nombre.ToUpper().Contains(nombre.ToUpper())).ToList();

            return new ObservableCollection<Catalogo>(listaFiltro);
        }

    }
}
