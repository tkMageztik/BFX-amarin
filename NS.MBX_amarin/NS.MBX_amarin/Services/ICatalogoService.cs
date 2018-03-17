using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NS.MBX_amarin.Services
{
    public interface ICatalogoService
    {
        string COD_OPE_MOVIL {  get; }
        string COD_OPC_ADICIONALES { get; }

        ObservableCollection<Catalogo> ObtenerListaPorCodigo(string codCatalogo);
        Catalogo BuscarPorNombre(string codCatalogo, string nombre);
        Catalogo BuscarPorCodigo(string codCatalogo, string codigo);

        Catalogo BuscarMonedaPorCodigo(string codigo);
        ObservableCollection<Catalogo> ListarOpcionesAdicionales();
        ObservableCollection<string> ListarOperadoresMovilesString();
        ObservableCollection<Catalogo> ListarOperadoresMoviles();
        Catalogo BuscarMonedaPorNombre(string nombre);
        ObservableCollection<string> ListarMonedasString();
        ObservableCollection<Catalogo> ListarMonedas();
        ObservableCollection<Catalogo> ListarTiposTarjetaCredito();
        Catalogo ObtenerTipoTarjetaCredito(string codigo);
        ObservableCollection<Catalogo> ListarEmpresasConServicios();
        Catalogo BuscarEmpresaConServicios(string codigo);
        ObservableCollection<Servicio> ListarServiciosxEmpresa(string id);
        ObservableCollection<Servicio> ListarServiciosxEmpresa(int id);
        ObservableCollection<Catalogo> BuscarEmpresa(string nombre);
    }
}
