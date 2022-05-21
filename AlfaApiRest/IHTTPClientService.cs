using System.Threading.Tasks;

namespace AlfaApiRest
{
    public interface IHTTPClientService
    {
        Task<string> GetUniversities(string country);
    }
}