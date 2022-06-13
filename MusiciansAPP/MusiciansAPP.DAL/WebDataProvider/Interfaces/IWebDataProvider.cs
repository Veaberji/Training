using MusiciansAPP.DAL.DALModels;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.WebDataProvider.Interfaces;

public interface IWebDataProvider
{
    Task<ArtistsPagingDAL> GetTopArtistsAsync(int pageSize, int page);
    Task<ArtistDetailsDAL> GetArtistDetailsAsync(string name);
    Task<ArtistTracksDAL> GetArtistTopTracksAsync(string name, int pageSize, int page);
    Task<ArtistAlbumsDAL> GetArtistTopAlbumsAsync(string name, int pageSize, int page);
    Task<SimilarArtistsDAL> GetSimilarArtistsAsync(string name, int pageSize, int page);
    Task<AlbumDetailsDAL> GetArtistAlbumDetailsAsync(string artistName, string albumName);
}