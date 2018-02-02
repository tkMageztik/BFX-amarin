using NS.MBX_amarin.Data;
using NS.MBX_amarin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace NS.MBX_amarin
{
    public partial class App : Application
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

        public App()
        {
            InitializeComponent();
            //MainPage = new MainPage();
            var navPage = new NavigationPage(new MainPage());
            MainPage = navPage;
        }

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
