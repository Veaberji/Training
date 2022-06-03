using MusiciansAPP.BL.ArtistsService.BLModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Interfaces;

public interface IDBDataService
{
    Task SaveTopArtistsAsync(IEnumerable<ArtistBL> topArtists);
    Task SaveArtistDetailsAsync(ArtistDetailsBL artist);
    Task SaveSimilarArtistsAsync(SimilarArtistsBL artists);
    Task SaveTopTracksAsync(ArtistTracksBL tracks);
    Task SaveTopAlbumsAsync(ArtistAlbumsBL albums);
    Task SaveAlbumDetailsAsync(AlbumDetailsBL albums);
}