using NS.MBX_amarin.Events;
using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NS.MBX_amarin.ViewModels
{
	public class OperacionesViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private IOperacionService OperacionService { get; set; }
        private IEventAggregator EventAggregator { get; set; }

        private string IdOperacion { get; set; }

        public OperacionesViewModel(IOperacionService operacionService, ICatalogoService catalogoService, INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            CatalogoService = catalogoService;
            OperacionService = operacionService;
            EventAggregator = eventAggregator;

            ListaOperaciones = OperacionService.ListarOperaciones();
            ListaOpeFrecuentes = operacionService.ListarOperacionesFrecuentes();

            //suscripcion
            EventAggregator.GetEvent<OpeFrecuenteAddedEvent>().Subscribe(ActualizarListaOpeFrecuentes);
        }

        private ObservableCollection<Operacion> _listaOperaciones;
        public ObservableCollection<Operacion> ListaOperaciones
        {
            get { return _listaOperaciones; }
            set { SetProperty(ref _listaOperaciones, value); }
        }

        private Operacion _opeSelected;
        public Operacion OpeSelected
        {
            get { return _opeSelected; }
            set { SetProperty(ref _opeSelected, value); }
        }

        private ObservableCollection<OperacionFrecuente> _listaOpeFrecuentes;
        public ObservableCollection<OperacionFrecuente> ListaOpeFrecuentes
        {
            get { return _listaOpeFrecuentes; }
            set { SetProperty(ref _listaOpeFrecuentes, value); }
        }

        private OperacionFrecuente _opeFrecSelected;
        public OperacionFrecuente OpeFrecSelected
        {
            get { return _opeFrecSelected; }
            set { SetProperty(ref _opeFrecSelected, value); }
        }

        private DelegateCommand _opeTappedIC;
        public DelegateCommand OpeTappedIC =>
            _opeTappedIC ?? (_opeTappedIC = new DelegateCommand(ExecuteOpeTappedIC));

        async void ExecuteOpeTappedIC()
        {
            try
            {
                NavigationParameters navParameters = new NavigationParameters();
                Operacion ope = new Operacion
                {
                    Id = OpeSelected.Id,
                    Nombre = OpeSelected.Nombre
                };
                navParameters.Add("Operacion", ope);
                navParameters.Add("IdOperacion", OpeSelected.Id);
                navParameters.Add("OrigenMisCuentas", false);
                await NavigationService.NavigateAsync("SubOperaciones", navParameters);

                OpeSelected = null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private DelegateCommand _opeFrecTappedIC;
        public DelegateCommand OpeFrecTappedIC =>
            _opeFrecTappedIC ?? (_opeFrecTappedIC = new DelegateCommand(ExecuteOpeFrecTappedIC));

        async void ExecuteOpeFrecTappedIC()
        {
            try
            {
                NavigationParameters navParameters = new NavigationParameters();
                navParameters.Add(Constantes.keyOperacionFrecuente, OpeFrecSelected);
                navParameters.Add(Constantes.keyOperacion, OpeFrecSelected.Operacion);
                navParameters.Add(Constantes.keySubOperacion, OpeFrecSelected.SubOperacion);

                //dependiendo de la operacion, envia la data necesaria
                if (OpeFrecSelected.Operacion.Id == "1")
                {
                    if (OpeFrecSelected.SubOperacion.Id == "0" || OpeFrecSelected.SubOperacion.Id == "1")//pago de servicios
                    {
                        //Application.Current.Properties["empresa"] = CatalogoService.BuscarEmpresaConServicios(OpeFrecSelected.Servicio.IdEmpresa);
                        //Application.Current.Properties["servicio"] = OpeFrecSelected.Servicio;
                        //Application.Current.Properties["pageOrigen"] = "OperacionesView";

                        navParameters.Add(Constantes.pageOrigen, Constantes.pageOperaciones);
                        navParameters.Add("Empresa", CatalogoService.BuscarEmpresaConServicios(OpeFrecSelected.Servicio.IdEmpresa));
                        navParameters.Add("Servicio", OpeFrecSelected.Servicio);

                        await NavigationService.NavigateAsync("ServicioEmpresa", navParameters);

                    }
                    else if (OpeFrecSelected.SubOperacion.Id == "2")//pago de tc
                    {
                        navParameters.Add(Constantes.pageOrigen, Constantes.pageOperaciones);
                        navParameters.Add(Constantes.keyOrigenTarjeta, OpeFrecSelected.OrigenTarjeta);
                        if (OpeFrecSelected.OrigenTarjeta.Codigo == "0")
                        {
                            if(OpeFrecSelected.TipoPropTarjeta.Codigo == "0")
                            {
                                await NavigationService.NavigateAsync(Constantes.pageTCPropia, navParameters);
                            }
                            else
                            {
                                await NavigationService.NavigateAsync(Constantes.pagePagoTCDatos, navParameters);
                            }
                        }
                        else
                        {
                            await NavigationService.NavigateAsync(Constantes.pagePagoTCDatos, navParameters);
                        }
                        
                        
                    }
                }
                else if (OpeFrecSelected.Operacion.Id == "2")//recargas
                {
                    if (OpeFrecSelected.SubOperacion.Id == "0")//recarga de celu
                    {
                        navParameters.Add(Constantes.pageOrigen, Constantes.pageOperaciones);
                        
                        await NavigationService.NavigateAsync(Constantes.pageRecargaCelular, navParameters);

                    }
                    else if (OpeFrecSelected.SubOperacion.Id == "1")
                    {
                        navParameters.Add(Constantes.pageOrigen, Constantes.pageOperaciones);
                        
                        await NavigationService.NavigateAsync(Constantes.pageRecargaBim, navParameters);
                    }
                }
                else if (OpeFrecSelected.Operacion.Id == "3")//transferencias
                {
                    if (OpeFrecSelected.SubOperacion.Id == "2")//cta propia
                    {
                        navParameters.Add(Constantes.pageOrigen, Constantes.pageOperaciones);

                        await NavigationService.NavigateAsync(Constantes.pageTransfCtaPropiaDatos, navParameters);

                    }
                    else if (OpeFrecSelected.SubOperacion.Id == "0")//tercero
                    {
                        navParameters.Add(Constantes.pageOrigen, Constantes.pageOperaciones);

                        await NavigationService.NavigateAsync(Constantes.pageTransfCtaTerceroDestino, navParameters);
                    }
                    else if (OpeFrecSelected.SubOperacion.Id == "1")//otro banco
                    {
                        navParameters.Add(Constantes.pageOrigen, Constantes.pageOperaciones);

                        await NavigationService.NavigateAsync(Constantes.pageTransfCtaOtroBancoDestino, navParameters);
                    }
                }

                OpeFrecSelected = null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ActualizarListaOpeFrecuentes()
        {
            ListaOpeFrecuentes = OperacionService.ListarOperacionesFrecuentes();
        }
    }
}
