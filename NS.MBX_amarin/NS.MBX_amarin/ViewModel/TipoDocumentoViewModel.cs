using NS.MBX_amarin.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NS.MBX_amarin.ViewModel
{
    public class TipoDocumentoViewModel
    {
        //TODO: Debería usar la forma de trabajo de una de las soluciones abiertas para 
        //TODO: crear un RestService class y su interface.
        public async Task<TipoDocumentoModel[]> GetTipoDocumentos()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GetUrl());
            var response = await client.GetAsync(client.BaseAddress);
            response.EnsureSuccessStatusCode();
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            return TipoDocumentoModel.FromJson(jsonResult);
        }

        //public Task<TipoDocumentoModel[]> GetTipoDocumentos2()
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri(GetUrl());
        //    var response = client.GetAsync(client.BaseAddress);
        //    response.EnsureSuccessStatusCode();

            
        //    var jsonResult = response.Content.ReadAsStringAsync().Result;
        //    return TipoDocumentoModel.FromJson(jsonResult);
        //}

        private string GetUrl()
        {
            //string serviceUrl = $"http://localhost:61883/api/tipoDocumentoApi";
            string serviceUrl = "http://nsmbxweb.azurewebsites.net/api/tipoDocumentoapi";
            return serviceUrl;
        }
    }
}
