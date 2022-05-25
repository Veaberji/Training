using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic
{
    public class ArtistsService : IArtistsService
    {
        private readonly IWebDataProvider _webDataProvider;

        public ArtistsService(IWebDataProvider webDataProvider)
        {
            _webDataProvider = webDataProvider;
        }

        public Task<IEnumerable<Artist>> GetTopArtists(int pageSize, int page)
        {
            return _webDataProvider.GetTopArtists(pageSize, page);
        }
    }
}
