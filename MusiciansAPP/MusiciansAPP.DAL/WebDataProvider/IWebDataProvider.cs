using System.Collections.Generic;
using System.Threading.Tasks;
using MusiciansAPP.DAL.DALModels;

namespace MusiciansAPP.DAL.WebDataProvider;

public interface IWebDataProvider
{
    Task<IEnumerable<ArtistDAL>> GetTopArtistsAsync(int pageSize, int page);

    Task<ArtistDAL> GetArtistDetailsAsync(string name);

    Task<ArtistTracksDAL> GetArtistTopTracksAsync(string name, int pageSize, int page);

    Task<ArtistAlbumsDAL> GetArtistTopAlbumsAsync(string name, int pageSize, int page);

    Task<SimilarArtistsDAL> GetSimilarArtistsAsync(string name, int pageSize, int page);

    Task<AlbumDAL> GetArtistAlbumDetailsAsync(string artistName, string albumName);
}