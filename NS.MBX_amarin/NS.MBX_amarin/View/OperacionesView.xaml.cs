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
	public partial class OperacionesView : ContentPage
	{
		public OperacionesView ()
		{
			InitializeComponent ();

            lsvCtas.ItemsSource = OperacionService.ListarOperaciones();
            lsvCtas.GestureRecognizers.Clear();
            lsvCtas.GestureRecognizers.Add(new TapGestureRecognizer());

            lsvOpeFrecuentes.ItemsSource = OperacionService.ListarSuboperacionesFrecuentes();
            lsvOpeFrecuentes.GestureRecognizers.Clear();
            lsvOpeFrecuentes.GestureRecognizers.Add(new TapGestureRecognizer());

        }

        async void LsvCtas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            Operacion ope = e.Item as Operacion;
            

            await Navigation.PushAsync(new SubOperacionesView(ope.Id, false), false);
            
            ((ListView)sender).SelectedItem = null;
        }

        async void LsvOpeFrecuentes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            SubOperacion subope = e.Item as SubOperacion;

            Application.Current.Properties["empresa"] = CatalogoService.BuscarEmpresa(subope.ServicioFrecuente.IdEmpresa);
            Application.Current.Properties["servicio"] = subope.ServicioFrecuente;
            Application.Current.Properties["pageOrigen"] = "OperacionesView";
            await Navigation.PushAsync(new ServicioEmpresaView(), false);
            
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            lsvOpeFrecuentes.ItemsSource = null;

            Device.BeginInvokeOnMainThread(() =>
            {
                lsvOpeFrecuentes.ItemsSource = OperacionService.ListarSuboperacionesFrecuentes();

            });

        }
    }
}
