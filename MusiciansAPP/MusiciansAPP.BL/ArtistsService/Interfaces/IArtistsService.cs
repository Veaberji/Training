using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Interfaces
{
    public interface IArtistsService
    {
        Task<IEnumerable<Artist>> GetTopArtists(int pageSize, int page);
    }
}
