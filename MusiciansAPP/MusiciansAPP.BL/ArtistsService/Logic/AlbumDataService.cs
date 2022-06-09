using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class AlbumDataService : IAlbumDataService
{
    private readonly ITrackDataService _trackDataService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AlbumDataService(IUnitOfWork unitOfWork, IMapper mapper,
        ITrackDataService trackDataService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _trackDataService = trackDataService;
    }

    public async Task SaveTopAlbumsAsync(ArtistAlbumsBL albums)
    {
        var mappedAlbums = _mapper.Map<IEnumerable<Album>>(albums.Albums);
        var artist = await _unitOfWork.Artists.GetArtistDetailsAsync(albums.ArtistName);
        await _unitOfWork.Albums
            .AddOrUpdateArtistAlbumsAsync(artist, mappedAlbums);

        await _unitOfWork.CompleteAsync();
    }

    public async Task SaveAlbumDetailsAsync(AlbumDetailsBL album)
    {
        var mappedAlbum = _mapper.Map<Album>(album,
            opts: opt => opt.AfterMap((_, dest) => dest.Tracks = new List<Track>()));
        await AddArtistToAlbumAsync(album, mappedAlbum);
        var addedAlbum = await _unitOfWork.Albums
            .AddOrUpdateAlbumDetailsAsync(mappedAlbum);

        await _trackDataService.UpdateAlbumTracksAsync(addedAlbum, album.Tracks);
    }

    private async Task AddArtistToAlbumAsync(AlbumDetailsBL album, Album newAlbum)
    {
        var artist = await _unitOfWork.Artists.GetArtistDetailsAsync(album.ArtistName);
        newAlbum.Artist = artist;
    }
}