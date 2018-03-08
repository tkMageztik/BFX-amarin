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
            containerRegistry.RegisterForNavigation<NavigationPage>();
            //containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<Test>();
            containerRegistry.RegisterForNavigation<Views.MainPage>();
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
