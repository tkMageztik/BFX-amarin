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

        private User user { get; set; }
        private bool eraseNroTarjeta { get; set; }
        private bool eraseTipNroDoc { get; set; }

        public ObservableCollection
           <Grouping<string, User>> Users
        { get; set; }

        public MainPage()
        {
            InitializeComponent();
            //GetMainPage();
            LoadTipDoc();
            LoadUser();
            txtNroTarjeta.TextChanged += TxtNroTarjeta_OnChanged;
            picTipDoc.SelectedIndexChanged += TxtTipNroDoc_OnChanged;
            txtNroDoc.TextChanged += TxtTipNroDoc_OnChanged;

            UserRepository repository = new UserRepository();
            //repository.Delete();

        }

        private void TxtTipNroDoc_OnChanged (object sender, EventArgs args)
        {
            SaveTipNroDoc();
        }

        private void TxtNroTarjeta_OnChanged(object sender, EventArgs args)
        {
            SaveNroTarjeta();
        }

        private void LoadTipDoc()
        {
            List<string> lstTipDoc = new List<string>();
            lstTipDoc.Add("DNI");
            lstTipDoc.Add("Pasaporte");
            lstTipDoc.Add("CE");

            picTipDoc.ItemsSource = lstTipDoc;
            picTipDoc.SelectedItem = "DNI";
        }
        private void LoadUser()
        {
            UserRepository repository = new UserRepository();
            if (repository.Users != null && repository.Users.Count > 0)
            {
                user = repository.Users[0];

                if (user.NroTarjeta != null)
                {
                    swtNroTarjeta.Toggled -= SwtNroTarjeta_OnToggled;
                    swtNroTarjeta.IsToggled = true;
                    swtNroTarjeta.Toggled += SwtNroTarjeta_OnToggled;
                }
                txtNroTarjeta.Text = user.NroTarjeta;

                if (user.TipDoc != null || user.NroDoc != null)
                {
                    swtTipNroDoc.Toggled -= SwtTipNroDoc_OnToggled;
                    swtTipNroDoc.IsToggled = true;
                    swtTipNroDoc.Toggled += SwtTipNroDoc_OnToggled;
                }

                if (user.TipDoc != null)
                {
                    picTipDoc.SelectedItem = user.TipDoc;
                }
                if (user.NroDoc != null)
                {
                    txtNroDoc.Text = user.NroDoc;
                }
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
            user = repository.Users[0];
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
            if (args.Value)
            {
                SaveTipNroDoc();
            }
            else
            {
                EraseTipNroDoc();
                EraseDatabase();
            }
        }

        private void EraseDatabase()
        {
            if (eraseNroTarjeta && eraseTipNroDoc)
            {
                App.Database.DeleteItemAsync(user);
            }
        }

        private void SwtNroTarjeta_OnToggled(object sender, ToggledEventArgs args)
        {
            if (args.Value)
            {
                SaveNroTarjeta();
            }
            else
            {
                EraseNroTarjeta();
                EraseDatabase();
            }

        }

        public async Task SaveTipNroDoc()
        {
            if (user != null)
            {
                user.TipDoc = picTipDoc.SelectedItem.ToString();
                user.NroDoc = txtNroDoc.Text;
            }
            else
            {
                user = new User()
                {
                    TipDoc = picTipDoc.SelectedItem.ToString(),
                    NroDoc = txtNroDoc.Text
                };
            }

            await App.Database.SaveItemAsync(this.user);
        }

        public async Task EraseTipNroDoc()
        {
            if (user != null)
            {
                user.TipDoc = null;
                user.NroDoc = null;
            }
            //else
            //{
            //    user = new User()
            //    {
            //        TipDoc = picTipDoc.SelectedItem.ToString(),
            //        NroDoc = txtNroDoc.Text
            //    };
            //}
            eraseTipNroDoc = true;
            await App.Database.SaveItemAsync(this.user);
        }

        public async Task SaveNroTarjeta()
        {
            if (user != null)
            {
                user.NroTarjeta = txtNroTarjeta.Text;
            }
            else
            {
                user = new User()
                {
                    NroTarjeta = txtNroTarjeta.Text
                };
            }

            await App.Database.SaveItemAsync(this.user);
        }

        public async Task EraseNroTarjeta()
        {
            if (user != null)
            {
                user.NroTarjeta = null;
            }
            //else
            //{
            //    user = new User()
            //    {
            //        NroTarjeta = null
            //    };
            //}

            eraseNroTarjeta = true;
            await App.Database.SaveItemAsync(this.user);
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
