using NS.MBX_amarin.Helpers;
using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NS.MBX_amarin
{
    public partial class MainPage : ContentPage
    {

        private User User { get; set; }
        public ObservableCollection
           <Grouping<string, User>> Users
        { get; set; }

        public MainPage()
        {
            InitializeComponent();
            //GetMainPage();
            LoadUser();

        }
        private void LoadUser()
        {

            UserRepository repository = new UserRepository();
            if (repository.Users != null && repository.Users.Count > 0)
            {
                User = repository.Users[0];

                txtNroTarjeta.Text = User.NroTarjeta;
                picTipDoc.SelectedItem = User.TipDoc;
                txtNroDoc.Text = User.NroDoc;
            };
        }


        public async Task GetUser()
        {
            UserRepository repository = new UserRepository();
            //Users =
            //    User usr = repository.GetAllGrouped()[0];

            //if (Users[0] != null)
            //{
            //    txtNroTarjeta.Text = U
            //}
            User = repository.Users[0];
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

        private void SwtTipNroDoc_OnToggled(object sender, ToggledEventArgs args)
        {
            SaveUser();

        }

        private void SwtNroTarjeta_OnToggled(object sender, ToggledEventArgs args)
        {

        }
        public async Task SaveUser()
        {
            if (User != null)
            {
                User.NroTarjeta = txtNroTarjeta.Text;
                User.TipDoc = picTipDoc.SelectedItem.ToString();
                User.NroDoc = txtNroDoc.Text;
            }
            else
            {
                User = new User()
                {
                    NroTarjeta = txtNroTarjeta.Text,
                    TipDoc = picTipDoc.SelectedItem.ToString(),
                    NroDoc = txtNroDoc.Text
                };
            }

            await App.Database.SaveItemAsync(this.User);
        }

        private void BtnRegistrar_OnClicked(object sender, EventArgs args)
        {
            //var navPage = new NavigationPage(new Registro());

            Navigation.PushAsync(new Registro());
            //App.Current.MainPage = new NavigationPage(new Registro);
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
