using Acr.UserDialogs;
using NS.MBX_amarin.Events;
using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModels
{
	public class ConfPagoServicioEmpresaViewModel : ViewModelBase
	{
        private ICuentaService CuentaService { get; set; }
        private IOperacionService OperacionService { get; set; }
        private IEventAggregator EventAggregator { get; set; }

        public ConfPagoServicioEmpresaViewModel(IOperacionService operacionService, ICuentaService cuentaService, INavigationService navigationService, IEventAggregator eventAggregator, IUserDialogs userDialogs)
            : base(navigationService, userDialogs)
        {
            this.CuentaService = cuentaService;
            this.OperacionService = operacionService;
            this.EventAggregator = eventAggregator;
        }

        private string _lblNombreCta;
        public string LblNombreCta
        {
            get { return _lblNombreCta; }
            set { SetProperty(ref _lblNombreCta, value); }
        }

        private string _lblCodCta;
        public string LblCodCta
        {
            get { return _lblCodCta; }
            set { SetProperty(ref _lblCodCta, value); }
        }

        private string _lblEmpresa;
        public string LblEmpresa
        {
            get { return _lblEmpresa; }
            set { SetProperty(ref _lblEmpresa, value); }
        }

        private string _lblServicio;
        public string LblServicio
        {
            get { return _lblServicio; }
            set { SetProperty(ref _lblServicio, value); }
        }

        private string _lblCodigo;
        public string LblCodigo
        {
            get { return _lblCodigo; }
            set { SetProperty(ref _lblCodigo, value); }
        }

        private string _lblMonedaMonto;
        public string LblMonedaMonto
        {
            get { return _lblMonedaMonto; }
            set { SetProperty(ref _lblMonedaMonto, value); }
        }

        private string _clave;
        public string Clave
        {
            get { return _clave; }
            set { SetProperty(ref _clave, value); }
        }

        private bool _isOperacionFrecuente;
        public bool IsOperacionFrecuente
        {
            get { return _isOperacionFrecuente; }
            set { SetProperty(ref _isOperacionFrecuente, value); }
        }

        private DelegateCommand _accionCompletarIC;
        public DelegateCommand AccionCompletarIC =>
            _accionCompletarIC ?? (_accionCompletarIC = new DelegateCommand(ExecuteAccionCompletarIC));

        async void ExecuteAccionCompletarIC()
        {
            try
            {
                if (string.IsNullOrEmpty(Clave))
                {
                    await UserDialogs.AlertAsync("Ingrese una clave válida", "Mensaje",  "OK");
                    return;
                }

                Cuenta cta = RefNavParameters["CtaCargo"] as Cuenta;
                string montoStr = RefNavParameters["monto"] as string;
                decimal monto = decimal.Parse(montoStr);

                string rpta = CuentaService.efectuarMovimiento(cta, monto, "PEN", false);

                if (rpta != "")
                {
                    await UserDialogs.AlertAsync(rpta, Constantes.MSJ_INFO,  Constantes.MSJ_BOTON_OK);
                }
                else
                {
                    if (IsOperacionFrecuente)
                    {
                        //RefNavParameters["suboperacionActual"] as SubOperacion;
                        SubOperacion subope = RefNavParameters["SubOperacion"] as SubOperacion;
                        Catalogo empresa = RefNavParameters["Empresa"] as Catalogo;
                        OperacionFrecuente opeFrec = new OperacionFrecuente
                        {
                            FechaOperacion = DateTime.Now,
                            SubOperacion = subope,
                            Operacion = OperacionService.BuscarOperacion(subope.IdOperacion),
                            Servicio = RefNavParameters["Servicio"] as Servicio,
                            NombreFrecuente = subope.Nombre + ": " + empresa.Nombre
                        };

                        OperacionService.AgregarOperacionFrecuente(opeFrec);
                        EventAggregator.GetEvent<OpeFrecuenteAddedEvent>().Publish();
                    }
                    await UserDialogs.AlertAsync(Constantes.msjExito, "Mensaje",  "OK");
                    await NavigationService.GoBackToRootAsync();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;

            Cuenta cta = RefNavParameters["CtaCargo"] as Cuenta;
            LblCodCta = cta.CodigoCta;
            LblNombreCta = cta.NombreCta;

            LblEmpresa = RefNavParameters["nombreEmpresa"] as string;
            LblServicio = RefNavParameters["nombreServicio"] as string;
            LblCodigo = RefNavParameters["codigo"] as string;

            LblMonedaMonto = "Moneda y monto: S/ " + (string)RefNavParameters["monto"];
        }

        public ICuentaService ObtenerCuentaService()
        {
            return CuentaService;
        }

        public IOperacionService ObtenerOperacionService()
        {
            return OperacionService;
        }

        public async Task RetornarInicio()
        {
            await NavigationService.GoBackToRootAsync();
        }

        public void PublicarEventoOpeFrecuente()
        {
            EventAggregator.GetEvent<OpeFrecuenteAddedEvent>().Publish();
        }
    }
}
