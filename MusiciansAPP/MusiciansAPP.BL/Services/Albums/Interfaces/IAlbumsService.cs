using MusiciansAPP.BL.Services.Albums.BLModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.Services.Albums.Interfaces;

public interface IAlbumsService
{
    Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(string name, int pageSize, int page);
    Task<AlbumDetailsBL> GetArtistAlbumDetailsAsync(string artistName, string albumName);
}