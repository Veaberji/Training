using MusiciansAPP.BL.ArtistsService.BLModels;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Interfaces;

public interface IAlbumDataService
{
    Task SaveTopAlbumsAsync(ArtistAlbumsBL albums);
    Task SaveAlbumDetailsAsync(AlbumDetailsBL album);
}