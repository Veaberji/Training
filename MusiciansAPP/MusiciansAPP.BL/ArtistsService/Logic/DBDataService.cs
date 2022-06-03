using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class DBDataService : IDBDataService
{
    private readonly IArtistDataService _artistDataService;
    private readonly ITrackDataService _trackDataService;
    private readonly IAlbumDataService _albumDataService;

    public DBDataService(IArtistDataService artistDataService,
        ITrackDataService trackDataService,
        IAlbumDataService albumDataService)
    {
        _artistDataService = artistDataService;
        _trackDataService = trackDataService;
        _albumDataService = albumDataService;
    }

    public async Task SaveTopArtistsAsync(IEnumerable<ArtistBL> artists)
    {
        await _artistDataService.SaveTopArtistsAsync(artists);
    }

    public async Task SaveArtistDetailsAsync(ArtistDetailsBL artist)
    {
        await _artistDataService.SaveArtistDetailsAsync(artist);
    }

    public async Task SaveSimilarArtistsAsync(SimilarArtistsBL artists)
    {
        await _artistDataService.SaveSimilarArtistsAsync(artists);
    }

    public async Task SaveTopTracksAsync(ArtistTracksBL tracks)
    {
        await _trackDataService.SaveTopTracksAsync(tracks);
    }

    public async Task SaveTopAlbumsAsync(ArtistAlbumsBL albums)
    {
        await _albumDataService.SaveTopAlbumsAsync(albums);
    }

    public async Task SaveAlbumDetailsAsync(AlbumDetailsBL album)
    {
        await _albumDataService.SaveAlbumDetailsAsync(album);
    }
}