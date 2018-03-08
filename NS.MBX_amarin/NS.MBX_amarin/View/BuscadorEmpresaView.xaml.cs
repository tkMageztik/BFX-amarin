using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using NS.MBX_amarin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BuscadorEmpresaView : ContentPage
	{
		public BuscadorEmpresaView ()
		{
			InitializeComponent ();
            //BindingContext = new BuscadorEmpresaViewModel();

            lsvEmpresas.ItemsSource = null;
            lsvEmpresas.GestureRecognizers.Clear();
            lsvEmpresas.GestureRecognizers.Add(new TapGestureRecognizer());
        }

        public void SearchPressed(object sender, EventArgs args)
        {
            lsvEmpresas.ItemsSource = CatalogoService.BuscarEmpresa(sbBuscador.Text);
        }

        public async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            Catalogo cat = e.Item as Catalogo;


            //await Navigation.PushAsync(new SubOperacionesView(ope.Id, false), false);

            ((ListView)sender).SelectedItem = null;
        }
    }
}