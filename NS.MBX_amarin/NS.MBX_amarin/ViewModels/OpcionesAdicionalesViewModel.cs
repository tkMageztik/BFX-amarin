using Acr.UserDialogs;
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
	public class OpcionesAdicionalesViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }

        public OpcionesAdicionalesViewModel(IUserDialogs userDialogs, ICatalogoService catalogoService, INavigationService navigationService)
            : base(navigationService, userDialogs)
        {
            CatalogoService = catalogoService;

            
        }

        private ObservableCollection<Catalogo> _listaOpcAdicionales;
        public ObservableCollection<Catalogo> ListaOpcAdicionales
        {
            get { return _listaOpcAdicionales; }
            set { SetProperty(ref _listaOpcAdicionales, value); }
        }

        private Catalogo _opcSelected;
        public Catalogo OpcSelected
        {
            get { return _opcSelected; }
            set { SetProperty(ref _opcSelected, value); }
        }

        private DelegateCommand _opcTappedIC;
        public DelegateCommand OpcTappedIC =>
            _opcTappedIC ?? (_opcTappedIC = new DelegateCommand(ExecuteOpcTappedIC));

        void ExecuteOpcTappedIC()
        {

        }

        private DelegateCommand _accionLogoutIC;
        public DelegateCommand AccionLogoutIC =>
            _accionLogoutIC ?? (_accionLogoutIC = new DelegateCommand(ExecuteAccionLogoutIC));

        async void ExecuteAccionLogoutIC()
        {
            using (UserDialogs.Loading(""))
            {
                await Task.Delay(300);
                await NavigationService.NavigateAsync("app:///NavigationPage/MainPage");
            }
                
        }

        private DelegateCommand _accionBloquearIC;
        public DelegateCommand AccionBloquearIC =>
            _accionBloquearIC ?? (_accionBloquearIC = new DelegateCommand(ExecuteAccionBloquearIC));

        void ExecuteAccionBloquearIC()
        {

        }

        private DelegateCommand _accionConfigurarIC;
        public DelegateCommand AccionConfigurarIC =>
            _accionConfigurarIC ?? (_accionConfigurarIC = new DelegateCommand(ExecuteAccionConfigurarIC));

        void ExecuteAccionConfigurarIC()
        {

        }

        private DelegateCommand _accionViajeIC;
        public DelegateCommand AccionViajeIC =>
            _accionViajeIC ?? (_accionViajeIC = new DelegateCommand(ExecuteAccionViajeIC));

        void ExecuteAccionViajeIC()
        {

        }

        private DelegateCommand _accionCompartirIC;
        public DelegateCommand AccionCompartirIC =>
            _accionCompartirIC ?? (_accionCompartirIC = new DelegateCommand(ExecuteAccionCompartirIC));

        void ExecuteAccionCompartirIC()
        {

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            ListaOpcAdicionales = CatalogoService.ObtenerListaPorCodigo(CatalogoService.COD_OPC_ADICIONALES);
        }
    }
}
