using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.Services.Artists;

public interface IArtistsService
{
    Task<IEnumerable<ArtistBL>> GetTopArtistsAsync(int pageSize, int page);

    Task<ArtistBL> GetArtistDetailsAsync(string name);

    Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(string name, int pageSize, int page);
}