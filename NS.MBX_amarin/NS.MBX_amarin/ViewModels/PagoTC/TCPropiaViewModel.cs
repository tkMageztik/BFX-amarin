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
	public class TCPropiaViewModel : ViewModelBase
	{
        private ITarjetaService TarjetaService { get; set; }

        public TCPropiaViewModel(ITarjetaService tarjetaService, INavigationService navigationService)
            : base(navigationService)
        {
            TarjetaService = tarjetaService;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;
            Lista = TarjetaService.ListarTarjetasCredito("");
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
            NavigationParameters parametros = GetNavigationParameters();
            parametros.Add("TCPropia", ItemSel);
            parametros.Add(Constantes.pageOrigen, Constantes.pageTCPropia);

            await NavigationService.NavigateAsync(Constantes.pagePagoTCDatos, parametros);

            ItemSel = null;
        }
    }
}
