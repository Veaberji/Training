using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Linq;
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
        var artist = await GetOrCreateArtistAsync(albums.ArtistName);
        await _unitOfWork.Albums
            .AddOrUpdateArtistAlbumsAsync(artist, mappedAlbums);
        await _unitOfWork.CompleteAsync();
    }

    public async Task SaveAlbumDetailsAsync(AlbumDetailsBL album)
    {
        var albumFromDb = await _unitOfWork.Albums
            .GetAlbumDetailsAsync(album.ArtistName, album.Name);
        if (albumFromDb is null)
        {
            var newAlbum = _mapper.Map<Album>(album);
            await AddArtistToAlbumAsync(album, newAlbum);
            await _trackDataService.UpdateAlbumTracksAsync(newAlbum);
            await _unitOfWork.Albums.AddAsync(newAlbum);
        }
        else
        {
            if (!IsAlbumTracksUpToDate(album, albumFromDb))
            {
                _mapper.Map(album, albumFromDb);
                await _trackDataService.UpdateAlbumTracksAsync(albumFromDb);
            }

            if (!IsAlbumTracksDetailsUpToDate(albumFromDb))
            {
                AddTracksDetails(album, albumFromDb);
            }
        }

        await _unitOfWork.CompleteAsync();
    }

    private async Task AddArtistToAlbumAsync(AlbumDetailsBL album, Album newAlbum)
    {
        var artist = await GetOrCreateArtistAsync(album.ArtistName);
        newAlbum.Artist = artist;
    }

    private async Task<Artist> GetOrCreateArtistAsync(string artistName)
    {
        return await _unitOfWork.Artists
            .GetArtistDetailsAsync(artistName) ?? new Artist
            {
                Name = artistName
            };
    }

    private bool IsAlbumTracksUpToDate(AlbumDetailsBL album, Album albumFromDb)
    {
        return album.Tracks.Count() == albumFromDb.Tracks.Count();
    }

    private bool IsAlbumTracksDetailsUpToDate(Album album)
    {
        return !album.Tracks.Any(track => track.DurationInSeconds is null);
    }

    private void AddTracksDetails(AlbumDetailsBL album, Album albumFromDb)
    {
        AddTracksDuration(album, albumFromDb);
    }

    private void AddTracksDuration(AlbumDetailsBL album, Album albumFromDb)
    {
        foreach (var track in albumFromDb.Tracks)
        {
            if (track.DurationInSeconds is not null)
            {
                continue;
            }

            int? duration = album.Tracks
                .FirstOrDefault(t => t.Name == track.Name)?
                .DurationInSeconds;
            track.DurationInSeconds = duration;
        }
    }
}