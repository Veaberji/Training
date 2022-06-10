using MusiciansAPP.DAL.DALModels;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.WebDataProvider.Interfaces;

public interface IWebDataProvider
{
    Task<ArtistsPagingDAL> GetTopArtistsAsync(int pageSize, int page);
    Task<ArtistDetailsDAL> GetArtistDetailsAsync(string name);
    Task<ArtistTracksDAL> GetArtistTopTracksAsync(string name);
    Task<ArtistAlbumsDAL> GetArtistTopAlbumsAsync(string name);
    Task<SimilarArtistsDAL> GetSimilarArtistsAsync(string name);
    Task<AlbumDetailsDAL> GetArtistAlbumDetailsAsync(string artistName, string albumName);
}