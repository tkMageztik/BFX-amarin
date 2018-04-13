using Newtonsoft.Json;
using NS.MBX_amarin.Model.Gplaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NS.MBX_amarin.Services.Impl
{
    public class GPlacesService : IGPlacesService
    {
        private string apiKey = Constantes.placesApiKey;
        private PlaceType tipo = PlaceType.Address;
        private Components pais = new Components("country:pe");

        public async Task<ObservableCollection<AutoCompletePrediction> > GetPlaces(string newTextValue)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception(
                    string.Format("You have not assigned a Google API key to PlacesBar"));
            }

            try
            {
                var requestURI = CreatePredictionsUri(newTextValue);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("PlacesBar HTTP request denied.");
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == "ERROR")
                {
                    Console.WriteLine("PlacesSearchBar Google Places API returned ERROR");
                    return null;
                }


                AutoCompleteResult resultado = JsonConvert.DeserializeObject<AutoCompleteResult>(result);
                ObservableCollection<AutoCompletePrediction> listaPredicciones = null;

                if(resultado.AutoCompletePlaces != null)
                {
                    listaPredicciones = new ObservableCollection<AutoCompletePrediction>(resultado.AutoCompletePlaces);
                }

                return listaPredicciones;
            }
            catch (Exception ex)
            {
                Console.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }
        private string CreatePredictionsUri(string newTextValue)
        {
            var url = "https://maps.googleapis.com/maps/api/place/autocomplete/json";
            var input = Uri.EscapeUriString(newTextValue);
            var pType = PlaceTypeValue(tipo);
            var constructedUrl = $"{url}?input={input}&types={pType}&key={apiKey}";
            
            if (pais != null)
                constructedUrl += pais.ToString();

            return constructedUrl;
        }

        private string PlaceTypeValue(PlaceType type)
        {
            switch (type)
            {
                case PlaceType.All:
                    return "";
                case PlaceType.Geocode:
                    return "geocode";
                case PlaceType.Address:
                    return "address";
                case PlaceType.Establishment:
                    return "establishment";
                case PlaceType.Regions:
                    return "(regions)";
                case PlaceType.Cities:
                    return "(cities)";
                default:
                    return "";
            }
        }
    }
}
