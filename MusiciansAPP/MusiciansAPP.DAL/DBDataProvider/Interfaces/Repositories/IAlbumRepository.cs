using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;

public interface IAlbumRepository : IRepository<Album>
{
    Task<IEnumerable<Album>> GetTopAlbumsForArtistAsync(
        string artistName, int pageSize, int page);
    Task<Album> GetAlbumDetailsAsync(string artistName, string albumName);
    Task AddOrUpdateArtistAlbumsAsync(Artist artist,
        IEnumerable<Album> albums);
}