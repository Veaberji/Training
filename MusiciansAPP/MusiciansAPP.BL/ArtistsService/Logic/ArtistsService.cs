using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class ArtistsService : IArtistsService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ArtistsService(IWebDataProvider webDataProvider, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _webDataProvider = webDataProvider;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ArtistBL>> GetTopArtistsAsync(int pageSize, int page)
    {
        var topArtistsDAL = await _webDataProvider.GetTopArtistsAsync(pageSize, page);
        var topArtistsBL = _mapper.Map<IEnumerable<ArtistBL>>(topArtistsDAL);

        await SaveTopArtistsAsync(topArtistsBL);

        return topArtistsBL;
    }

    public async Task<ArtistDetailsBL> GetArtistDetailsAsync(string name)
    {
        var artistDetailsDAL = await _webDataProvider.GetArtistDetailsAsync(name);
        var artistDetailsBL = _mapper.Map<ArtistDetailsBL>(artistDetailsDAL);

        await SaveArtistDetailsAsync(artistDetailsBL);

        return artistDetailsBL;
    }

    public async Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(string name)
    {
        var artistTracksDAL = await _webDataProvider.GetArtistTopTracksAsync(name);
        var artistTracksBL = _mapper.Map<ArtistTracksBL>(artistTracksDAL);

        await SaveTopTracksAsync(artistTracksBL);

        return artistTracksBL.Tracks;
    }

    public async Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(string name)
    {
        var artistAlbumsDAL = await _webDataProvider.GetArtistTopAlbumsAsync(name);
        var artistAlbumsBL = _mapper.Map<ArtistAlbumsBL>(artistAlbumsDAL);

        await SaveTopAlbumsAsync(artistAlbumsBL);

        return artistAlbumsBL.Albums;
    }

    public async Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(string name)
    {
        var similarArtistsDAL = await _webDataProvider.GetSimilarArtistsAsync(name);
        var similarArtistsBL = _mapper.Map<SimilarArtistsBL>(similarArtistsDAL);

        await SaveSimilarArtistsAsync(similarArtistsBL);

        return similarArtistsBL.Artists;
    }

    public async Task<AlbumDetailsBL> GetArtistAlbumDetailsAsync(
        string artistName, string albumName)
    {
        var albumDetailsDAL = await _webDataProvider
            .GetArtistAlbumDetailsAsync(artistName, albumName);
        var albumDetailsBL = _mapper.Map<AlbumDetailsBL>(albumDetailsDAL);

        await SaveAlbumDetailsAsync(albumDetailsBL);

        return albumDetailsBL;
    }

    private async Task SaveTopArtistsAsync(IEnumerable<ArtistBL> artists)
    {
        var artistsFromDb = await GetArtistsFromDbAsync(artists);
        var newArtists = CreateNotExistingArtists(artists, artistsFromDb);
        UpdateArtistImageUrl(artists, artistsFromDb);

        await _unitOfWork.Artists.AddRangeAsync(newArtists);
        await _unitOfWork.CompleteAsync();
    }

    private async Task<IEnumerable<Artist>> GetArtistsFromDbAsync(
        IEnumerable<ArtistBL> artists)
    {
        var artistsNames = artists.Select(a => a.Name).ToList();
        return await _unitOfWork.Artists
            .FindAsync(a => artistsNames.Contains(a.Name));
    }

    private IEnumerable<Artist> CreateNotExistingArtists(IEnumerable<ArtistBL> artists,
        IEnumerable<Artist> artistsFromDb)
    {
        var artistsFromDbNames = artistsFromDb.Select(a => a.Name).ToList();
        var newArtists = artists
            .Where(artist => IsNewItem(artistsFromDbNames, artist.Name))
            .ToList();

        return _mapper.Map<IEnumerable<Artist>>(newArtists);
    }

    private void UpdateArtistImageUrl(IEnumerable<ArtistBL> artists,
        IEnumerable<Artist> artistsFromDb)
    {
        foreach (var artistFromDb in artistsFromDb)
        {
            if (IsArtistHasImageUrl(artistFromDb))
            {
                continue;
            }

            var artist = artists.First(a => a.Name == artistFromDb.Name);
            _mapper.Map(artist, artistFromDb);
        }
    }

    private static bool IsNewItem(IEnumerable<string> existingNames, string name)
    {
        return !existingNames.Contains(name);
    }

    private async Task SaveArtistDetailsAsync(ArtistDetailsBL artist)
    {
        var artistFromDb = (await _unitOfWork.Artists
            .FindAsync(a => a.Name == artist.Name)).FirstOrDefault();
        if (artistFromDb is null)
        {
            var newArtist = _mapper.Map<Artist>(artist);
            await _unitOfWork.Artists.AddAsync(newArtist);
        }
        else if (!IsArtistDetailsUpToDate(artistFromDb))
        {
            _mapper.Map(artist, artistFromDb);
        }

        await _unitOfWork.CompleteAsync();
    }

    private bool IsArtistDetailsUpToDate(Artist artist)
    {
        return IsArtistHasImageUrl(artist) && artist.Biography is not null;
    }

    private bool IsArtistHasImageUrl(Artist artist)
    {
        return artist.ImageUrl is not null;
    }

    private async Task SaveTopTracksAsync(ArtistTracksBL tracks)
    {
        var tracksNames = tracks.Tracks.Select(t => t.Name).ToList();
        var tracksFromDb = await GetTracksFromDbAsync(tracksNames, tracks.ArtistName);
        var newTracks = await CreateNotExistingTracksAsync(tracks, tracksFromDb);
        UpdateTrackPlayCount(tracks.Tracks, tracksFromDb);

        await _unitOfWork.Tracks.AddRangeAsync(newTracks);
        await _unitOfWork.CompleteAsync();
    }

    private async Task<IEnumerable<Track>> GetTracksFromDbAsync(IEnumerable<string> tracksNames,
        string artistName)
    {
        return await _unitOfWork.Tracks
            .FindAsync(t => tracksNames.Contains(t.Name)
                            && t.Artist.Name == artistName);
    }

    private async Task<IEnumerable<Track>> CreateNotExistingTracksAsync(ArtistTracksBL tracksBL,
        IEnumerable<Track> tracksFromDb)
    {
        var tracks = _mapper.Map<IEnumerable<Track>>(tracksBL.Tracks);
        var newTracks = GetNewTracks(tracks, tracksFromDb);
        var artist = await GetOrCreateArtistAsync(tracksBL.ArtistName);
        AddArtistToTracks(artist, newTracks);

        return newTracks;
    }

    private void UpdateTrackPlayCount(IEnumerable<TrackBL> tracks,
        IEnumerable<Track> tracksFromDb)
    {
        foreach (var trackFromDb in tracksFromDb)
        {
            if (IsTrackHasPlayCount(trackFromDb))
            {
                continue;
            }

            var track = tracks.First(a => a.Name == trackFromDb.Name);
            _mapper.Map(track, trackFromDb);
        }
    }

    private bool IsTrackHasPlayCount(Track trackFromDb)
    {
        return trackFromDb.PlayCount is not null;
    }

    private IEnumerable<Track> GetNewTracks(IEnumerable<Track> tracks,
        IEnumerable<Track> tracksFromDb)
    {
        var tracksFromDbNames = tracksFromDb.Select(t => t.Name).ToList();
        return tracks
            .Where(track => IsNewItem(tracksFromDbNames, track.Name))
            .ToList();
    }

    private void AddArtistToTracks(Artist artist,
        IEnumerable<Track> newTracks)
    {
        foreach (var track in newTracks)
        {
            track.Artist = artist;
        }
    }

    private async Task<Artist> GetOrCreateArtistAsync(string name)
    {
        return await _unitOfWork.Artists
            .GetArtistDetailsAsync(name) ?? new Artist
            {
                Name = name
            };
    }

    private async Task SaveTopAlbumsAsync(ArtistAlbumsBL albums)
    {
        var albumsFromDb = await GetAlbumsFromDbAsync(albums);
        var newAlbums = await CreateNotExistingAlbumsAsync(albums, albumsFromDb);
        UpdateAlbumPlayCount(albums.Albums, albumsFromDb);

        await _unitOfWork.Albums.AddRangeAsync(newAlbums);
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

    private bool IsAlbumHasPlayCount(Album albumFromDb)
    {
        return albumFromDb.PlayCount is not null;
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

    private async Task SaveSimilarArtistsAsync(SimilarArtistsBL artists)
    {
        var artistInDB = await _unitOfWork.Artists
            .GetArtistWithSimilarAsync(artists.ArtistName, pageSize: int.MaxValue, page: 1);
        if (artistInDB is null)
        {
            var newArtist = _mapper.Map<Artist>(artists);
            await AddSimilarToArtistAsync(newArtist, artists.Artists);
            await _unitOfWork.Artists.AddAsync(newArtist);
        }
        else
        {
            await AddSimilarToArtistAsync(artistInDB, artists.Artists);
        }

        await _unitOfWork.CompleteAsync();
    }

    private async Task AddSimilarToArtistAsync(Artist artist,
        IEnumerable<ArtistBL> similarArtists)
    {
        var artists = await GetSimilarArtistsAsync(similarArtists);
        var newSimilarArtists = GetNewSimilarArtists(artist, artists);
        artist.SimilarArtists.AddRange(newSimilarArtists);
    }

    private async Task<List<Artist>> GetSimilarArtistsAsync(IEnumerable<ArtistBL> similarArtists)
    {
        var artistsFromDb = await GetArtistsFromDbAsync(similarArtists);
        var newArtists = CreateNotExistingArtists(similarArtists, artistsFromDb);

        var artists = artistsFromDb.Concat(newArtists).ToList();
        return artists;
    }

    private static IEnumerable<Artist> GetNewSimilarArtists(Artist artist,
        IEnumerable<Artist> artists)
    {
        var existingSimilarArtistsIds = artist.SimilarArtists.Select(a => a.Id).ToList();
        var newSimilarArtists = artists
            .Where(artist => IsNewSimilarArtist(existingSimilarArtistsIds, artist))
            .ToList();

        return newSimilarArtists;
    }

    private static bool IsNewSimilarArtist(IEnumerable<Guid> existingSimilarArtistsIds,
        Artist artist)
    {
        return !existingSimilarArtistsIds.Contains(artist.Id);
    }

    private async Task SaveAlbumDetailsAsync(AlbumDetailsBL album)
    {
        var albumFromDb = await _unitOfWork.Albums
            .GetAlbumDetailsAsync(album.ArtistName, album.Name);
        if (albumFromDb is null)
        {
            var newAlbum = _mapper.Map<Album>(album);
            await AddArtistToAlbumAsync(album, newAlbum);
            await UpdateArtistTracksAsync(newAlbum);
            await _unitOfWork.Albums.AddAsync(newAlbum);
        }
        else
        {
            if (!IsAlbumTracksUpToDate(album, albumFromDb))
            {
                _mapper.Map(album, albumFromDb);
                await UpdateArtistTracksAsync(albumFromDb);
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

    private async Task UpdateArtistTracksAsync(Album newAlbum)
    {
        var tracksNames = newAlbum.Tracks.Select(t => t.Name).ToList();
        var tracksFromDb =
            await GetTracksFromDbAsync(tracksNames, newAlbum.Artist.Name);
        var newTracks = GetNewTracks(newAlbum.Tracks, tracksFromDb);

        AddArtistToTracks(newAlbum.Artist, newTracks);
        ReplaceAlbumTracks(newAlbum, tracksFromDb, newTracks);
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

    private static void ReplaceAlbumTracks(Album newAlbum, IEnumerable<Track> tracksFromDb,
        IEnumerable<Track> newTracks)
    {
        var emptyEnumerable = Enumerable.Empty<Track>();
        var tracks = (tracksFromDb ?? emptyEnumerable)
            .Concat(newTracks ?? emptyEnumerable).ToList();
        newAlbum.Tracks = tracks;
    }
}