using AutoMapper;
using MusiciansAPP.BL.Services.Albums.BLModels;
using MusiciansAPP.BL.Services.Albums.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.Services.Albums.Logic;

public class AlbumsService : IAlbumsService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public AlbumsService(IWebDataProvider webDataProvider,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _webDataProvider = webDataProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(
        string name, int pageSize, int page)
    {
        var albumsFromDb = await _unitOfWork.Albums
            .GetTopAlbumsForArtistAsync(name, pageSize, page);
        if (Entity.IsFullData(albumsFromDb, pageSize))
        {
            return _mapper.Map<IEnumerable<AlbumBL>>(albumsFromDb);
        }

        var artistAlbumsDAL = await _webDataProvider.GetArtistTopAlbumsAsync(name, pageSize, page);
        var artistAlbumsBL = _mapper.Map<ArtistAlbumsBL>(artistAlbumsDAL);

        await SaveArtistTopAlbumsAsync(artistAlbumsBL);

        return artistAlbumsBL.Albums;
    }

    public async Task<AlbumDetailsBL> GetArtistAlbumDetailsAsync(
        string artistName, string albumName)
    {
        var albumFromDb = await _unitOfWork.Albums.GetAlbumDetailsAsync(artistName, albumName);
        if (albumFromDb.IsAlbumDetailsUpToDate())
        {
            return _mapper.Map<AlbumDetailsBL>(albumFromDb);
        }

        var albumDetailsDAL = await _webDataProvider
            .GetArtistAlbumDetailsAsync(artistName, albumName);
        var albumDetailsBL = _mapper.Map<AlbumDetailsBL>(albumDetailsDAL);

        await SaveArtistAlbumDetailsAsync(albumDetailsBL);

        return albumDetailsBL;
    }

    private async Task SaveArtistTopAlbumsAsync(ArtistAlbumsBL model)
    {
        var albums = _mapper.Map<IEnumerable<Album>>(model.Albums);
        var artist =
            await _unitOfWork.Artists.GetArtistDetailsAsync(model.ArtistName);
        await _unitOfWork.Albums.AddOrUpdateArtistAlbumsAsync(artist, albums);
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