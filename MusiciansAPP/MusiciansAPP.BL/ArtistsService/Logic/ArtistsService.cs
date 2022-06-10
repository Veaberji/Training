using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class ArtistsService : IArtistsService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public ArtistsService(IWebDataProvider webDataProvider, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _webDataProvider = webDataProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ArtistBL>> GetTopArtistsAsync(int pageSize, int page)
    {
        var topArtistsDAL = await _webDataProvider.GetTopArtistsAsync(pageSize, page);
        var topArtistsBL = _mapper.Map<IEnumerable<ArtistBL>>(topArtistsDAL);

        var artists = _mapper.Map<IEnumerable<Artist>>(topArtistsBL);
        await _unitOfWork.Artists.AddOrUpdateRangeAsync(artists);
        await _unitOfWork.CompleteAsync();

        return topArtistsBL;
    }

    public async Task<ArtistDetailsBL> GetArtistDetailsAsync(string name)
    {
        var artistDetailsDAL = await _webDataProvider.GetArtistDetailsAsync(name);
        var artistDetailsBL = _mapper.Map<ArtistDetailsBL>(artistDetailsDAL);

        var artist = _mapper.Map<Artist>(artistDetailsBL);
        await _unitOfWork.Artists.AddOrUpdateAsync(artist);
        await _unitOfWork.CompleteAsync();

        return artistDetailsBL;
    }

    public async Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(string name)
    {
        var artistTracksDAL = await _webDataProvider.GetArtistTopTracksAsync(name);
        var artistTracksBL = _mapper.Map<ArtistTracksBL>(artistTracksDAL);

        var tracks = _mapper.Map<IEnumerable<Track>>(artistTracksBL.Tracks);
        var artist = await _unitOfWork.Artists.GetArtistDetailsAsync(artistTracksBL.ArtistName);
        await _unitOfWork.Tracks
            .AddOrUpdateArtistTracksAsync(artist, tracks);
        await _unitOfWork.CompleteAsync();

        return artistTracksBL.Tracks;
    }

    public async Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(string name)
    {
        var artistAlbumsDAL = await _webDataProvider.GetArtistTopAlbumsAsync(name);
        var artistAlbumsBL = _mapper.Map<ArtistAlbumsBL>(artistAlbumsDAL);

        var albums = _mapper.Map<IEnumerable<Album>>(artistAlbumsBL.Albums);
        var artist = await _unitOfWork.Artists.GetArtistDetailsAsync(artistAlbumsBL.ArtistName);
        await _unitOfWork.Albums.AddOrUpdateArtistAlbumsAsync(artist, albums);
        await _unitOfWork.CompleteAsync();

        return artistAlbumsBL.Albums;
    }

    public async Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(string name)
    {
        var similarArtistsDAL = await _webDataProvider.GetSimilarArtistsAsync(name);
        var similarArtistsBL = _mapper.Map<SimilarArtistsBL>(similarArtistsDAL);

        var similarArtists = _mapper.Map<IEnumerable<Artist>>(similarArtistsBL.Artists);
        await _unitOfWork.Artists
            .AddOrUpdateSimilarArtistsAsync(similarArtistsBL.ArtistName, similarArtists);
        await _unitOfWork.CompleteAsync();

        return similarArtistsBL.Artists;
    }

    public async Task<AlbumDetailsBL> GetArtistAlbumDetailsAsync(
        string artistName, string albumName)
    {
        var albumDetailsDAL = await _webDataProvider
            .GetArtistAlbumDetailsAsync(artistName, albumName);
        var albumDetailsBL = _mapper.Map<AlbumDetailsBL>(albumDetailsDAL);

        var album = _mapper.Map<Album>(albumDetailsBL,
                opts: opt => opt.AfterMap((_, dest) => dest.Tracks = new List<Track>()));
        await AddArtistToAlbumAsync(albumDetailsBL.ArtistName, album);
        var tracks = _mapper.Map<IEnumerable<Track>>(albumDetailsBL.Tracks);
        await _unitOfWork.SaveAlbumDetailsAsync(album, tracks);

        return albumDetailsBL;
    }

    private async Task AddArtistToAlbumAsync(string artistName, Album newAlbum)
    {
        var artist = await _unitOfWork.Artists.GetArtistDetailsAsync(artistName);
        newAlbum.Artist = artist;
    }
}