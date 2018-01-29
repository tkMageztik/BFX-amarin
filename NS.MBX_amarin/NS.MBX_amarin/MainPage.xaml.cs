using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NS.MBX_amarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //GetMainPage();
        }

        //public async Task<ImageSource> GetImageFromStream(string url)
        //{ var resp = await obj_Client.GetStreamAsync(url); return Xamarin.Forms.ImageSource.FromStream(() => { return resp; }); }

        private void BtnIngresar_OnClicked(object sender, EventArgs args)
        {
            string msg = ValidarIngreso();
            if (msg == "")
            {
                DisplayAlert("Banco X", "En mantenimiento...", "Aceptar");
            }
            else { DisplayAlert("Banco X", msg, "Aceptar"); }
        }

        private void BtnRegistrar_OnClicked(object sender, EventArgs args)
        {
            //Navigation.PushAsync(new Registro());
            App.Current.MainPage = new NavigationPage(new Registro());
        }
        private void BtnContacto_OnClicked(object sender, EventArgs args)
        {
            DisplayAlert("Banco X", "En mantenimiento...", "Aceptar");

        }

        public static bool Luhn(string digits)
        {
            return digits.All(char.IsDigit) && digits.Reverse()
                .Select(c => c - 48)
                .Select((thisNum, i) => i % 2 == 0
                    ? thisNum
                    : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum)
                ).Sum() % 10 == 0;
        }

        private string ValidarIngreso()
        {
            if (txtNroTarjeta.Text == "" || txtNroTarjeta.Text is null) return "Por favor, ingresa tu número de tarjeta.";
            if (!Luhn(txtNroTarjeta.Text)) return "El número de tarjeta, no es válido.";
            if (txtNroDoc.Text == "" || txtNroDoc.Text is null) return "Por favor, ingrese su número de documento.";
            if (txtClaveWeb.Text == "" || txtClaveWeb.Text is null) return "Por favor, ingrese su clave web.";
            return "";

        }

        //public static Page GetMainPage()
        //{


        //    var myLabel = new Label()
        //    {
        //        Text = "Hello World",
        //        FontSize = 20,
        //        TextColor = Color.White,
        //        HorizontalTextAlignment = TextAlignment.Center,
        //        VerticalTextAlignment = TextAlignment.Center
        //    };

        //    var myImage = new Image()
        //    {
        //        Source = FileImageSource.FromUri(
        //            new Uri("http://xamarin.com/content/images/pages/index/hero-slide.jpg"))
        //    };

        //    RelativeLayout layout = new RelativeLayout();

        //    layout.Children.Add(myImage,
        //        Constraint.Constant(0),
        //        Constraint.Constant(0), 
        //        Constraint.RelativeToParent((parent) => { return parent.Width; }),
        //        Constraint.RelativeToParent((parent) => { return parent.Height; }));

        //    layout.Children.Add(myLabel,
        //        Constraint.Constant(0),
        //        Constraint.Constant(0),
        //        Constraint.RelativeToParent((parent) => { return parent.Width; }),
        //        Constraint.RelativeToParent((parent) => { return parent.Height; }));

        //    return new ContentPage
        //    {
        //        Content = layout
        //    };
        //}
    }
}
