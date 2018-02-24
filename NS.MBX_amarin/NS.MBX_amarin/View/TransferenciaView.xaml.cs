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
	public partial class TransferenciaView : ContentPage
	{
        public Cuenta ctaOrigen;
        public Cuenta ctaDestino;
        public bool origenMisCuentas;
        public string tipoTransf;

        public TransferenciaView(Cuenta ctaOrigen, Cuenta ctaDestino, bool origenMisCuentas, string tipoTransf)
		{
			InitializeComponent ();

            Title = "Transferencia";
            
            this.ctaOrigen = ctaOrigen;
            this.ctaDestino = ctaDestino;
            this.origenMisCuentas = origenMisCuentas;
            this.tipoTransf = tipoTransf;

            lblNombreOri.Text = "De: " + ctaOrigen.NombreCta;
            lblSaldoOri.Text = ctaOrigen.Moneda + " " + ctaOrigen.SaldoDisponible.ToString();

            lblNombreDest.Text = "Para: " + ctaDestino.NombreCta;

            lblMonto.Text = "Monto(" + ctaOrigen.Moneda + "):";

            navBar.seleccionarBoton("1");
		}

        private async void BtnSgt_OnClicked(object sender, EventArgs args)
        {
            //try
            //{
                //validaciones
                decimal montoDec;
                if (!Decimal.TryParse(entMonto.Text, out montoDec))
                {
                    await DisplayAlert("Transferencia", "Ingrese un monto numérico", "OK");
                    return;
                }else 
                if (ctaOrigen.SaldoDisponible < montoDec)
                {
                    await DisplayAlert("Transferencia", "No hay suficiente saldo en la cuenta de origen", "OK");
                    return;
                }
                //ok
                else
                {
                    ctaOrigen.SaldoDisponible -= montoDec;

                    if(tipoTransf == "2")
                    {
                        decimal monto = montoDec;
                        if (ctaOrigen.idMoneda == "PEN" && ctaDestino.idMoneda == "USD")
                        {
                            monto = decimal.Round(Decimal.Divide(montoDec, 3.3M), 2, MidpointRounding.AwayFromZero);
                        
                        }
                        else if (ctaOrigen.idMoneda == "USD" && ctaDestino.idMoneda == "PEN")
                        {
                            monto = decimal.Round(Decimal.Multiply(montoDec, 3.3M), 2, MidpointRounding.AwayFromZero);
                        }

                        ctaDestino.SaldoDisponible += monto;

                    }

                    await DisplayAlert("Transferencia Exitosa", "Transferido correctamente", "OK");
                }

            //catch(Exception e)
            //{

            //}
            //await Navigation.PushAsync(new SeleccionaCtaCargo("Transferencia Ctas mismo banco"));
            //ShowPopup();
            //if(ctaOrigen.SaldoDisponible < System.Convert.ToDecimal(entMonto.Text))

            //await DisplayAlert("Transferencia Exitosa", "Transferido correctamente", "OK");


            //var page = new NavigationPage(new CuentasView());

            //await Navigation.PushAsync(page);
            if (origenMisCuentas)
            {
                RemoveBeforeDestination(typeof(CuentasView));
            }
            else
            {
                RemoveBeforeDestination(typeof(OperacionesView));
            }
            

            //await Navigation.PushAsync(new CuentasView());
            //await Navigation.PushAsync(page);
            await Navigation.PopAsync();

        }

        public void RemoveBeforeDestination(Type DestinationPage)
        {
            int LeastFoundIndex = 0;
            int PagesToRemove = 0;

            for (int index = Navigation.NavigationStack.Count - 2; index > 0; index--)
            {
                if (Navigation.NavigationStack[index].GetType().Equals(DestinationPage))
                {
                    break;
                }
                else
                {
                    LeastFoundIndex = index;
                    PagesToRemove++;
                }
            }

            for (int index = 0; index < PagesToRemove; index++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[LeastFoundIndex]);
            }

        }

    }
}