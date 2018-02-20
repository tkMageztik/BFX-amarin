using NS.MBX_amarin.Model;
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
	public partial class OperacionesView : ContentPage
	{
		public OperacionesView ()
		{
			InitializeComponent ();

            Title = "Operaciones";

            List<Operacion> lstCtas = new List<Operacion>
            {
                new Operacion { Id= "1", Nombre = "Pagos" },
                new Operacion { Id= "2", Nombre = "Recargas" },
                new Operacion { Id= "3", Nombre = "Transferencias" }
            };

            lsvCtas.ItemsSource = lstCtas;

            lsvCtas.GestureRecognizers.Clear();
            lsvCtas.GestureRecognizers.Add(new TapGestureRecognizer());
        }

        async void LsvCtas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            Operacion ope = e.Item as Operacion;
            
            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            await Navigation.PushAsync(new SubOperacionesView(ope.Id));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}