using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicioEmpresaView : ContentPage
    {
        public ServicioEmpresaView()
        {
            InitializeComponent();

            
            Catalogo empresa = Application.Current.Properties["empresa"] as Catalogo;
            string pageOrigen = Application.Current.Properties["pageOrigen"] as string;
            lblEmpresa.Text = empresa.Nombre;

            ObservableCollection<Servicio> listaServicios = CatalogoService.ListarServiciosxEmpresa(empresa.Codigo);

            foreach (Servicio item in listaServicios)
            {
                picServicio.Items.Add(item.Nombre);
            }

            if(pageOrigen == "OperacionesView")
            {
                Servicio servicio = Application.Current.Properties["servicio"] as Servicio;
                picServicio.SelectedItem = servicio.Nombre;
            }

        }

        public async void EventoSiguiente(object sender, EventArgs args)
        {
            if(entCodigo.Text == null || entCodigo.Text == "")
            {
                await DisplayAlert("Mensaje", "Ingrese un código válido", "OK");
                return;
            }
            Catalogo empresa = Application.Current.Properties["empresa"] as Catalogo;
            ObservableCollection<Servicio> listaServicios = CatalogoService.ListarServiciosxEmpresa(empresa.Codigo);
            foreach(Servicio ser in listaServicios){
                if(ser.Nombre == picServicio.SelectedItem.ToString())
                {
                    Application.Current.Properties["servicio"] = ser;
                    break;
                }
            }
            
            await Navigation.PushAsync(new PagoServicioEmpresaView(lblEmpresa.Text, picServicio.SelectedItem.ToString(), entCodigo.Text), false);
        }
    }
}