using Microsoft.EntityFrameworkCore;
using MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;
using MusiciansAPP.Domain;
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
            .FirstOrDefaultAsync();
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

    private DbSet<Artist> Artists => (Context as AppDbContext)?.Artists;
}