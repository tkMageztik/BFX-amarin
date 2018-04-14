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
using Xamarin.Forms;

namespace NS.MBX_amarin.ViewModels
{
	public class SubOperacionesViewModel : ViewModelBase
	{
        private IOperacionService OperacionService { get; set; }

        public Operacion Operacion;
        private string IdOperacion;
        public bool OrigenMisCuentas;

        public SubOperacionesViewModel(IOperacionService operacionService, INavigationService navigationService)
            : base(navigationService)
        {
            this.OperacionService = operacionService;
        }

        private ObservableCollection<SubOperacion> _listaSuboperaciones;
        public ObservableCollection<SubOperacion> ListaSuboperaciones
        {
            get { return _listaSuboperaciones; }
            set { SetProperty(ref _listaSuboperaciones, value); }
        }

        private SubOperacion _subopeSelected;
        public SubOperacion SubopeSelected
        {
            get { return _subopeSelected; }
            set { SetProperty(ref _subopeSelected, value); }
        }

        private DelegateCommand _subopeTappedIC;
        public DelegateCommand SubopeTappedIC =>
            _subopeTappedIC ?? (_subopeTappedIC = new DelegateCommand(ExecuteSubopeTappedIC));

        async void ExecuteSubopeTappedIC()
        {
            SubOperacion subope = new SubOperacion
            {
                Id = SubopeSelected.Id,
                IdOperacion = SubopeSelected.IdOperacion,
                Nombre = SubopeSelected.Nombre
            };

            NavigationParameters navParameters = new NavigationParameters();
            navParameters.Add("Operacion", Operacion);
            navParameters.Add("SubOperacion", subope);
            Application.Current.Properties["suboperacionActual"] = subope;

            //pagos
            if (Operacion.Id == "1")
            {
                //pago de servicios
                if (subope.Id == "0")
                {
                    await NavigationService.NavigateAsync(Constantes.pageEmpresa, navParameters);
                }
                else if (subope.Id == "1") //pago a institucion o empresa
                {
                    await NavigationService.NavigateAsync(Constantes.pageBuscadorEmpresa, navParameters);
                }
                else if (subope.Id == "2") //pago de tarjetas
                {
                    await NavigationService.NavigateAsync(Constantes.pageTipoTarjeta, navParameters);
                }
            }
            else if (Operacion.Id == "2")//recargas
            {
                //recarga de celular
                if (subope.Id == "0")
                {
                    await NavigationService.NavigateAsync(Constantes.pageRecargaCelular, navParameters);
                }
                else if (SubopeSelected.Id == "1")
                {
                    await NavigationService.NavigateAsync(Constantes.pageRecargaBim, navParameters);
                }
            }

            else if (Operacion.Id == "3")
            {
                navParameters.Add(Constantes.pageOrigen, Constantes.pageSubOperaciones);

                await NavigationService.NavigateAsync("CtaCargo", navParameters);
            }

            SubopeSelected = null;
        }

        //cuando se navega hacia aqui
        //los parametros que son objetos deben ser copias, no referencias 
        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;
            Operacion = parameters["Operacion"] as Operacion;
            IdOperacion = parameters["IdOperacion"] as string;

            ListaSuboperaciones = OperacionService.ListarSubOperaciones(Operacion.Id);
        }
    }

}

