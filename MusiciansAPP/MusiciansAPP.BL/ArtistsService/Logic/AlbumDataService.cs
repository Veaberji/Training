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
        var albumsFromDb = await GetAlbumsFromDbAsync(albums);
        var newAlbums = await CreateNotExistingAlbumsAsync(albums, albumsFromDb);
        UpdateAlbumPlayCount(albums.Albums, albumsFromDb);

        await _unitOfWork.Albums.AddRangeAsync(newAlbums);
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
            await _trackDataService.UpdateArtistTracksAsync(newAlbum);
            await _unitOfWork.Albums.AddAsync(newAlbum);
        }
        else
        {
            if (!IsAlbumTracksUpToDate(album, albumFromDb))
            {
                _mapper.Map(album, albumFromDb);
                await _trackDataService.UpdateArtistTracksAsync(albumFromDb);
            }

            if (!IsAlbumTracksDetailsUpToDate(albumFromDb))
            {
                AddTracksDetails(album, albumFromDb);
            }
        }

        await _unitOfWork.CompleteAsync();
    }

    private async Task<IEnumerable<Album>> GetAlbumsFromDbAsync(ArtistAlbumsBL albums)
    {
        var albumsNames = albums.Albums.Select(a => a.Name).ToList();
        return await _unitOfWork.Albums
            .FindAsync(a => albumsNames.Contains(a.Name)
                            && a.Artist.Name == albums.ArtistName);
    }

    private async Task<IEnumerable<Album>> CreateNotExistingAlbumsAsync(ArtistAlbumsBL albums,
        IEnumerable<Album> albumsFromDb)
    {
        var newAlbums = GetNewAlbums(albums, albumsFromDb);
        await AddArtistToAlbumsAsync(albums, newAlbums);

        return newAlbums;
    }

    private void UpdateAlbumPlayCount(IEnumerable<AlbumBL> albums,
        IEnumerable<Album> albumsFromDb)
    {
        foreach (var albumFromDb in albumsFromDb)
        {
            if (IsAlbumHasPlayCount(albumFromDb))
            {
                continue;
            }

            var album = albums.First(a => a.Name == albumFromDb.Name);
            _mapper.Map(album, albumFromDb);
        }
    }

    private IEnumerable<Album> GetNewAlbums(ArtistAlbumsBL albums,
        IEnumerable<Album> albumsFromDb)
    {
        var albumsFromDbNames = albumsFromDb.Select(a => a.Name).ToList();
        var newAlbumsBL = albums.Albums
            .Where(album => IsNewItem(albumsFromDbNames, album.Name))
            .ToList();
        var newAlbums = _mapper.Map<IEnumerable<Album>>(newAlbumsBL);

        return newAlbums;
    }

    private async Task AddArtistToAlbumsAsync(ArtistAlbumsBL albums,
        IEnumerable<Album> newAlbums)
    {
        var artist = await GetOrCreateArtistAsync(albums.ArtistName);
        foreach (var album in newAlbums)
        {
            album.Artist = artist;
        }
    }

    private bool IsAlbumHasPlayCount(Album albumFromDb)
    {
        return albumFromDb.PlayCount is not null;
    }

    private static bool IsNewItem(IEnumerable<string> existingNames, string name)
    {
        return !existingNames.Contains(name);
    }

    private async Task<Artist> GetOrCreateArtistAsync(string name)
    {
        return await _unitOfWork.Artists
            .GetArtistDetailsAsync(name) ?? new Artist
            {
                Name = name
            };
    }

    private async Task AddArtistToAlbumAsync(AlbumDetailsBL album, Album newAlbum)
    {
        var artist = await GetOrCreateArtistAsync(album.ArtistName);
        newAlbum.Artist = artist;
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