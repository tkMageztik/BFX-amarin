using Acr.UserDialogs;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NS.MBX_amarin.ViewModels
{
    public class PagoServicioEmpresaViewModel : ViewModelBase
    {
        private ITipoCambioService TipoCambioService { get; set; }

        public PagoServicioEmpresaViewModel(ITipoCambioService tipoCambioService, INavigationService navigationService, IUserDialogs userDialogs)
            : base(navigationService, userDialogs)
        {
            TipoCambioService = tipoCambioService;
        }

        private string _lblServicio;
        public string LblServicio
        {
            get { return _lblServicio; }
            set { SetProperty(ref _lblServicio, value); }
        }

        private string _lblEmpresa;
        public string LblEmpresa
        {
            get { return _lblEmpresa; }
            set { SetProperty(ref _lblEmpresa, value); }
        }

        private string _lblCodigoCliente;
        public string LblCodigoCliente
        {
            get { return _lblCodigoCliente; }
            set { SetProperty(ref _lblCodigoCliente, value); }
        }

        private string _monto;
        public string Monto
        {
            get { return _monto; }
            set { SetProperty(ref _monto, value); }
        }

        private string _lblTipoCambio;
        public string LblTipoCambio
        {
            get { return _lblTipoCambio; }
            set { SetProperty(ref _lblTipoCambio, value); }
        }

        private DelegateCommand _siguienteIC;
        public DelegateCommand SiguienteIC =>
            _siguienteIC ?? (_siguienteIC = new DelegateCommand(ExecuteSiguienteIC));

        async void ExecuteSiguienteIC()
        {
            try
            {
                int mont = 0;
                if (!int.TryParse(Monto, out mont))
                {
                    await UserDialogs.AlertAsync("Ingrese un monto válido", "Mensaje", "OK");
                    return;
                }
                else if (mont <= 0)
                {
                    await UserDialogs.AlertAsync("Ingrese un monto válido", "Mensaje", "OK");
                    return;
                }

                NavigationParameters parametros = GetNavigationParameters();
                parametros.Add("nombreEmpresa", LblEmpresa);
                parametros.Add("nombreServicio", LblServicio);
                parametros.Add("codigo", LblCodigoCliente);
                parametros.Add("monto", Monto);

                Application.Current.Properties["strTipoTransf"] = "0";
                Application.Current.Properties["strOrigenMisCuentas"] = false;
                Application.Current.Properties["strPageOrigen"] = "PagoServicioEmpresaView";
                await NavigationService.NavigateAsync(Constantes.pageCtaCargo, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;

            LblEmpresa = RefNavParameters["stringEmpresa"] as string;
            LblServicio = RefNavParameters["stringPicServicio"] as string;
            LblCodigoCliente = RefNavParameters["stringCodigo"] as string;

            LblTipoCambio = TipoCambioService.ObtenerDescTipoCambio();
        }
    }
}
