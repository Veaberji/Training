using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Interfaces
{
    public interface IArtistsService
    {
        Task<IEnumerable<Artist>> GetTopArtists(int pageSize, int page);
        Task<Artist> GetArtistDetails(string name);
        Task<ArtistTracksDto> GetArtistTopTracks(string name);
        Task<ArtistAlbumsDto> GetArtistTopAlbums(string name);
        Task<SimilarArtistDto> GetSimilarArtists(string name);
    }
}
