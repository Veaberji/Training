using Microsoft.EntityFrameworkCore;
using MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;
using MusiciansAPP.Domain;
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

    private DbSet<Track> Tracks => (Context as AppDbContext)?.Tracks;
}