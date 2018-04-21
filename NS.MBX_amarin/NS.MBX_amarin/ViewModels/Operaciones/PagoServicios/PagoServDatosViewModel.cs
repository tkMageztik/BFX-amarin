using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class PagoServDatosViewModel : ViewModelBase
    {
        private ICatalogoService CatalogoService { get; set; }
        private ICuentaService CuentaService { get; set; }
        private ITipoCambioService TipoCambioService { get; set; }

        public PagoServDatosViewModel(ICuentaService cuentaService, ITipoCambioService tipoCambioService, ICatalogoService catalogoService, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            CuentaService = cuentaService;
            CatalogoService = catalogoService;
            TipoCambioService = tipoCambioService;
        }

        //init
        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;

            string pageOrigen = RefNavParameters[Constantes.pageOrigen] as string;
            Recibo reciboIBS = RefNavParameters[Constantes.keyReciboIBS] as Recibo;
            Catalogo empresa = RefNavParameters[Constantes.keyEmpresa] as Catalogo;
            Servicio servicio = RefNavParameters[Constantes.keyServicio] as Servicio;
            string codigoServicio = RefNavParameters[Constantes.keyCodigoServicio] as string;

            LblNomServicio = servicio.Nombre;
            LblEmpresa = empresa.Nombre;
            LblCodigo = codigoServicio;
            LblNomCliente = reciboIBS.NombreCliente;

            ListaDetRecibos = reciboIBS.ListaDetalle;
            LblTipoCambio = TipoCambioService.ObtenerDescTipoCambio();

        }

        private string _lblNomServicio;
        public string LblNomServicio
        {
            get { return _lblNomServicio; }
            set { SetProperty(ref _lblNomServicio, value); }
        }

        private string _lblEmpresa;
        public string LblEmpresa
        {
            get { return _lblEmpresa; }
            set { SetProperty(ref _lblEmpresa, value); }
        }

        private string _lblCodigo;
        public string LblCodigo
        {
            get { return _lblCodigo; }
            set { SetProperty(ref _lblCodigo, value); }
        }

        private string _lblNomCliente;
        public string LblNomCliente
        {
            get { return _lblNomCliente; }
            set { SetProperty(ref _lblNomCliente, value); }
        }

        private string _lblFechaVencimiento;
        public string LblFechaVencimiento
        {
            get { return _lblFechaVencimiento; }
            set { SetProperty(ref _lblFechaVencimiento, value); }
        }

        private ObservableCollection<DetalleRecibo> _listaDetRecibos;
        public ObservableCollection<DetalleRecibo> ListaDetRecibos
        {
            get { return _listaDetRecibos; }
            set { SetProperty(ref _listaDetRecibos, value); }
        }

        private string _lblTipoCambio;
        public string LblTipoCambio
        {
            get { return _lblTipoCambio; }
            set { SetProperty(ref _lblTipoCambio, value); }
        }

        private DetalleRecibo _detReciboSel;
        public DetalleRecibo DetReciboSel
        {
            get { return _detReciboSel; }
            set { SetProperty(ref _detReciboSel, value); }
        }

        private DelegateCommand _reciboTappedIC;
        public DelegateCommand ReciboTappedIC =>
            _reciboTappedIC ?? (_reciboTappedIC = new DelegateCommand(ExecuteReciboTappedIC));

        void ExecuteReciboTappedIC()
        {
            try
            {
                LblFechaVencimiento = "Fecha de Vencimiento: " + DetReciboSel.FechaVencimiento;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private DelegateCommand _accionSigIC;
        public DelegateCommand AccionSigIC =>
            _accionSigIC ?? (_accionSigIC = new DelegateCommand(ExecuteAccionSigIC));

        async void ExecuteAccionSigIC()
        {
            try
            {
                string msj = ValidarCampos();
                if (msj != "")
                {
                    await DialogService.DisplayAlertAsync("Validación", msj, "OK");
                }
                else
                {

                    NavigationParameters navParametros = GetNavigationParameters();
                    navParametros.Add(Constantes.pageOrigen, Constantes.pagePagoServDatos);
                    navParametros.Add(Constantes.keyDetalleReciboIBS, DetReciboSel);

                    await NavigationService.NavigateAsync(Constantes.pageCtaCargo, navParametros);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string ValidarCampos()
        {
            string msj = "";
            Cuenta ctaOrigen = RefNavParameters[Constantes.keyCtaCargo] as Cuenta;
            Catalogo origenTarjeta = RefNavParameters[Constantes.keyOrigenTarjeta] as Catalogo;

            //if (string.IsNullOrEmpty(Monto))
            //{
            //    msj = "Ingrese un monto válido";
            //}
            //else 
            if (DetReciboSel == null)
            {
                msj = "Seleccione un recibo.";
            }
            //else if (!CuentaService.ValidarSaldoOperacion(ctaOrigen, decimal.Parse(Monto), Moneda.Codigo))
            //{
            //    msj = "La cuenta de origen no tiene saldo suficiente";
            //}
            //else
            //{
            //    if (origenTarjeta.Codigo == "0")//cuenta propio banco
            //    {
            //        msj = ValidarCuentasPropioBancoTercero();
            //    }
            //    else//cuenta otro banco
            //    {
            //        msj = ValidarCuentasOtroBanco();
            //    }

            //}

            return msj;
        }

    }
}
