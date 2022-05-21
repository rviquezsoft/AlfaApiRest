using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using AlfaApiRest.Models;
using System.Collections.Generic;
using System.Linq;

namespace AlfaApiRest
{
    class Program
    {

        static async Task Main(string[] args)
        {
            string header = "NAME                                           WEB PAGE";
            string line = "===================================================================================";
            string exit = "n";
            //inicializaciones necesarias para el correcto funcionamiento
            IHost host = await init();
            var rest = host.Services.GetRequiredService<IHTTPClientService>();
            var deserealize = host.Services.GetRequiredService<IDeserealizeService<University>>();
            if (host==null||rest==null||deserealize==null)
            {
                Console.WriteLine("No se logró ejecutar correctamente la inicialización" +
                    " del sistema por lo que se finalizará su ejecución");
                Environment.Exit(0);
            }
            
            Console.ForegroundColor = ConsoleColor.Blue;
            
            string country = "";
            do
            {
                Console.Clear();
                Console.WriteLine("Ingrese el nombre del país que desea consultar");
                country=Console.ReadLine();

                if (string.IsNullOrWhiteSpace(country))
                {
                    Console.WriteLine("Debe ingresar un país válido");
                }
                else
                {
                    var result = await rest.GetUniversities(country);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        Console.WriteLine(result);
                    }
                    else
                    {
                        List<University> data = await deserealize.Deserealize(result);

                        if (data==null||data.Count<1)
                        {
                            Console.WriteLine("No se encontró ningún país con " +
                                "el código ingresado");
                        }
                        else
                        {
                            Console.WriteLine(header);
                            Console.WriteLine(line);
                            foreach (var item in data)
                            {
                                string text = item.name +
                                    "                            " +
                                    (item.web_pages != null ?
                                    item.web_pages.FirstOrDefault() :
                                    "");
                                Console.WriteLine(text);
                            }
                            Console.WriteLine(line);
                        }
                    }
                }

                Console.WriteLine("Digite \"s\"si desea consultar otro país" +
                    " o enter para salir");
                exit = Console.ReadLine();

            } while (exit == "s"||exit=="S");
        }

        /// <summary>
        /// Método init se usa para poder hacer uso de el appsettings.json y
        /// para poder hacer uso de la inyección de dependencias
        /// </summary>
        /// <returns></returns>
        static Task<IHost> init()
        {
            IHost host = null;
            try
            {
                string internalRoute =
                        Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);


                var builder = new HostBuilder()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.SetBasePath(internalRoute);
                        config.AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables();
                    })
                    .ConfigureServices((hostingContext, services) =>
                    {
                        services.AddHttpClient();
                        services.AddTransient<IHTTPClientService, HTTPClientService>();
                        services.AddTransient<IDeserealizeService<University>, DeserealizeService<University>>();
                    });

                host = builder.Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Task.FromResult(host);
        }
    }


}

