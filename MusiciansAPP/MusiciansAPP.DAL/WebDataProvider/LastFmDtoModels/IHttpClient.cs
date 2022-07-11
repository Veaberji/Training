using System.Net.Http;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels;

public interface IHttpClient
{
    Task<HttpResponseMessage> GetAsync(string url);
}