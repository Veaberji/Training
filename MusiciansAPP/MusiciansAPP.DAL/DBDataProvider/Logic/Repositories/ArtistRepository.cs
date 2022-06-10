using Microsoft.EntityFrameworkCore;
using MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;
using MusiciansAPP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Logic.Repositories;

public class ArtistRepository : Repository<Artist>, IArtistRepository
{
    public ArtistRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Artist>> GetTopArtistsAsync(int pageSize, int page)
    {
        return await Artists
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Artist> GetArtistDetailsAsync(string artistName)
    {
        return await Artists
            .Where(a => a.Name == artistName)
            .FirstOrDefaultAsync() ?? new Artist
            {
                Name = artistName
            };
    }

    public async Task<Artist> GetArtistWithSimilarAsync(
        string artistName, int pageSize, int page)
    {
        return await Artists.Where(a => a.Name == artistName)
            .Include(a => a.SimilarArtists
                .Skip((page - 1) * pageSize)
                .Take(pageSize))
            .FirstOrDefaultAsync();
    }

    public async Task AddOrUpdateAsync(Artist artist)
    {
        var artistFromDb = (await FindAsync(a => a.Name == artist.Name)).FirstOrDefault();
        if (artistFromDb is null)
        {
            await AddAsync(artist);
        }
        else if (!artistFromDb.IsArtistDetailsUpToDate())
        {
            UpdateArtistDetails(artist, artistFromDb);
        }
    }

    public async Task AddOrUpdateRangeAsync(IEnumerable<Artist> artists)
    {
        var artistsFromDb = await GetArtistsFromDbAsync(artists);
        var artistsFromDbNames = artistsFromDb.Select(a => a.Name).ToList();
        var newArtists = artists
            .Where(artist => IsNewItem(artistsFromDbNames, artist.Name))
            .ToList();
        await AddRangeAsync(newArtists);

        UpdateArtistImageUrl(artists, artistsFromDb);
    }

    public async Task AddOrUpdateSimilarArtistsAsync(string artistName,
        IEnumerable<Artist> similarArtists)
    {
        var artistInDB = await GetArtistWithSimilarAsync(artistName,
            pageSize: int.MaxValue, page: 1);
        if (artistInDB is null)
        {
            var newArtist = new Artist
            { Name = artistName };

            await AddSimilarToArtistAsync(newArtist, similarArtists);
            await AddAsync(newArtist);
        }
        else
        {
            await AddSimilarToArtistAsync(artistInDB, similarArtists);
        }
    }

    private DbSet<Artist> Artists => (Context as AppDbContext)?.Artists;

    private async Task<IEnumerable<Artist>> GetArtistsFromDbAsync(
        IEnumerable<Artist> artists)
    {
        var artistsNames = artists.Select(a => a.Name).ToList();
        return await FindAsync(a => artistsNames.Contains(a.Name));
    }


    private void UpdateArtistImageUrl(IEnumerable<Artist> artists,
        IEnumerable<Artist> artistsFromDb)
    {
        foreach (var artistFromDb in artistsFromDb.Where(a => !a.IsArtistHasImageUrl()))
        {
            var artist = artists.First(a => a.Name == artistFromDb.Name);
            artistFromDb.ImageUrl = artist.ImageUrl;
        }
    }

    private void UpdateArtistDetails(Artist artist, Artist artistFromDb)
    {
        artistFromDb.Biography = artist.Biography;
        artistFromDb.ImageUrl = artist.ImageUrl;
    }

    private async Task AddSimilarToArtistAsync(Artist artist,
        IEnumerable<Artist> similarArtists)
    {
        var artists = await GetSimilarArtistsAsync(similarArtists);
        var newSimilarArtists = GetNewSimilarArtists(artist, artists);
        artist.SimilarArtists.AddRange(newSimilarArtists);
    }

    private async Task<List<Artist>> GetSimilarArtistsAsync(IEnumerable<Artist> similarArtists)
    {
        var artistsFromDb = await GetArtistsFromDbAsync(similarArtists);
        var newArtists = CreateNotExistingArtists(similarArtists, artistsFromDb);
        var artists = artistsFromDb.Concat(newArtists).ToList();

        return artists;
    }

    private IEnumerable<Artist> CreateNotExistingArtists(IEnumerable<Artist> artists,
        IEnumerable<Artist> artistsFromDb)
    {
        var artistsFromDbNames = artistsFromDb.Select(a => a.Name).ToList();
        return artists
            .Where(artist => IsNewItem(artistsFromDbNames, artist.Name))
            .ToList();
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