using Microsoft.EntityFrameworkCore;
using MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Logic.Repositories;

public class AlbumRepository : Repository<Album>, IAlbumRepository
{
    public AlbumRepository(DbContext context) : base(context)
    {
    }

    public async Task<Album> GetAlbumDetailsAsync(string artistName, string albumName)
    {
        return await Albums.Where(
                a => a.Artist.Name == artistName && a.Name == albumName)
            .Include(a => a.Artist)
            .Include(a => a.Tracks)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Album>> GetTopAlbumsForArtistAsync(
        string artistName, int pageSize, int page)
    {
        return await Albums.Where(
                a => a.Artist.Name == artistName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    private DbSet<Album> Albums => (Context as AppDbContext)?.Albums;
}