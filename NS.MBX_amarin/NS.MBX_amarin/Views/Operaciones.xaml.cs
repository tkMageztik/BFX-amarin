using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using NS.MBX_amarin.View;
using NS.MBX_amarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.Views
{
    public partial class Operaciones : ContentPage
    {
        public Operaciones()
        {
            InitializeComponent();

            lsvCtas.ItemsSource = ((OperacionesViewModel)BindingContext).ObtenerOperacionService().ListarOperaciones();
            lsvCtas.GestureRecognizers.Clear();
            lsvCtas.GestureRecognizers.Add(new TapGestureRecognizer());

            lsvOpeFrecuentes.ItemsSource = ((OperacionesViewModel)BindingContext).ObtenerOperacionService().ListarSuboperacionesFrecuentes();
            lsvOpeFrecuentes.GestureRecognizers.Clear();
            lsvOpeFrecuentes.GestureRecognizers.Add(new TapGestureRecognizer());

        }

        async void LsvCtas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            Operacion ope = e.Item as Operacion;

            Application.Current.Properties["opeId"] = ope.Id;
            Application.Current.Properties["origenMisCuentas"] = false;
            await ((OperacionesViewModel)BindingContext).NavegarSuboperaciones();
            //await Navigation.PushAsync(new SubOperacionesView(ope.Id, false), false);

            ((ListView)sender).SelectedItem = null;
        }

        async void LsvOpeFrecuentes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            SubOperacion subope = e.Item as SubOperacion;

            Application.Current.Properties["empresa"] = ((OperacionesViewModel)BindingContext).ObtenerCatalogoService().BuscarEmpresaConServicios(subope.ServicioFrecuente.IdEmpresa);
            Application.Current.Properties["servicio"] = subope.ServicioFrecuente;
            Application.Current.Properties["pageOrigen"] = "OperacionesView";
            await ((OperacionesViewModel)BindingContext).Navegar("ServicioEmpresa");
            //await Navigation.PushAsync(new ServicioEmpresaView(), false);

            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            lsvOpeFrecuentes.ItemsSource = null;

            Device.BeginInvokeOnMainThread(() =>
            {
                lsvOpeFrecuentes.ItemsSource = ((OperacionesViewModel)BindingContext).ObtenerOperacionService().ListarSuboperacionesFrecuentes();

            });

        }
    }
}
