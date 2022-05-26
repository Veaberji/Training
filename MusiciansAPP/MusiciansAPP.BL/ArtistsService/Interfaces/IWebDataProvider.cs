using MusiciansAPP.BL.ArtistsService.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Interfaces
{
    public interface IWebDataProvider
    {
        Task<IEnumerable<ArtistDto>> GetTopArtists(int pageSize, int page);
        Task<ArtistDetailsDto> GetArtistDetails(string name);
        Task<ArtistTracksDto> GetArtistTopTracks(string name);
        Task<ArtistAlbumsDto> GetArtistTopAlbums(string name);
        Task<SimilarArtistDto> GetSimilarArtists(string name);
    }
}
