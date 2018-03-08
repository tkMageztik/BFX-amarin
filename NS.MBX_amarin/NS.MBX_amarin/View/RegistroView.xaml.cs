using NS.MBX_amarin.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro : ContentPage
    {
        public Registro()
        {
            InitializeComponent();
            Title = "Registro";
            lblDatosPersonales.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnLblDatosPersonales_Clicked()),
            });
        }

        private void OnLblDatosPersonales_Clicked()
        {
            Navigation.PushAsync(new GenericTextScrollView("TRATAMIENTO DE DATOS PERSONALES",
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd" +
                "fdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfdfdfdfdsfdsfsdfd",
                "Aceptar") { Title = "Registro" });
        }
    }
}