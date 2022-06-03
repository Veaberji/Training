using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class ArtistDataService : IArtistDataService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ArtistDataService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task SaveTopArtistsAsync(IEnumerable<ArtistBL> artists)
    {
        var artistsFromDb = await GetArtistsFromDbAsync(artists);
        var newArtists = CreateNotExistingArtists(artists, artistsFromDb);
        UpdateArtistImageUrl(artists, artistsFromDb);

        await _unitOfWork.Artists.AddRangeAsync(newArtists);
        await _unitOfWork.CompleteAsync();
    }

    public async Task SaveArtistDetailsAsync(ArtistDetailsBL artist)
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

    public async Task SaveSimilarArtistsAsync(SimilarArtistsBL artists)
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

    private bool IsArtistHasImageUrl(Artist artist)
    {
        return artist.ImageUrl is not null;
    }

    private bool IsArtistDetailsUpToDate(Artist artist)
    {
        return IsArtistHasImageUrl(artist) && artist.Biography is not null;
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
}