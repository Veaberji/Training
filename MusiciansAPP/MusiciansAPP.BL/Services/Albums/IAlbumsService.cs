using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.Services.Albums;

public interface IAlbumsService
{
    Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(string name, int pageSize, int page);

    Task<AlbumBL> GetArtistAlbumDetailsAsync(string artistName, string albumName);
}