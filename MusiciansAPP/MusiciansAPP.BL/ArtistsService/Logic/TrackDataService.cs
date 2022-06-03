using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class TrackDataService : ITrackDataService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TrackDataService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task SaveTopTracksAsync(ArtistTracksBL tracks)
    {
        var tracksNames = tracks.Tracks.Select(t => t.Name).ToList();
        var tracksFromDb = await GetTracksFromDbAsync(tracksNames, tracks.ArtistName);
        var newTracks = await CreateNotExistingTracksAsync(tracks, tracksFromDb);
        UpdateTrackPlayCount(tracks.Tracks, tracksFromDb);

        await _unitOfWork.Tracks.AddRangeAsync(newTracks);
        await _unitOfWork.CompleteAsync();
    }

    public async Task UpdateArtistTracksAsync(Album newAlbum)
    {
        var tracksNames = newAlbum.Tracks.Select(t => t.Name).ToList();
        var tracksFromDb =
            await GetTracksFromDbAsync(tracksNames, newAlbum.Artist.Name);
        var newTracks = GetNewTracks(newAlbum.Tracks, tracksFromDb);

        AddArtistToTracks(newAlbum.Artist, newTracks);
        ReplaceAlbumTracks(newAlbum, tracksFromDb, newTracks);
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

    private void AddArtistToTracks(Artist artist,
        IEnumerable<Track> newTracks)
    {
        foreach (var track in newTracks)
        {
            track.Artist = artist;
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