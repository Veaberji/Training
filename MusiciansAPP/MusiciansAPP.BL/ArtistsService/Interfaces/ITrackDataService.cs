using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.Domain;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Interfaces;

public interface ITrackDataService
{
    Task SaveTopTracksAsync(ArtistTracksBL tracks);
    Task UpdateArtistTracksAsync(Album newAlbum);
}