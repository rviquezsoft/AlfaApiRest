using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AlfaApiRest
{
    public class DeserealizeService<T> : IDeserealizeService<T> where T : class
    {
        public DeserealizeService()
        {
        }

        /// <summary>
        /// Recibe los datos descargados de el api e intenta deserealizar
        /// los datos en el tipo (T) que se desea como una lista o devuelve
        /// null en caso de error
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public Task<List<T>> Deserealize(string payload)
        {
            string error = "No se logró obtener la información del servidor";
            List<T> list = null;
            try
            {
                list = JsonConvert.DeserializeObject<List<T>>(payload,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(error + "-->" + ex.ToString());
            }

            return Task.FromResult(list);
        }
    }
}

