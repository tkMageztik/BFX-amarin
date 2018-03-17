using NS.MBX_amarin.Data;
using NS.MBX_amarin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism;
using Prism.Ioc;
using Prism.Autofac;

using Xamarin.Forms;
using NS.MBX_amarin.Views;
using NS.MBX_amarin.Services.Impl;
using Acr.UserDialogs;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NS.MBX_amarin
{
    public partial class App : PrismApplication
    {
        private static UserDatabase bd;
        public static UserDatabase Database
        {
            get
            {
                if (bd is null)
                {
                    bd = new UserDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("mobilex.mbx"));
                    return bd;
                }
                return bd;
            }

        }

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ITipoCambioService tipoCambioService = new TipoCambioService();
            ICuentaService cuentaService = new CuentaService(tipoCambioService);
            containerRegistry.RegisterInstance<ITipoCambioService>(tipoCambioService);
            containerRegistry.RegisterInstance<ICuentaService>(cuentaService);
            containerRegistry.RegisterInstance<ICatalogoService>(new CatalogoService());
            containerRegistry.RegisterInstance<IOperacionService>(new OperacionService());

            containerRegistry.RegisterInstance<IUserDialogs>(UserDialogs.Instance);

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<NavBar>();
            containerRegistry.RegisterForNavigation<Cuentas>();
            containerRegistry.RegisterForNavigation<Operaciones>();
            containerRegistry.RegisterForNavigation<SubOperaciones>();
            containerRegistry.RegisterForNavigation<Empresa>();
            containerRegistry.RegisterForNavigation<BuscadorEmpresa>();
            containerRegistry.RegisterForNavigation<ServicioEmpresa>();
            containerRegistry.RegisterForNavigation<PagoServicioEmpresa>();
            containerRegistry.RegisterForNavigation<CtaCargo>();
            containerRegistry.RegisterForNavigation<ConfPagoServicioEmpresa>();
            containerRegistry.RegisterForNavigation<TipoTarjeta>();
            containerRegistry.RegisterForNavigation<DatosPagoTarjeta>();
            containerRegistry.RegisterForNavigation<ConfDatosPago>();
            containerRegistry.RegisterForNavigation<Consultas>();
            containerRegistry.RegisterForNavigation<PopUpOperaciones>();
            containerRegistry.RegisterForNavigation<CtaDestino>();
            containerRegistry.RegisterForNavigation<Transferencia>();
            containerRegistry.RegisterForNavigation<Registro>();
            containerRegistry.RegisterForNavigation<GenericTextScrollView>();
            containerRegistry.RegisterForNavigation<RecargaCelular>();
            containerRegistry.RegisterForNavigation<RecargaBim>();
            containerRegistry.RegisterForNavigation<OpcionesAdicionales>();
            containerRegistry.RegisterForNavigation<MiPerfil>();
        }

        //public App()
        //{
        //    InitializeComponent();
        //    //MainPage = new MainPage();
        //    var navPage = new NavigationPage(new MainPage());
        //    navPage.BarBackgroundColor = Color.Blue;
        //    MainPage = navPage;
        //}

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
