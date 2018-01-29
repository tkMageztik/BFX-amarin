using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NS.BFX_amarin
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
        {
            InitializeComponent();
            //GetMainPage();
		}

        private void BtnIngresar_OnClicked(object sender,EventArgs args)
        {
            DisplayAlert("Mensaje", "Hola nena JM", "ok");
        }

        public static bool Luhn(string digits)
        {
            return digits.All(char.IsDigit) && digits.Reverse()
                .Select(c => c - 48)
                .Select((thisNum, i) => i % 2 == 0
                    ? thisNum
                    : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum)
                ).Sum() % 10 == 0;
        }

        //public static Page GetMainPage()
        //{


        //    var myLabel = new Label()
        //    {
        //        Text = "Hello World",
        //        FontSize = 20,
        //        TextColor = Color.White,
        //        HorizontalTextAlignment = TextAlignment.Center,
        //        VerticalTextAlignment = TextAlignment.Center
        //    };

        //    var myImage = new Image()
        //    {
        //        Source = FileImageSource.FromUri(
        //            new Uri("http://xamarin.com/content/images/pages/index/hero-slide.jpg"))
        //    };

        //    RelativeLayout layout = new RelativeLayout();

        //    layout.Children.Add(myImage,
        //        Constraint.Constant(0),
        //        Constraint.Constant(0), 
        //        Constraint.RelativeToParent((parent) => { return parent.Width; }),
        //        Constraint.RelativeToParent((parent) => { return parent.Height; }));

        //    layout.Children.Add(myLabel,
        //        Constraint.Constant(0),
        //        Constraint.Constant(0),
        //        Constraint.RelativeToParent((parent) => { return parent.Width; }),
        //        Constraint.RelativeToParent((parent) => { return parent.Height; }));

        //    return new ContentPage
        //    {
        //        Content = layout
        //    };
        //}
    }
}
