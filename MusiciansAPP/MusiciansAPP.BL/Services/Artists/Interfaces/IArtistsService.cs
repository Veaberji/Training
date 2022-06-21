using MusiciansAPP.BL.Services.Artists.BLModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.Services.Artists.Interfaces;

public interface IArtistsService
{
    Task<ArtistsPagingBL> GetTopArtistsAsync(int pageSize, int page);
    Task<ArtistDetailsBL> GetArtistDetailsAsync(string name);
    Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(string name, int pageSize, int page);
}