﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NS.MBX_amarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeleccionaCtaDestino : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }
        private string origen { get; set; }
        public SeleccionaCtaDestino(string origen)
        {

            InitializeComponent();
            this.origen = origen;
            Items = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
            };

            MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void BtnSgt_OnClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new SeleccionaCtaCargo("Transferencia Ctas mismo banco"));
            //ShowPopup();

            await DisplayAlert("Transferencia Exitosa", "Transferido correctamente", "OK");


            var page = new NavigationPage(new CuentasView());

            await Navigation.PushAsync(page);
            //await Navigation.PushAsync(page);


        }
    }
}
