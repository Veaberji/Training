using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.BL.Extensions;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class ArtistsService : IArtistsService
{
    private const int DefaultSize = 10;
    private const int DefaultPage = 1;

    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ArtistsService(IWebDataProvider webDataProvider, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _webDataProvider = webDataProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ArtistsPagingBL> GetTopArtistsAsync(int pageSize, int page)
    {
        var artistsFromDb = await _unitOfWork.Artists
            .GetTopArtistsAsync(pageSize, page);
        if (artistsFromDb.IsFullData(pageSize))
        {
            return _mapper.Map<ArtistsPagingBL>(artistsFromDb);
        }

        var topArtistsDAL = await _webDataProvider.GetTopArtistsAsync(pageSize, page);
        var topArtistsBL = _mapper.Map<ArtistsPagingBL>(topArtistsDAL);

        await SaveTopArtistsAsync(topArtistsBL);

        return topArtistsBL;
    }

    public async Task<ArtistDetailsBL> GetArtistDetailsAsync(string name)
    {
        var artistFromDb = await _unitOfWork.Artists.GetArtistDetailsAsync(name);
        if (artistFromDb.IsArtistDetailsUpToDate())
        {
            return _mapper.Map<ArtistDetailsBL>(artistFromDb);
        }

        var artistDetailsDAL = await _webDataProvider.GetArtistDetailsAsync(name);
        var artistDetailsBL = _mapper.Map<ArtistDetailsBL>(artistDetailsDAL);

        await SaveArtistDetailsAsync(artistDetailsBL);

        return artistDetailsBL;
    }

    public async Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(string name)
    {
        var tracksFromDb = await _unitOfWork.Tracks
            .GetTopTracksForArtistAsync(name, DefaultSize, DefaultPage);
        if (tracksFromDb.IsFullData(DefaultSize))
        {
            return _mapper.Map<IEnumerable<TrackBL>>(tracksFromDb);
        }

        var artistTracksDAL = await _webDataProvider.GetArtistTopTracksAsync(name, DefaultSize, DefaultPage);
        var artistTracksBL = _mapper.Map<ArtistTracksBL>(artistTracksDAL);

        await SaveArtistTopTracksAsync(artistTracksBL);

        return artistTracksBL.Tracks;
    }

    public async Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(string name)
    {
        var albumsFromDb = await _unitOfWork.Albums
            .GetTopAlbumsForArtistAsync(name, DefaultSize, DefaultPage);
        if (albumsFromDb.IsFullData(DefaultSize))
        {
            return _mapper.Map<IEnumerable<AlbumBL>>(albumsFromDb);
        }

        var artistAlbumsDAL = await _webDataProvider.GetArtistTopAlbumsAsync(name, DefaultSize, DefaultPage);
        var artistAlbumsBL = _mapper.Map<ArtistAlbumsBL>(artistAlbumsDAL);

        await SaveArtistTopAlbumsAsync(artistAlbumsBL);

        return artistAlbumsBL.Albums;
    }

    public async Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(string name)
    {
        var artistsFromDb = await _unitOfWork.Artists
            .GetArtistWithSimilarAsync(name, DefaultSize, DefaultPage);
        if (artistsFromDb.SimilarArtists.IsFullData(DefaultSize))
        {
            return _mapper.Map<IEnumerable<ArtistBL>>(artistsFromDb.SimilarArtists);
        }

        var similarArtistsDAL = await _webDataProvider.GetSimilarArtistsAsync(name, DefaultSize, DefaultPage);
        var similarArtistsBL = _mapper.Map<SimilarArtistsBL>(similarArtistsDAL);

        await SaveSimilarArtistsAsync(similarArtistsBL);

        return similarArtistsBL.Artists;
    }

    public async Task<AlbumDetailsBL> GetArtistAlbumDetailsAsync(
        string artistName, string albumName)
    {
        var albumFromDb = await _unitOfWork.Albums.GetAlbumDetailsAsync(artistName, albumName);
        if (albumFromDb.IsAlbumTracksDetailsUpToDate())
        {
            return _mapper.Map<AlbumDetailsBL>(albumFromDb);
        }

        var albumDetailsDAL = await _webDataProvider
            .GetArtistAlbumDetailsAsync(artistName, albumName);
        var albumDetailsBL = _mapper.Map<AlbumDetailsBL>(albumDetailsDAL);

        await SaveArtistAlbumDetailsAsync(albumDetailsBL);

        return albumDetailsBL;
    }

    private async Task SaveTopArtistsAsync(ArtistsPagingBL model)
    {
        var artists = _mapper.Map<IEnumerable<Artist>>(model.Artists);
        await _unitOfWork.Artists.AddOrUpdateRangeAsync(artists);
        await _unitOfWork.CompleteAsync();
    }

    private async Task SaveArtistDetailsAsync(ArtistDetailsBL model)
    {
        var artist = _mapper.Map<Artist>(model);
        await _unitOfWork.Artists.AddOrUpdateAsync(artist);
        await _unitOfWork.CompleteAsync();
    }

    private async Task SaveArtistTopTracksAsync(ArtistTracksBL model)
    {
        var tracks = _mapper.Map<IEnumerable<Track>>(model.Tracks);
        var artist =
            await _unitOfWork.Artists.GetArtistDetailsAsync(model.ArtistName);
        await _unitOfWork.Tracks
            .AddOrUpdateArtistTracksAsync(artist, tracks);
        await _unitOfWork.CompleteAsync();
    }

    private async Task SaveArtistTopAlbumsAsync(ArtistAlbumsBL model)
    {
        var albums = _mapper.Map<IEnumerable<Album>>(model.Albums);
        var artist =
            await _unitOfWork.Artists.GetArtistDetailsAsync(model.ArtistName);
        await _unitOfWork.Albums.AddOrUpdateArtistAlbumsAsync(artist, albums);
        await _unitOfWork.CompleteAsync();
    }

    private async Task SaveSimilarArtistsAsync(SimilarArtistsBL model)
    {
        var similarArtists =
            _mapper.Map<IEnumerable<Artist>>(model.Artists);
        await _unitOfWork.Artists
            .AddOrUpdateSimilarArtistsAsync(model.ArtistName, similarArtists);
        await _unitOfWork.CompleteAsync();
    }

    private async Task SaveArtistAlbumDetailsAsync(AlbumDetailsBL model)
    {
        var album = _mapper.Map<Album>(model,
            opts: opt =>
                opt.AfterMap((_, dest) => dest.Tracks = new List<Track>()));
        await AddArtistToAlbumAsync(model.ArtistName, album);
        var tracks = _mapper.Map<IEnumerable<Track>>(model.Tracks);
        await _unitOfWork.SaveAlbumDetailsAsync(album, tracks);
    }

    private async Task AddArtistToAlbumAsync(string artistName, Album newAlbum)
    {
        var artist = await _unitOfWork.Artists.GetArtistDetailsAsync(artistName);
        newAlbum.Artist = artist;
    }
}