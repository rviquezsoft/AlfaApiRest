using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlfaApiRest
{
    public interface IDeserealizeService<T> where T : class
    {
        Task<List<T>> Deserealize(string payload);
    }
}