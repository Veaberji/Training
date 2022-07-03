using Microsoft.EntityFrameworkCore;
using MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;
using MusiciansAPP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Logic.Repositories;

public class TrackRepository : Repository<Track>, ITrackRepository
{
    public TrackRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Track>> GetTopTracksForArtistAsync(
        string artistName, int pageSize, int page)
    {
        return await Tracks
            .Where(t => t.Artist.Name == artistName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddOrUpdateArtistTracksAsync(Artist artist, IEnumerable<Track> tracks)
    {
        var tracksNames = tracks.Select(t => t.Name).ToList();
        var tracksFromDb = await GetTracksFromDbAsync(tracksNames, artist.Name);
        var newTracks = CreateNotExistingTracks(artist, tracks, tracksFromDb);
        UpdateTrackPlayCount(tracks, tracksFromDb);

        await AddRangeAsync(newTracks);
    }

    public async Task AddOrUpdateAlbumTracksAsync(Album album,
        IEnumerable<Track> tracks)
    {
        await UpdateAlbumTracksAsync(album, tracks);
        if (!album.IsAlbumTracksDetailsUpToDate())
        {
            AddTracksDetails(album, tracks);
        }
    }

    private DbSet<Track> Tracks => (Context as AppDbContext)?.Tracks;

    private async Task<IEnumerable<Track>> GetTracksFromDbAsync(IEnumerable<string> tracksNames,
        string artistName)
    {
        return await FindAsync(t => tracksNames.Contains(t.Name)
                            && t.Artist.Name == artistName);
    }

    private IEnumerable<Track> CreateNotExistingTracks(Artist artist,
        IEnumerable<Track> tracks,
        IEnumerable<Track> tracksFromDb)
    {
        var newTracks = GetNewTracks(tracks, tracksFromDb);
        AddArtistToTracks(artist, newTracks);

        return newTracks;
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

    private void AddAlbumToTracks(Album album,
        IEnumerable<Track> tracks)
    {
        foreach (var track in tracks)
        {
            track.Album = album;
        }
    }

    private void UpdateTrackPlayCount(IEnumerable<Track> tracks,
        IEnumerable<Track> tracksFromDb)
    {
        foreach (var trackFromDb in tracksFromDb.Where(t => !t.IsTrackHasPlayCount()))
        {
            var track = tracks.First(t => string
                .Equals(t.Name, trackFromDb.Name, StringComparison.OrdinalIgnoreCase));
            trackFromDb.PlayCount = track.PlayCount;
        }
    }

    private static void ReplaceAlbumTracks(Album album, IEnumerable<Track> tracksFromDb,
        IEnumerable<Track> newTracks)
    {
        var emptyEnumerable = Enumerable.Empty<Track>();
        var tracks = (tracksFromDb ?? emptyEnumerable)
            .Concat(newTracks ?? emptyEnumerable).ToList();
        album.Tracks = tracks;
    }

    private async Task UpdateAlbumTracksAsync(Album album, IEnumerable<Track> tracks)
    {
        var tracksNames = tracks.Select(t => t.Name).ToList();
        var tracksFromDb =
            await GetTracksFromDbAsync(tracksNames, album.Artist.Name);
        var newTracks = GetNewTracks(tracks, tracksFromDb);

        AddArtistToTracks(album.Artist, newTracks);
        AddAlbumToTracks(album, tracksFromDb);
        ReplaceAlbumTracks(album, tracksFromDb, newTracks);
    }

    private void AddTracksDetails(Album album, IEnumerable<Track> tracks)
    {
        AddTracksDuration(album, tracks);
    }

    private void AddTracksDuration(Album album, IEnumerable<Track> tracks)
    {
        foreach (var track in album.Tracks.Where(t => !t.IsTrackHasDurationInSeconds()))
        {
            int? duration = tracks
            .FirstOrDefault(t => t.Name == track.Name)?
            .DurationInSeconds;
            track.DurationInSeconds = duration;
        }
    }
}