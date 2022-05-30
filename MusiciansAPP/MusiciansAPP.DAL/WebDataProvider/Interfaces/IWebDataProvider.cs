using MusiciansAPP.DAL.DALModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.WebDataProvider.Interfaces;

public interface IWebDataProvider
{
    Task<IEnumerable<ArtistDAL>> GetTopArtistsAsync(int pageSize, int page);
    Task<ArtistDetailsDAL> GetArtistDetailsAsync(string name);
    Task<ArtistTracksDAL> GetArtistTopTracksAsync(string name);
    Task<ArtistAlbumsDAL> GetArtistTopAlbumsAsync(string name);
    Task<SimilarArtistDAL> GetSimilarArtistsAsync(string name);
    Task<AlbumDetailsDAL> GetArtistAlbumAsync(string artistName, string albumName);
}