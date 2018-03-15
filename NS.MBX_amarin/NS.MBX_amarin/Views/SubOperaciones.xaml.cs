using NS.MBX_amarin.Model;
using NS.MBX_amarin.Services;
using NS.MBX_amarin.View;
using NS.MBX_amarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.Views
{
    public partial class SubOperaciones : ContentPage
    {
        public string idOpe;
        public bool origenMisCuentas;

        public SubOperaciones()
        {
            InitializeComponent();
            //string idOperacion = Application.Current.Properties["opeId"] as string;
            //bool origenMisCuentas = (bool)Application.Current.Properties["origenMisCuentas"];

            //this.origenMisCuentas = origenMisCuentas;
            //ObservableCollection<SubOperacion> lstCtas = ((SubOperacionesViewModel)BindingContext).ObtenerOperacionService().ListarSubOperaciones(idOperacion);
            //idOpe = idOperacion;


            //lsvCtas.ItemsSource = lstCtas;

            //lsvCtas.GestureRecognizers.Clear();
            //lsvCtas.GestureRecognizers.Add(new TapGestureRecognizer());

            //navBar.seleccionarBoton("1");
        }

        
    }
}
