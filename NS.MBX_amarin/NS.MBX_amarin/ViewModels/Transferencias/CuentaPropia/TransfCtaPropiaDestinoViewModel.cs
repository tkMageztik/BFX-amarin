using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class TransfCtaPropiaDestinoViewModel : ViewModelBase
	{
        private ICuentaService CuentaService { get; set; }

        public TransfCtaPropiaDestinoViewModel(ICuentaService cuentaService, INavigationService navigationService)
            : base(navigationService)
        {
            this.CuentaService = cuentaService;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;

            //logica para ocultar la cuenta actual 
            Cuenta cuentaOrigen = parameters["CtaCargo"] as Cuenta;

            ObservableCollection<Cuenta> listaCtas = CuentaService.listaCuentas;
            ObservableCollection<Cuenta> listaNueva = new ObservableCollection<Cuenta>();

            foreach (Cuenta cta in listaCtas)
            {
                if (cta.idCta != cuentaOrigen.idCta)
                {
                    listaNueva.Add(cta);
                }
            }

            ListaCuentas = listaNueva;
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
            NavigationParameters navParameters = GetNavigationParameters();
            navParameters.Add(Constantes.pageOrigen, Constantes.pageTransfCtaPropiaDestino);
            navParameters.Add("CtaDestino", CtaSelected);

            await NavigationService.NavigateAsync(Constantes.pageTransfCtaPropiaDatos, navParameters);

            CtaSelected = null;
        }

        void ValidarCuentas()
        {

        }
    }
}
