using NS.MBX_amarin.View;
using BottomBar.XamarinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.Views
{
    public partial class NavBar
    {
        public NavBar()
        {
            InitializeComponent();

            Application.Current.Properties["tabActual"] = 0;
            //this.BarBackgroundColor = Color.Blue;
            //this.BarTextColor = Color.White;
            //this.FixedMode = true;
            string[] tabTitles = { "Mis ctas", "Operaciones", "Ubícanos", "Más" };
            //0000FF
            //005eaa
            string[] tabColors = { "#0000FF", "#0000FF", "#0000FF", "#0000FF" };
            // int[] tabBadgeCounts = { 0, 1, 5, 3, 4 };
            //string[] tabBadgeColors = { "#000000", "#FF0000", "#000000", "#0000FF" };

            foreach (Page page in this.Children)
            {
                BottomBarPageExtensions.SetTabColor(page, Color.FromHex(tabColors[0]));
            }
            //Page[] paginas = { new NavigationPage(new CuentasView()), new NavigationPage(new OperacionesView()), new CuentasView(), new OtrasOpcionesView() };
            //string[] imagenes = { "ctas1_2.png", "ope2_2.png", "ic_nearby_2.png", "menu_2.png" };

            //((NavigationPage)paginas[0]).BarBackgroundColor = Color.Blue;
            //((NavigationPage)paginas[1]).BarBackgroundColor = Color.Blue;

            //for (int i = 0; i < tabTitles.Length; ++i)
            //{
            //    string title = tabTitles[i];
            //    string tabColor = tabColors[i];
            //    // int tabBadgeCount = tabBadgeCounts[i];
            //    //string tabBadgeColor = tabBadgeColors[i];

            //    FileImageSource icon = (FileImageSource)FileImageSource.FromFile(imagenes[i]);

            //    // create tab page
            //    paginas[i].Title = tabTitles[i];
            //    paginas[i].Icon = icon;

            //    // set tab color
            //    if (tabColor != null)
            //    {
            //        BottomBarPageExtensions.SetTabColor(paginas[i], Color.FromHex(tabColor));
            //    }

            //    // Set badges
            //    //BottomBarPageExtensions.SetBadgeCount(paginas[i], tabBadgeCount);
            //    //BottomBarPageExtensions.SetBadgeColor(paginas[i], Color.FromHex(tabBadgeColor));


            //    // add tab pag to tab control
            //    this.Children.Add(paginas[i]);
            //}

            this.CurrentPageChanged += (object sender, EventArgs e) => {
                int nuevaPagina = this.Children.IndexOf(this.CurrentPage);//nueva pagina
                int tabActual = (int)Application.Current.Properties["tabActual"];
                if (tabActual < 2)
                {
                    navegarRoot(tabActual);
                }

                Application.Current.Properties["tabActual"] = nuevaPagina;
            };
        }

        public async void navegarRoot(int indicePagina)
        {
            await this.Children[indicePagina].Navigation.PopToRootAsync();
        }
    }
}
