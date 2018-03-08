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
    public partial class Empresa : ContentPage
    {
        public Empresa()
        {
            InitializeComponent();

            lsvData.ItemsSource = CatalogoService.ListarEmpresasConServicios();

        }

        async void EventoItemSeleccionado(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            Application.Current.Properties["empresa"] = e.Item as Catalogo;

            await ((EmpresaViewModel)BindingContext).NavegarServicioEmpresa();
            //await Navigation.PushAsync(new ServicioEmpresaView(), false);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
