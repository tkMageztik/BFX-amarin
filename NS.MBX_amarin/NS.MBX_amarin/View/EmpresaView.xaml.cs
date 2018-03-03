using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
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
	public partial class EmpresaView : ContentPage
	{
		public EmpresaView ()
		{
			InitializeComponent ();

            lsvData.ItemsSource = CatalogoService.ListarEmpresas();

        }

        async void EventoItemSeleccionado(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            Application.Current.Properties["empresa"] = e.Item as Catalogo;

            await Navigation.PushAsync(new ServicioEmpresaView(), false);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}