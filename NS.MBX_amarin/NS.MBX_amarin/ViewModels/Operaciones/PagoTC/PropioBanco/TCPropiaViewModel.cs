using NS.MBX_amarin.BusinessLogic;
using NS.MBX_amarin.Common;
using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

//PERMITE ELEGIR UNA DE MIS PROPIAS TARJETAS
namespace NS.MBX_amarin.ViewModels
{
	public class TCPropiaViewModel : ViewModelBase
	{
        private ITarjetaService TarjetaService { get; set; }

        private string UltimoDiaPago;

        private string PagoMinSol;
        private string PagoTotSol;
        private string PagoDiaSol;
        private string PagoMinDol;
        private string PagoTotDol;
        private string PagoDiaDol;

        public TCPropiaViewModel(ITarjetaService tarjetaService, INavigationService navigationService)
            : base(navigationService)
        {
            TarjetaService = tarjetaService;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;
            Lista = TarjetaService.ListarTarjetasCredito("");

            string pageOrigen = RefNavParameters[Constantes.pageOrigen] as string;

            if (pageOrigen == Constantes.pageOperaciones)//es una operacion frecuente
            {
                OperacionFrecuente opeFrec = RefNavParameters[Constantes.keyOperacionFrecuente] as OperacionFrecuente;
                //se agregan las referencias que deberian existir hasta este punto
                RefNavParameters.Add(Constantes.keyCtaCargo, opeFrec.CtaOrigen);
                RefNavParameters.Add(Constantes.keyOrigenTarjeta, opeFrec.OrigenTarjeta);
            }
        }

        private ObservableCollection<Tarjeta> _lista;
        public ObservableCollection<Tarjeta> Lista
        {
            get { return _lista; }
            set { SetProperty(ref _lista, value); }
        }

        private Tarjeta _itemSel;
        public Tarjeta ItemSel
        {
            get { return _itemSel; }
            set { SetProperty(ref _itemSel, value); }
        }

        private DelegateCommand _itemTappedIC;
        public DelegateCommand ItemTappedIC =>
            _itemTappedIC ?? (_itemTappedIC = new DelegateCommand(ExecuteItemTappedIC));

        async void ExecuteItemTappedIC()
        {
            try
            {
                string msj = ValidarTarjeta();
                if (msj != "")
                {
                    await DialogService.DisplayAlertAsync("Validación", msj, "OK");
                    return;
                }
                
                NavigationParameters parametros = GetNavigationParameters();
                parametros.Add(Constantes.keyTCPropia, ItemSel);
                parametros.Add(Constantes.pageOrigen, Constantes.pageTCPropia);

                parametros.Add("TCUltimoDiaPago", UltimoDiaPago);
                parametros.Add("TCPagoMinSol", PagoMinSol);
                parametros.Add("TCPagoTotSol", PagoTotSol);

                await NavigationService.NavigateAsync(Constantes.pagePagoTCPropiaDatos, parametros);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            ItemSel = null;
        }

        private string ValidarTarjeta()
        {
            string _strError = "";
            string mensajeRpta = "";

            string strTarjeta = ItemSel.NroTarjeta;

            string strMensaje = "%" + strTarjeta.PadRight(19, ' ') + '%';
            TransaccionesMBX tx = new TransaccionesMBX();
            DataSet dsSalida = tx.EjecutarTransaccion(ListaTransacciones.PagoTarjetaBancoFinancieroConsulta, 103, strMensaje, ListaTransacciones.NombreMensajeOut(), ListaTransacciones.PosicionInicialCorte(), out _strError);
            //base.InsertaLogGeneral(TipoTransaccion.Consultas(), TipoOperacion.Consulta(), DefaultValues.PagoTarjetaBancoFinancieroConsulta, strMensaje, dsSalida, _strError);

            if (_strError != "0000")
            {
                try
                {
                    mensajeRpta = "La tarjeta ingresada no existe.";
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(int.Parse(_strError)), ListImages.Error);
                }
                catch (Exception)
                {
                    //MostrarMensaje("", ErrorManager.ObtenerMensajeSistema(6666), ListImages.Error);
                }
                //return false;
            }

            DataRow drSalida = dsSalida.Tables["OData"].Rows[0];
            if (true)
            //if (IsTCPropia)
            {

                //((Label)Step2.ContentTemplateContainer.FindControl("lbfFechaHoy")).Text = "Resumen al " + DateTime.Now.ToString("dd/MM/yyyy");
                //((Label)Step2.ContentTemplateContainer.FindControl("lblEtiquetaTarjeta")).Text = "Número de tarjeta";
                //((Label)Step2.ContentTemplateContainer.FindControl("lblTarjeta")).Text = ddlTarjetaPropia.SelectedItem.Text;
                //((Label)Step2.ContentTemplateContainer.FindControl("lblEtiquetaDiaPago")).Text = "Ultimo día de pago";
                UltimoDiaPago = drSalida["ODfepag"].ToString().Substring(6, 2) + '/' + drSalida["ODfepag"].ToString().Substring(4, 2) + '/' + drSalida["ODfepag"].ToString().Substring(0, 4);
                PagoMinSol = "50";// Comunes.FormateaNumero(drSalida["ODfill9"].ToString() + drSalida["ODpmina"].ToString());
                PagoTotSol = "150";// Comunes.FormateaNumero(drSalida["ODfill7"].ToString() + drSalida["ODpfula"].ToString());
                PagoDiaSol = Comunes.FormateaNumero(drSalida["ODfill2"].ToString() + drSalida["ODcreda"].ToString());
                PagoMinDol = Comunes.FormateaNumero(drSalida["ODfill6"].ToString() + drSalida["ODpminb"].ToString());
                PagoTotDol = Comunes.FormateaNumero(drSalida["ODfill4"].ToString() + drSalida["ODpfulb"].ToString());
                PagoDiaDol = Comunes.FormateaNumero(drSalida["ODfill1"].ToString() + drSalida["ODcredb"].ToString());

                //((GlosaFooter)Step2.ContentTemplateContainer.FindControl("gloBody2")).Glosa = "*&nbsp;&nbsp;La descripción a la que se asocie esta transacción aparecerá en el estado de cuenta mensual. <br/>**&nbsp;Pagos inferiores, iguales o en exceso al Monto Mínimo (MM) serán imputados según Reglamento de Tarjetas de Crédito.<br />El cliente podrá elegir una forma de imputación distinta únicamente para los pagos en exceso al MM comunicándose inmediatamente a los teléfonos 612-2222 (Lima) ó al 0-801-00222 (Provincias). <br/>Mayor información en www.financiero.com.pe";
            }
            else
            {
                //trDatosPagoMes.Style.Add("display", "none");
                //((Label)Step2.ContentTemplateContainer.FindControl("lblEtiquetaTarjeta")).Text = "Tipo y número";
                //((Label)Step2.ContentTemplateContainer.FindControl("lblTarjeta")).Text = drSalida["ODclstj"].ToString() + ' ' + ddlCuentaDestino.Text;
                //((Label)Step2.ContentTemplateContainer.FindControl("lblEtiquetaDiaPago")).Text = "Titular de la tarjeta";
                UltimoDiaPago = drSalida["ODnomtj"].ToString();

                //((GlosaFooter)Step2.ContentTemplateContainer.FindControl("gloBody2")).Glosa = "*&nbsp;&nbsp;La descripción a la que se asocie esta transacción aparecerá en el estado de cuenta mensual.";
            }

            return mensajeRpta;
        }
    }
}
