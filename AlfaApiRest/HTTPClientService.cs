using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AlfaApiRest
{
    public class HTTPClientService : IHTTPClientService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;
        public HTTPClientService(
             IHttpClientFactory _httpClientFactory,
             IConfiguration _configuration)
        {
            httpClientFactory = _httpClientFactory;
            configuration = _configuration;
        }

        /// <summary>
        /// Busca la url a la que se debe conectar en el appsettings.json
        /// y se le agrega el nombre del país por parámetro para no hacer get de
        /// todos los países si no solo de el que se ocupa
        /// a continuación ejecuta un GET y devuelve un string con el payload descargado
        /// o un mensaje de error en caso de no obtener datos
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public async Task<string> GetUniversities(string country)
        {
            string result = "";
            string error = "No fue posible obtener la url del " +
                "archivo appsettings.json. " +
                "Por favor revise la configuración e intente de nuevo";
            try
            {
                string url = configuration["ApiURL"];

                using (var httpClient = httpClientFactory.CreateClient())
                {
                    var call = await httpClient.GetAsync(url+country);
                    result = await call.Content.ReadAsStringAsync();
                }

                if (string.IsNullOrWhiteSpace(result))
                {
                    result = error;
                }
            }
            catch (Exception ex)
            {
                result = error;
                return result;
            }

            return result;
        }
    }
}

