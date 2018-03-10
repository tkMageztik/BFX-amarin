using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenericTextScrollView : ContentPage
    {
        public GenericTextScrollView(string subtitulo, string texto, string textoBoton)
        {
            InitializeComponent();
            lblSubtitulo.Text = subtitulo;
            lblTexto.Text = texto;
            btn.Text = textoBoton;
            //TOD: incluir un delegate para el comportamiento del botón.
        }

        private void Btn_OnClicked(object sender, EventArgs args)
        {
            Navigation.PopAsync();
        }
    }
}