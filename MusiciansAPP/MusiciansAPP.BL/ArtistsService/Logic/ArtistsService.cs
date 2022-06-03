using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class ArtistsService : IArtistsService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IDBDataService _dataService;
    private readonly IMapper _mapper;

    public ArtistsService(IWebDataProvider webDataProvider, IDBDataService dataService,
        IMapper mapper)
    {
        _webDataProvider = webDataProvider;
        _dataService = dataService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ArtistBL>> GetTopArtistsAsync(int pageSize, int page)
    {
        var topArtistsDAL = await _webDataProvider.GetTopArtistsAsync(pageSize, page);
        var topArtistsBL = _mapper.Map<IEnumerable<ArtistBL>>(topArtistsDAL);

        await _dataService.SaveTopArtistsAsync(topArtistsBL);

        return topArtistsBL;
    }

    public async Task<ArtistDetailsBL> GetArtistDetailsAsync(string name)
    {
        var artistDetailsDAL = await _webDataProvider.GetArtistDetailsAsync(name);
        var artistDetailsBL = _mapper.Map<ArtistDetailsBL>(artistDetailsDAL);

        await _dataService.SaveArtistDetailsAsync(artistDetailsBL);

        return artistDetailsBL;
    }

    public async Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(string name)
    {
        var artistTracksDAL = await _webDataProvider.GetArtistTopTracksAsync(name);
        var artistTracksBL = _mapper.Map<ArtistTracksBL>(artistTracksDAL);

        await _dataService.SaveTopTracksAsync(artistTracksBL);

        return artistTracksBL.Tracks;
    }

    public async Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(string name)
    {
        var artistAlbumsDAL = await _webDataProvider.GetArtistTopAlbumsAsync(name);
        var artistAlbumsBL = _mapper.Map<ArtistAlbumsBL>(artistAlbumsDAL);

        await _dataService.SaveTopAlbumsAsync(artistAlbumsBL);

        return artistAlbumsBL.Albums;
    }

    public async Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(string name)
    {
        var similarArtistsDAL = await _webDataProvider.GetSimilarArtistsAsync(name);
        var similarArtistsBL = _mapper.Map<SimilarArtistsBL>(similarArtistsDAL);

        await _dataService.SaveSimilarArtistsAsync(similarArtistsBL);

        return similarArtistsBL.Artists;
    }

    public async Task<AlbumDetailsBL> GetArtistAlbumDetailsAsync(
        string artistName, string albumName)
    {
        var albumDetailsDAL = await _webDataProvider
            .GetArtistAlbumDetailsAsync(artistName, albumName);
        var albumDetailsBL = _mapper.Map<AlbumDetailsBL>(albumDetailsDAL);

        await _dataService.SaveAlbumDetailsAsync(albumDetailsBL);

        return albumDetailsBL;
    }
}