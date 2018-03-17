using Acr.UserDialogs;
using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
	public class MiPerfilViewModel : ViewModelBase
	{
        private ICatalogoService CatalogoService { get; set; }
        private Cliente Cliente;

        public MiPerfilViewModel(ICatalogoService catalogoService, INavigationService navigationService, IUserDialogs userDialogs)
            : base(navigationService, userDialogs)
        {
            CatalogoService = catalogoService;
        }

        private string _txtBtnEditar;
        public string TxtBtnEditar
        {
            get { return _txtBtnEditar; }
            set { SetProperty(ref _txtBtnEditar, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private bool _isEdicion;
        public bool IsEdicion
        {
            get { return _isEdicion; }
            set { SetProperty(ref _isEdicion, value); }
        }

        private string _numCelular;
        public string NumCelular
        {
            get { return _numCelular; }
            set { SetProperty(ref _numCelular, value); }
        }

        private bool _isAceptaDatos;
        public bool IsAceptaDatos
        {
            get { return _isAceptaDatos; }
            set { SetProperty(ref _isAceptaDatos, value); }
        }

        private DelegateCommand _tapDatosPersIC;
        public DelegateCommand TapDatosPersIC =>
            _tapDatosPersIC ?? (_tapDatosPersIC = new DelegateCommand(ExecuteTapDatosPersIC));

        async void ExecuteTapDatosPersIC()
        {
            await UserDialogs.AlertAsync("Estas son las condiciones que debes aceptar.", "Tratamiento de datos personales", Constantes.MSJ_BOTON_ACEPTAR);
        }

        private DelegateCommand _editarIC;
        public DelegateCommand EditarIC =>
            _editarIC ?? (_editarIC = new DelegateCommand(ExecuteEditarIC));

        private DelegateCommand _tapInfoEmailIC;
        public DelegateCommand TapInfoEmailIC =>
            _tapInfoEmailIC ?? (_tapInfoEmailIC = new DelegateCommand(ExecuteTapInfoEmailIC));

        async void ExecuteTapInfoEmailIC()
        {
            await UserDialogs.AlertAsync("Enviaremos las constancias de tus operaciones al e-mail registrado.", Constantes.MSJ_INFO, Constantes.MSJ_BOTON_ACEPTAR);
        }

        async void ExecuteEditarIC()
        {
            if (IsEdicion)
            {
                if (!IsAceptaDatos)
                {
                    await UserDialogs.AlertAsync("Para guardar los datos, debes leer y aceptar el tratamiento de datos personales.", Constantes.MSJ_INFO, Constantes.MSJ_BOTON_ACEPTAR);
                }
                else
                {
                    IsEdicion = false;
                    TxtBtnEditar = "Editar";
                    Cliente.Email = Email;
                    Cliente.Celular = NumCelular;
                }
                
            }
            else
            {
                IsEdicion = true;
                TxtBtnEditar = "Ok";
            }
        }

        private DelegateCommand _tapInfoCelularIC;
        public DelegateCommand TapInfoCelularIC =>
            _tapInfoCelularIC ?? (_tapInfoCelularIC = new DelegateCommand(ExecuteTapInfoCelularIC));

        async void ExecuteTapInfoCelularIC()
        {
            await UserDialogs.AlertAsync("Para modificar tu número afiliado a la Clave SMS, debes realizarlo mediante un cajero Global Net.", Constantes.MSJ_INFO, Constantes.MSJ_BOTON_ACEPTAR);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            RefNavParameters = parameters;

            IsEdicion = false;
            TxtBtnEditar = "Editar";

            Cliente = CatalogoService.ObtenerCliente();
            NumCelular = Cliente.Celular;
            Email = Cliente.Email;
        }
    }
}
