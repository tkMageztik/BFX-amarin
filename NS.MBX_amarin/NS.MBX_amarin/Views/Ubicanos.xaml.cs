using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NS.MBX_amarin.Views
{
    public partial class Ubicanos : ContentPage
    {
        public Ubicanos()
        {
            InitializeComponent();

            Pin _pin1 = new Pin()
            {
                Type = PinType.Place,
                Label = "Banco Financiero",
                Address = "Av. Ricardo Palma, Lima, Peru",
                Position = new Position(-12.1193605d, -77.0278707d)
            };

            map.Pins.Add(_pin1);
        }
    }
}
