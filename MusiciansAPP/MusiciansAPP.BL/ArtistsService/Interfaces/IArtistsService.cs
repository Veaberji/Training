using MusiciansAPP.BL.ArtistsService.BLModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Interfaces;

public interface IArtistsService
{
    Task<IEnumerable<ArtistBL>> GetTopArtistsAsync(int pageSize, int page);
    Task<ArtistDetailsBL> GetArtistDetailsAsync(string name);
    Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(string name);
    Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(string name);
    Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(string name);
}