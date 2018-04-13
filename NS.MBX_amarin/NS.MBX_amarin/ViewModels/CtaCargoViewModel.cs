using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModels
{
	public class CtaCargoViewModel : ViewModelBase
	{
        private ICuentaService CuentaService { get; set; }

        public CtaCargoViewModel(ICuentaService cuentaService, INavigationService navigationService)
            : base(navigationService)
        {
            this.CuentaService = cuentaService;
        }

        private ObservableCollection<Cuenta> _listaCuentas;
        public ObservableCollection<Cuenta> ListaCuentas
        {
            get { return _listaCuentas; }
            set { SetProperty(ref _listaCuentas, value); }
        }

        private Cuenta _ctaSelected;
        public Cuenta CtaSelected
        {
            get { return _ctaSelected; }
            set { SetProperty(ref _ctaSelected, value); }
        }

        private DelegateCommand _ctaTappedIC;
        public DelegateCommand CtaTappedIC =>
            _ctaTappedIC ?? (_ctaTappedIC = new DelegateCommand(ExecuteCtaTappedIC));

        async void ExecuteCtaTappedIC()
        {
            NavigationParameters parametros = ObtenerNavParametros();
            parametros.Add("CtaCargo", CtaSelected);

            string pageOrigen = parametros[Constantes.pageOrigen] as string;
            string pageDestino = string.Empty;

            if(pageOrigen == "PagoServicioEmpresaView")
            {
                pageDestino = Constantes.pageConfPagoServicioEmpresa;
            }
            else if (pageOrigen == Constantes.pageTipoTarjeta)
            {
                pageDestino = Constantes.pageDatosPagoTarjeta;
            }
            else if (pageOrigen == Constantes.pageRecargaCelular)
            {
                pageDestino = Constantes.pageConfDatosPago;
            }
            else if (pageOrigen == Constantes.pageRecargaBim)
            {
                pageDestino = Constantes.pageConfDatosPago;
            }
            else if (pageOrigen == Constantes.pageOperaciones)
            {
                pageDestino = Constantes.pageDatosPagoTarjeta;

            }
            else if (pageOrigen == Constantes.pageSubOperaciones)//posible transferencia
            {
                Operacion operacion = parametros["Operacion"] as Operacion;
                SubOperacion suboperacion = parametros["SubOperacion"] as SubOperacion;

                if(operacion.Id == "3")
                {
                    if(suboperacion.Id == "0")
                    {
                        pageDestino = Constantes.pageTransfCtaTerceroDestino;
                    }
                    else if(suboperacion.Id == "1")
                    {
                        pageDestino = Constantes.pageTransfCuentaOtroBanco;
                    }
                    else if(suboperacion.Id == "2")
                    {
                        pageDestino = Constantes.pageTransfCtaPropiaDestino;
                    }
                }
            }
            else
            {
            }

            await NavigationService.NavigateAsync(pageDestino, parametros);

            CtaSelected = null;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;

            ListaCuentas = CuentaService.listaCuentas;
        }

        public async Task Navegar(string ruta)
        {
            await NavigationService.NavigateAsync(ruta);
        }

        public NavigationParameters ObtenerNavParametros()
        {
            return GetNavigationParameters(true, null);
        }

        public async Task Navegar(string ruta, NavigationParameters navParameters)
        {
            await NavigationService.NavigateAsync(ruta, navParameters);
        }
    }
}
